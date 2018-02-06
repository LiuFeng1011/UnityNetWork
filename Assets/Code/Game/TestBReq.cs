using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBReq : Request {
    public override int GetProtocol()
    {
        return NetProtocols.TEST_B;
    }

    public override void Serialize(DataStream writer)
    {
        base.Serialize(writer);
    }
}
