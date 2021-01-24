using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab = default;
    [SerializeField] private Bug _bugPrefab = default;
    [SerializeField] private Target _gate = default;
    [SerializeField] private Text _levelLabel = default;
    [SerializeField] private PlayerSO _player = default;
     private Bug _bug;
    private Cell[,] _cells;
    private Stack<Cell> _visitedCell;
    private List<Cell> _doneCell;
    private const float _distance = 0.56f, _initX = -2.52f, _initY = 2.5f;
    private const int _column = 10, _row = 13;
    private int count = 0, _randomStepToGenerateGate = 0;
    private Cell[] _path;
    private void Awake()
    {
        _visitedCell = new Stack<Cell>();
        _doneCell = new List<Cell>();
        _cells = new Cell[10, 13];
        _levelLabel.text = "No. " + _player.currentLevel;
        _randomStepToGenerateGate = Random.Range(5, 60);
        for (int j = 0; j < _row; j++)
        {
            for (int i = 0; i < _column; i++)
            {
                CreateCell(i, j);
            }
        }
        _bug = Instantiate(_bugPrefab);
        Retry();
        GenerateMaze();
    }
    public void Retry()
    {        
        _bug.Reset(_cells[0, 0]);
    }

    private void CreateCell(int i, int j)
    {
        var cell = Instantiate(_cellPrefab);
        cell.name = i + "" + j;
        cell.transform.SetParent(transform);
        cell.transform.position = new Vector3(
            _initX + i * _distance,
            _initY - j * _distance,
            0
        );
        if (i > 0)
        {
            cell.adjacentCells[(int) WallPosition.Left] = _cells[i - 1, j];
            _cells[i - 1, j].adjacentCells[(int) WallPosition.Right] = cell;
        }
        if (j > 0) 
        {
            cell.adjacentCells[(int) WallPosition.Up] = _cells[i, j - 1];
            _cells[i, j - 1].adjacentCells[(int) WallPosition.Down] = cell;
        }      
        _cells[i,j] = cell;
    }
    private void GenerateMaze()
    {
        MoveToCell(null, _cells[0, 0]);
    }
    private void MoveToCell(Cell from, Cell to)
    {
        if (!to._isVisited)
        {
            CollapseWallBetween(from, to); // collapse wall if moved to unvisited cell
            to._isVisited = true;
        }
        if (CheckIfCellDone(to.adjacentCells, out var randomCell)) // check if cell is done (all adjacent cells visited)
        {
            if (count == _randomStepToGenerateGate)
            {
                var target = Instantiate(_gate);
                target.transform.position = to.transform.position;
                _visitedCell.Push(to);
                _path = new Cell[_visitedCell.Count];
                _visitedCell.CopyTo(_path, 0);
                _visitedCell.Pop();
                System.Array.Reverse(_path);
            }
            count++;
            _doneCell.Add(to);
            if (_doneCell.Count == 10 * 13) // stop if all cells mark as done
                return;
            MoveToCell(to, _visitedCell.Pop()); // bactrack
        }
        else
        {
            _visitedCell.Push(to); // push current cell to backtrack stack
            MoveToCell(to, randomCell); // random foward
        }
    }

    private void CollapseWallBetween(Cell from, Cell to)
    {            
        if (from == null)
            return;
        var wallPositionFrom = -1;
        for (int i = 0; i < from.adjacentCells.Length; i++)
        {
            if (from.adjacentCells[i] && (from.adjacentCells[i] == to))
                wallPositionFrom = i;
        }
        from.RemoveWall(wallPositionFrom);
        var direction = wallPositionFrom > 1 ? -1 : 1;
        to.RemoveWall(wallPositionFrom + 2 * direction);
    }

    private bool CheckIfCellDone(Cell[] cellsToCheck, out Cell randomCell)
    {
        randomCell = null;
        List<Cell> unVisitedCells = new List<Cell>();
        for (int i = 0; i < cellsToCheck.Length; i++)
        {
            if ((cellsToCheck[i] != null) && (!cellsToCheck[i]._isVisited))
            {                
                unVisitedCells.Add(cellsToCheck[i]);
            }
        }
        if (unVisitedCells.Count > 0)
            randomCell = unVisitedCells[Random.Range(0, unVisitedCells.Count)];
        return unVisitedCells.Count == 0;
    }
    public void ShowHint() => _bug.ShowHint(_path);
    public void AutoMove() => _bug.Move(_path);
    public void ReturnToMap()
    {
        SceneManager.LoadScene("Map");
    }
}
