using UnityEngine;
using System.IO;

public class DataWriter
{
    private BinaryWriter _writer;
    public DataWriter(BinaryWriter writer) => _writer = writer;
    public void Write(int value) => _writer.Write(value);
    public void Write(float value) => _writer.Write(value);
}