using System;
using System.IO;
using UnityEngine;

public class DataStream
{
	private BinaryReader mBinReader;
	private BinaryWriter mBinWriter;
	private MemoryStream mMemStream;
	private bool mBEMode;//big endian mode
	
	public DataStream(bool isBigEndian)
	{
		mMemStream = new MemoryStream();
		InitWithMemoryStream(mMemStream, isBigEndian);
	}
	
	public DataStream(byte[] buffer, bool isBigEndian)
	{
		mMemStream = new MemoryStream(buffer);
		InitWithMemoryStream(mMemStream, isBigEndian);
	}
	
	public DataStream(byte[] buffer, int index, int count, bool isBigEndian)
	{
		mMemStream = new MemoryStream(buffer, index, count);
		InitWithMemoryStream(mMemStream, isBigEndian);
	}
	
	private void InitWithMemoryStream(MemoryStream ms, bool isBigEndian)
	{
		mBinReader = new BinaryReader(mMemStream);
		mBinWriter = new BinaryWriter(mMemStream);
		mBEMode = isBigEndian;
	}
	
	public void Close()
	{
		mMemStream.Close();
		mBinReader.Close();
		mBinWriter.Close();
	}
	
	public void SetBigEndian(bool isBigEndian)
	{
		mBEMode = isBigEndian;
	}
	
	public bool IsBigEndian()
	{
		return mBEMode;
	}
	
	public long Position
	{
		get 
		{
			return mMemStream.Position;
		}
		set 
		{
			mMemStream.Position = value;
		}
	}
	
	public long Length
	{
		get
		{
			return mMemStream.Length;
		}
	}

	public byte[] ToByteArray()
	{
		//return mMemStream.GetBuffer();
		return mMemStream.ToArray();
	}
	
	
	
	public long Seek(long offset, SeekOrigin loc)
	{
		return mMemStream.Seek(offset, loc);
	}
	
	public void WriteRaw(byte[] bytes)
	{
		mBinWriter.Write(bytes);
	}
	
	public void WriteRaw(byte[] bytes, int offset, int count)
	{
		mBinWriter.Write(bytes, offset, count);
	}
	
	public void WriteByte(byte value)
	{
		mBinWriter.Write(value);
	}
	
	public byte ReadByte()
	{
		return mBinReader.ReadByte();
	}
	
	public void WriteInt16(UInt16 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public UInt16 ReadInt16()
	{
		UInt16 val =  mBinReader.ReadUInt16();
		if (mBEMode)
			return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	public void WriteInt32(UInt32 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public UInt32 ReadInt32()
	{
		UInt32 val = mBinReader.ReadUInt32();
		if (mBEMode)
			return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	public void WriteInt64(UInt64 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public UInt64 ReadInt64()
	{
		UInt64 val = mBinReader.ReadUInt64();
		if (mBEMode)
			return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	//public void WriteString8(string value)
	//{
	//    WriteInteger(BitConverter.GetBytes((byte)value.Length));
	//    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
	//    //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
	//    mBinWriter.Write(encoding.GetBytes(value));
	//}
	
	public void WriteString8(string value)
	{
		// System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
		byte[] bytes = encoding.GetBytes(value);
		mBinWriter.Write((byte)bytes.Length);
		mBinWriter.Write(bytes);
	}
	
	public string ReadString8()
	{
		int len = ReadByte();
		byte[] bytes = mBinReader.ReadBytes(len);
		// System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
		return encoding.GetString(bytes);
	}
	
	public void WriteString16(string value)
	{
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
		byte[] data = encoding.GetBytes(value);
		WriteInteger(BitConverter.GetBytes((Int16)data.Length));
		//System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
		mBinWriter.Write(data);
	}
	
	public string ReadString16()
	{
		ushort len = ReadInt16();
		byte[] bytes = mBinReader.ReadBytes(len);
		//  System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
		return encoding.GetString(bytes);
	}
	
	private void WriteInteger(byte[] bytes)
	{
		if (mBEMode)
			FlipBytes(bytes);
		mBinWriter.Write(bytes);
	}
	
	private byte[] FlipBytes(byte[] bytes)
	{
		//Array.Reverse(bytes); 
		for (int i = 0, j = bytes.Length - 1; i < j; ++i, --j)
		{
			byte temp = bytes[i];
			bytes[i] = bytes[j];
			bytes[j] = temp;
		}
		return bytes;
	}
	
	
	/// <summary>
	/// signed型数据读写
	/// </summary>
	public void WriteSByte(sbyte value)
	{
		mBinWriter.Write(value);
	}
	
	public sbyte ReadSByte()
	{
		return mBinReader.ReadSByte();
	}
	
	public void WriteSInt16(Int16 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public Int16 ReadSInt16()
	{
		Int16 val = mBinReader.ReadInt16();
		if (mBEMode)
			return BitConverter.ToInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	public void WriteSInt32(Int32 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public Int32 ReadSInt32()
	{
		Int32 val = mBinReader.ReadInt32();
		if (mBEMode)
			return BitConverter.ToInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	public void WriteSInt64(Int64 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public Int64 ReadSInt64()
	{
		Int64 val = mBinReader.ReadInt64();
		if (mBEMode)
			return BitConverter.ToInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	
	/// <summary>
	/// Unsigned型数据读写
	/// </summary>
	public void WriteUByte(byte value)
	{
		mBinWriter.Write(value);
	}
	
	public byte ReadUByte()
	{
		return mBinReader.ReadByte();
	}
	
	public void WriteUInt16(UInt16 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public UInt16 ReadUInt16()
	{
		UInt16 val = mBinReader.ReadUInt16();
		if (mBEMode)
			return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	public void WriteUInt32(UInt32 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public UInt32 ReadUInt32()
	{
		UInt32 val = mBinReader.ReadUInt32();
		if (mBEMode)
			return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
	
	public void WriteUInt64(UInt64 value)
	{
		WriteInteger(BitConverter.GetBytes(value));
	}
	
	public UInt64 ReadUInt64()
	{
		UInt64 val = mBinReader.ReadUInt64();
		if (mBEMode)
			return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
		return val;
	}
}