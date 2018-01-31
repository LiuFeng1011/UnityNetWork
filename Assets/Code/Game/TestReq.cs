using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReq : Request {
    public int testInt;
    public string testString;

    public TestReq(int testInt,string testString){
        this.testInt = testInt;
        this.testString = testString;
    }

    public override int GetProtocol()
    {
        return NetProtocols.TEST;
    }

    public override void Serialize(DataStream writer)
    {
        base.Serialize(writer);

        writer.WriteSInt32(testInt);
        writer.WriteString16(testString);
    }
}
