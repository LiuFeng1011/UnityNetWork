using UnityEngine;
using System.Collections;

public abstract class Resp {

	public virtual int GetProtocol(){
        Debug.LogError("can't get Protocol");
		return 0;
	}

	public virtual void Serialize(DataStream writer)
	{
		//no need to implement as this is a response
	}

	public virtual void Deserialize(DataStream reader)
	{
	}

}
