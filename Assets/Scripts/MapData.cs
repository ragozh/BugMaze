using System.Collections.Generic;
public class MapData
{
    public List<int> mapStars;
    public MapData()
    {
        mapStars = new List<int>();
    }
    public void ClearOldData()
    {
        mapStars.Clear();
    }
}
