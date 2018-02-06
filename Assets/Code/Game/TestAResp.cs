using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAResp : Resp {
    public int testint;
    public long testlong;
    public string teststring;

    public override int GetProtocol()
    {
        return NetProtocols.TEST_A;
    }

    public override void Deserialize(DataStream reader)
    {
        base.Deserialize(reader);
        testint = reader.ReadSInt32();
        testlong = reader.ReadSInt64();
        teststring = reader.ReadString16();
    }

}
