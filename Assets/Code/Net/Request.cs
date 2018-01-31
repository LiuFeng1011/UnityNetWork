using UnityEngine;
using System.Collections;

public abstract class Request {

	public virtual int GetProtocol(){
        Debug.LogError("can't get Protocol");
		return 0;
	}
	public virtual void Serialize(DataStream writer)
	{
		writer.WriteSInt32(GetProtocol());
		writer.WriteByte(0);
	}

	public virtual void Deserialize(DataStream reader)
	{
		//no need to implement as this is a request
	}

	public void Send(){
		SocketHelper.GetInstance().SendMessage(this);
	}
}
