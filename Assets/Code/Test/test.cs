using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class test : MonoBehaviour {
	const int height = 40;
	const int width = 150;

	void Awake()
	{
        ProtoManager.Instance.AddRespDelegate(NetProtocols.TEST,Response);
	}
	
	void OnDisable()
	{
        ProtoManager.Instance.DelRespDelegate(NetProtocols.TEST,Response);
	}

	int high;
	void OnGUI()  
	{
		high = 10;
		if(CreateBtn( "connect"))  
		{   
			SocketHelper.GetInstance().Connect("ID",1,ConnectCallBack,null);
		}  

		if(CreateBtn(  "send"))  
		{   
            TestReq req = new TestReq(1, "2");
            req.Send();
		}  

	}

	public bool CreateBtn(string btnname){
		bool b = GUI.Button (new Rect (width, high,100,height),btnname);
		high += height+5;
		return b;
	}

	void Response(Resp r){
        TestResp resp = (TestResp)r;

        Debug.Log(resp.data);
	}

	public void ConnectCallBack(){
        Debug.Log("Connect success!");
	}

	public void TestCallBack(){
		
	}
}
