using UnityEngine;
using System.IO;

public class DataReader
{
    private BinaryReader _reader;
    public DataReader(BinaryReader reader) => _reader = reader;
    public int ReadInt() => _reader.ReadInt32();
    public float ReadFloat() => _reader.ReadSingle();
}