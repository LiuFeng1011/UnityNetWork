using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryGameResp : Resp {

    public override int GetProtocol()
    {
        return NetProtocols.ENTRY_GAME;
    }

    public override void Deserialize(DataStream reader)
    {
        base.Deserialize(reader);
    }
}
