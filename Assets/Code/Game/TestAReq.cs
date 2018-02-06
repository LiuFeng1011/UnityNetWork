using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAReq : Request {

    int testint;
    long testlong;
    string teststring;

    public TestAReq(int testint,long testlong,string teststring){
        this.testint = testint;
        this.testlong = testlong;
        this.teststring = teststring;
    }

    public override int GetProtocol()
    {
        return NetProtocols.TEST_A;
    }

    public override void Serialize(DataStream writer)
    {
        base.Serialize(writer);

        writer.WriteSInt32(testint);
        writer.WriteSInt64(testlong);
        writer.WriteString16(teststring);
    }
}
