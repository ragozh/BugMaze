using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    [SerializeField] GameObject[] _stars = default;
    [SerializeField] GameObject _lock = default;
    [SerializeField] PlayerSO _player = default;
    private bool _touchDown = false;
    private float _nextTime;
    private int _level = -1;
    private int _star = 0;    
    private TextMeshPro _levelLabel;
    private float _delayTouch = 0.5f;

    public int Star
    {
        get => _star;
        set
        {
            if(ValidateStar(value))
            {
                _star = value;
                SetStar(_star);
            }
        }
    }
    private void Awake() => _levelLabel = GetComponentInChildren<TextMeshPro>();
    private void Update()
    {
        if (Time.time > _nextTime)
        {
            _touchDown = false;
        }
    }
    public void SetLevelLabel(int level)
    {
        _levelLabel.text = level + "";
        _level = level;
    }
    public void DisableLock() => _lock.SetActive(false);
    public void EnableLock() => _lock.SetActive(true);
    public void ResetStar()
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].SetActive(false);
        }
    }
    private void SetStar(int star = 1)
    {        
        for (int i = 0; i < _stars.Length; i++)
        {
            if (i < star)
                _stars[i].SetActive(true);
            else
                _stars[i].SetActive(false);
        }
    }
    private bool ValidateStar(int star)
    {
        if (_lock.activeSelf)
        {
            Debug.LogWarning($"{name} did not unlock yet.");
            return false;
        }
        if (star > 3 || star < 0)
        {            
            Debug.LogWarning($"Star must be in range 0 - 3");
            return false;
        }
        return true;
    }
    private void OnMouseDown()
    {
        _touchDown = true;
        _nextTime = Time.time + _delayTouch;
    }
    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject() 
            || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //Debug.Log("UI");
            return;
        }
        if (_touchDown && !_lock.activeSelf)
        {
            _player.currentLevel = _level;
            SceneManager.LoadScene("Ingame");
        }
    }
}
