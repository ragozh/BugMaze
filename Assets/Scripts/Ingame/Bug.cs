using UnityEngine;
using System.Collections;
using System;

public class Bug : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.5f;
    private LineRenderer _line;
    private Coroutine _moveCoroutine;
    private bool _moving;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        
    }
    public void ShowHint(Cell[] path)
    {
        _line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 1.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.7f, 0.7f), new GradientAlphaKey(0.7f, 0.7f) }
        );
        _line.colorGradient = gradient;
        _line.positionCount = path.Length;
        _line.widthMultiplier = 0.1f;
        for (int i = 0; i < path.Length; i++)
        {
            _line.SetPosition(i, path[i].transform.position);
        }
    }
    public void Move(Cell[] path)
    {
        if (!_moving)
        {
            //Debug.Log("Start moving");
            _moveCoroutine = StartCoroutine(MoveToCell(path));
            _moving = true;
        }
    }
    public void Reset(Cell startCell)
    {
        StopAllCoroutines();
        _moving = false;
        transform.SetParent(startCell.transform);
        transform.position = startCell.transform.position;
        _line.positionCount = 0;
    }
    private IEnumerator MoveToCell(Cell[] path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            transform.rotation = Quaternion.identity;
            if (path[i].transform.position.x == transform.position.x)
            {
                yield return StartCoroutine(VerticalMove(path[i]));
            }
            else if (path[i].transform.position.y == transform.position.y)
            {
                yield return StartCoroutine(HorizontalMove(path[i]));
            }
        }
        _moving = false;
    }

    private IEnumerator HorizontalMove(Cell cell)
    {
        if (cell.transform.position.x < transform.position.x)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            while(transform.position.x > cell.transform.position.x)
            {
                transform.position += Vector3.left * Time.deltaTime * _moveSpeed;
                yield return null;
            }
        }
        else if (cell.transform.position.x > transform.position.x)
        {
            transform.Rotate(new Vector3(0, 0, -90));
            while(transform.position.x < cell.transform.position.x)
            {
                transform.position -= Vector3.left * Time.deltaTime * _moveSpeed;
                yield return null;
            }
        }
        transform.position = cell.transform.position;
        transform.SetParent(cell.transform);
    }

    private IEnumerator VerticalMove(Cell cell)
    {
        if (cell.transform.position.y < transform.position.y)
        {
            transform.Rotate(new Vector3(0, 0, 180));
            while(transform.position.y > cell.transform.position.y)
            {
                transform.position -= Vector3.up * Time.deltaTime * _moveSpeed;
                yield return null;
            }
        }
        else if (cell.transform.position.y > transform.position.y)
        {
            while(transform.position.y < cell.transform.position.y)
            {
                transform.position += Vector3.up * Time.deltaTime * _moveSpeed;
                yield return null;
            }
        }
        transform.position = cell.transform.position;
        transform.SetParent(cell.transform);
    }
}
