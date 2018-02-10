using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class test : MonoBehaviour {
	const int height = 40;
	const int width = 150;

	void Awake()
    {
        ProtoManager.Instance.AddRespDelegate(NetProtocols.ENTRY_GAME, Response);
        ProtoManager.Instance.AddRespDelegate(NetProtocols.TEST_A,Response);
        ProtoManager.Instance.AddRespDelegate(NetProtocols.TEST_B, Response);

        int width = (int)(Screen.currentResolution.width * 0.3f);
        int height = (int)(Screen.currentResolution.height * 0.3f);
        Debug.Log("width : " + width + " Screen.currentResolution.width : " + Screen.currentResolution.width);
        Debug.Log("height : " + height + " Screen.currentResolution.height : " + Screen.currentResolution.height);
        Screen.SetResolution(width, height, true);
	}
	
	void OnDisable()
    {
        ProtoManager.Instance.DelRespDelegate(NetProtocols.ENTRY_GAME, Response);
        ProtoManager.Instance.DelRespDelegate(NetProtocols.TEST_A,Response);
        ProtoManager.Instance.DelRespDelegate(NetProtocols.TEST_B, Response);
	}

	int high;
	void OnGUI()  
	{
		high = 10;
		if(CreateBtn( "connect"))  
		{   
            SocketHelper.GetInstance().Connect("192.168.1.91",9999,ConnectCallBack,null);
		}  
        if(CreateBtn(  "Entry Game"))  
        {   
            EntryGameReq req = new EntryGameReq(123);
            req.Send();
        } 

		if(CreateBtn(  "sendA"))  
		{   
            TestAReq req = new TestAReq(1,9999999999999,"123abcd的");
            req.Send();
		}  
        if(CreateBtn(  "sendB"))  
        {   
            TestBReq req = new TestBReq();
            req.Send();
        }  
	}

	public bool CreateBtn(string btnname){
		bool b = GUI.Button (new Rect (width, high,100,height),btnname);
		high += height+5;
		return b;
	}

	void Response(Resp r){
        Debug.Log( " receive server msg : " + r.GetProtocol());
        Debug.Log(NetProtocols.TEST_A);
        if(r.GetProtocol() == NetProtocols.TEST_A){
            TestAResp resp = (TestAResp)r;
            Debug.Log("int : " + resp.testint);
            Debug.Log("long : " + resp.testlong);
            Debug.Log("string : " + resp.teststring);
        }

	}

	public void ConnectCallBack(){
        Debug.Log("Connect success!");
	}

	public void TestCallBack(){
		
	}
}
