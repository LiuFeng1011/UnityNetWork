using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBResp : Resp {

    public override int GetProtocol()
    {
        return NetProtocols.TEST_B;
    }

    public override void Deserialize(DataStream reader)
    {
        base.Deserialize(reader);
    }
}
