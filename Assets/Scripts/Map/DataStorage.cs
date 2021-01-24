using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataStorage : MonoBehaviour
{
    private string _savePath;
    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "map_data");
        //Debug.Log(_savePath);
    }
    public void Save(MapData mapData)
    {
        using(var writer = new BinaryWriter(File.Open(_savePath, FileMode.OpenOrCreate)))
        {
            var data = mapData.mapStars;
            writer.Write(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                writer.Write(data[i]);
            }
        }
    }
    public void Load(MapData mapData)
    {
        using(var reader = new BinaryReader(File.Open(_savePath, FileMode.Open)))
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                mapData.mapStars.Add(reader.ReadInt32());
            }
        }
    }
}
