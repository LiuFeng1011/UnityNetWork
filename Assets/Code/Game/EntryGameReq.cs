using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryGameReq : Request {

    long uid;
    public override int GetProtocol()
    {
        return NetProtocols.ENTRY_GAME;
    }
    public EntryGameReq(long uid){
        this.uid = uid;
    }

    public override void Serialize(DataStream writer)
    {
        base.Serialize(writer);
        writer.WriteSInt64(uid);
    }
}
