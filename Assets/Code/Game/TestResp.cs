using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResp : Resp {
    public string data;

    public override int GetProtocol()
    {
        return NetProtocols.TEST;
    }

    public override void Deserialize(DataStream reader)
    {
        base.Deserialize(reader);
        data = reader.ReadString16();
    }
}
