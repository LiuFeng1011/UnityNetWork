using System;
using System.Collections;
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Net;  
using System.Net.Sockets;  
using System.Threading;  
using UnityEngine; 


public class SocketHelper : MonoBehaviour{

	private DataHolder mDataHolder = new DataHolder();
	private static SocketHelper socketHelper;  


	public delegate void ConnectCallback();

	ConnectCallback connectDelegate = null;
	ConnectCallback connectFailedDelegate = null;

	Queue<byte[]> dataQueue = new Queue<byte[]>();

	Queue<Request> sendDataQueue = new Queue<Request>();

	private Socket socket;  
	bool isStopReceive = true;

	public static SocketHelper GetInstance()  
	{  
		return socketHelper;  
	}  

	void Awake(){
		socketHelper = this;
	}

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <returns>The connect.</returns>
    /// <param name="serverIp">Server ip.</param>
    /// <param name="serverPort">Server port.</param>
    /// <param name="connectCallback">Connect callback.</param>
    /// <param name="connectFailedCallback">Connect failed callback.</param>
    public void Connect(string serverIp,int serverPort,ConnectCallback connectCallback,ConnectCallback connectFailedCallback){
		connectDelegate = connectCallback;
		connectFailedDelegate = connectFailedCallback;

		//采用TCP方式连接  
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

		//服务器IP地址  
        IPAddress address = IPAddress.Parse(serverIp);  

		//服务器端口  
        IPEndPoint endpoint = new IPEndPoint(address,serverPort);  

		//异步连接,连接成功调用connectCallback方法  
		IAsyncResult result = socket.BeginConnect(endpoint, new AsyncCallback(ConnectedCallback), socket);  

		//这里做一个超时的监测，当连接超过5秒还没成功表示超时  
		bool success = result.AsyncWaitHandle.WaitOne(5000, true);  
		if (!success)  
		{  
			//超时  
			Closed();  
			if(connectFailedDelegate != null){
				connectFailedDelegate();
			}
		}  
		else  
		{  
			//与socket建立连接成功，开启线程接受服务端数据。  
			isStopReceive = false;
			Thread thread = new Thread(new ThreadStart(ReceiveSorket));  
			thread.IsBackground = true;  
			thread.Start();  
		}  
        RegisterResp.RegisterAll();
	}

	private void ConnectedCallback(IAsyncResult asyncConnect)  
	{  
		if (!socket.Connected)  {
			if(connectFailedDelegate != null){
				connectFailedDelegate();
			}
			return;
		}

		if(connectDelegate != null){
			connectDelegate();
		}
	}  

	private void ReceiveSorket(){
		mDataHolder.Reset();
		while(!isStopReceive){
			if (!socket.Connected)  
			{  
				//与服务器断开连接跳出循环  
                Debug.Log("Failed to clientSocket server.");  
				socket.Close();  
				break;  
			}  

			try  
			{  
				//接受数据保存至bytes当中  
				byte[] bytes = new byte[4096];  
				//Receive方法中会一直等待服务端回发消息  
				//如果没有回发会一直在这里等着。  

				int i = socket.Receive(bytes); 

				if (i <= 0)  
				{  
					socket.Close();  
					break;  
				}  
				mDataHolder.PushData(bytes, i);

				while(mDataHolder.IsFinished()){
					dataQueue.Enqueue(mDataHolder.mRecvData);

					mDataHolder.RemoveFromHead();
				}
			}  
			catch (Exception e)  
			{  
                Debug.Log("Failed to clientSocket error." + e);  
				socket.Close();  
				break;  
			}  
		}
	}

	//接收到数据放入数据队列，按顺序取出
	void Update(){
		if(dataQueue.Count > 0){
			Resp resp = ProtoManager.Instance.TryDeserialize(dataQueue.Dequeue());
		}

		if(sendDataQueue.Count > 0){
			Send();
		}
	}


	//关闭Socket  
	public void Closed()  
	{  
		isStopReceive = true;

		if (socket != null && socket.Connected)  
		{  
			socket.Shutdown(SocketShutdown.Both);  
			socket.Close();  
		}  
		socket = null;  
	}  

	public bool isConnect(){
		return socket != null && socket.Connected;
	}

	public void SendMessage(Request req)  
	{  
		sendDataQueue.Enqueue(req);
	}  

	private void Send(){
		if(socket == null){
			return;
		}
		if (!socket.Connected)  
		{  
			Closed();
			return;  
		}  
		try  
		{  
			Request req = sendDataQueue.Dequeue();

			DataStream bufferWriter = new DataStream(true);
			req.Serialize(bufferWriter);
			byte[] msg = bufferWriter.ToByteArray();

			byte[] buffer = new byte[msg.Length + 4];
			DataStream writer = new DataStream(buffer, true);

			writer.WriteInt32((uint)msg.Length);//增加数据长度
			writer.WriteRaw(msg);

			byte[] data = writer.ToByteArray();

			IAsyncResult asyncSend = socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);  
			bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);  
			if (!success)  
			{  
				Closed(); 
			}  
		}  
		catch  (Exception e)
		{  
            Debug.Log("send error : " + e.ToString());
		} 
	}

	private void SendCallback(IAsyncResult asyncConnect)  
	{  
        Debug.Log("send success");  
	}  

	void OnDestroy(){ 
		Closed();  
	}
}
