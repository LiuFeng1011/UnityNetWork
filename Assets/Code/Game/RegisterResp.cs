using UnityEngine;
using System.Collections;

public class RegisterResp  {
	//所有response必须要在这里注册
	public static void RegisterAll(){
        ProtoManager.Instance.AddProtocol<TestResp>(NetProtocols.TEST);
	}
}
