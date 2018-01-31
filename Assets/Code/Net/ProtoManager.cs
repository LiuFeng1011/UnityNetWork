using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProtoManager
{
	private static ProtoManager mInstance;
	private Dictionary<int, Func<DataStream, Resp>> mProtocolMapping;

	public delegate void responseDelegate(Resp resp);
	private Dictionary<int, List<responseDelegate>> mDelegateMapping;

	private ProtoManager()
	{
		mProtocolMapping = new Dictionary<int, Func<DataStream, Resp>>();
		mDelegateMapping = new Dictionary<int, List<responseDelegate>>();
	}



	public void AddProtocol<T>(int protocol) where T: Resp, new()
	{
		if (mProtocolMapping.ContainsKey(protocol))
		{
			mProtocolMapping.Remove(protocol);
		}

		mProtocolMapping.Add(protocol, 
			(stream) => {
				T data = new T();
				data.Deserialize(stream);
				return data;
			});
	}


	/// <summary>
	/// 添加代理，在接受到服务器数据时会下发数据
	/// </summary>
	/// <param name="protocol">Protocol.</param>
	/// <param name="d">D.</param>
	public void AddRespDelegate(int protocol,responseDelegate d){
		List<responseDelegate>  dels ;
		if (mDelegateMapping.ContainsKey(protocol))
		{
			dels = mDelegateMapping[protocol];
			for(int i = 0 ; i < dels.Count ; i ++){
				if(dels[i] == d){
					return;
				}
			}
		}else{
			dels = new List<responseDelegate>();
			mDelegateMapping.Add(protocol,dels);
		}
		dels.Add(d);

	}
	public void DelRespDelegate(int protocol,responseDelegate d){
		if (mDelegateMapping.ContainsKey(protocol))
		{
			List<responseDelegate> dels = mDelegateMapping[protocol];
			dels.Remove(d);
		}
	}

	public Resp TryDeserialize(byte[] buffer)
	{
		DataStream stream = new DataStream(buffer, true);

		int protocol = stream.ReadSInt32();
		Resp ret = null;
		if (mProtocolMapping.ContainsKey(protocol))
		{
			ret = mProtocolMapping[protocol](stream);
			if(ret != null){
				if(mDelegateMapping.ContainsKey(protocol)){
					List<responseDelegate> dels = mDelegateMapping[protocol];
					for(int i = 0 ; i < dels.Count ; i ++){
						dels[i](ret);
					}
				}
			}
		}else{
            Debug.Log("no register protocol : " + protocol +"!please reg to RegisterResp.");
		}

		return ret;
	}

	public static ProtoManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new ProtoManager();
			}
			return mInstance;
		}
	}

}


