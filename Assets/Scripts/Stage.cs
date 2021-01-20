using TMPro;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] GameObject[] _stars = default;
    [SerializeField] GameObject _lock = default;
    private int _star = 0;    
    private TextMeshPro _levelLabel;
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
    public void SetLevelLabel(int level) => _levelLabel.text = level + "";
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
}
