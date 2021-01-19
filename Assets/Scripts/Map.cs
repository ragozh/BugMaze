using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Stage _stage = default;
    [SerializeField] private Transform _horizontalLine = default;
    [SerializeField] private Transform _verticalLine = default;
    [SerializeField] private Transform _tutorial = default;
    private List<Stage> _listStage;
    private List<Stage> _unlockedStages;
    private bool _isRight = true;
    private float _horizontalDistance = 4f / 3f;
    private float _verticalDistance = 3f / 2f;
    private const int _stageCount = 999;
    private void Awake()
    {
        _listStage = new List<Stage>();
        _unlockedStages = new List<Stage>();
        GenerateMap();
    }

    private void GenerateMap()
    {
        var column = 0;
        var row = 0;
        for (int i = 0; i <= _stageCount; i++)
        {
            GenerateStage(column, row, i);

            if (column == 0)
            {
                GenerateHorizontalLine(row);
            }
            if (column == 3)
            {
                GenerateVerticalLine(column, row);
                column = -1;
                row++;
            }
            column++;
        }
    }

    private void GenerateStage(int column, int row, int i)
    {
        if (i == 0) return;
        var stage = Instantiate(_stage);
        stage.transform.SetParent(transform);
        stage.name = "Stage " + i;
        stage.SetLevelLabel(i);
        //var stagePosX = _tutorial.position.x + _horizontalDistance * column;
        var stagePosX = _isRight ?
            _tutorial.position.x + _horizontalDistance * column:
            _tutorial.position.x + _horizontalDistance * (3 - column);
        var stagePosY = _tutorial.position.y + _verticalDistance * row;
        stage.transform.position = new Vector3(
            stagePosX,
            stagePosY,
            0
        );
        _listStage.Add(stage);
    }

    private void GenerateVerticalLine(int column, int row)
    {
        var line = Instantiate(_verticalLine);
        line.SetParent(transform);
        var linePosX = _isRight ?
            _tutorial.position.x + column * _horizontalDistance :
            _tutorial.position.x;
        var linePosY = (_tutorial.position.y + _verticalDistance * row) + 0.8f;
        line.position = new Vector3(
            linePosX,
            linePosY,
            line.position.z
        );
        _isRight = !_isRight;
    }

    private void GenerateHorizontalLine(int row)
    {
        var line = Instantiate(_horizontalLine);
        line.SetParent(transform);
        line.position = new Vector3(
            line.position.x,
            _tutorial.position.y + row * _verticalDistance,
            line.position.z
        );
    }

    public void RandomStage()
    {
        var stageUnlocked = Random.Range(1, 999);
        Debug.Log(stageUnlocked);;
        for (int i = 0; i < stageUnlocked; i++)
        {
            _listStage[i].DisableLock();
            _unlockedStages.Add(_listStage[i]);
        }
        foreach (var item in _unlockedStages)
        {
            var star = Random.Range(1, 4);
            item.SetStar(star);
        }
    }
}