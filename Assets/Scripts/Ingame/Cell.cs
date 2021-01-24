using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject[] _walls = default;
    public Cell[] adjacentCells;
    public bool _isVisited = false;
    public bool _isDone = false;
    private void Awake()
    {
        adjacentCells = new Cell[4];
    }
    public void RemoveWall(int wallIndex)
    {
        if (wallIndex < 0)
            return;
        _walls[wallIndex].gameObject.SetActive(false);
    }
}
public enum WallPosition{
    Left, Up, Right, Down
}
