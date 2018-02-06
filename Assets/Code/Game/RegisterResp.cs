using UnityEngine;
using System.Collections;

public class RegisterResp  {
	//所有response必须要在这里注册
    public static void RegisterAll(){
        ProtoManager.Instance.AddProtocol<EntryGameResp>(NetProtocols.ENTRY_GAME);
        ProtoManager.Instance.AddProtocol<TestAResp>(NetProtocols.TEST_A);
        ProtoManager.Instance.AddProtocol<TestBResp>(NetProtocols.TEST_B);
	}
}
