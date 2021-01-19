using TMPro;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] GameObject[] _star = default;
    [SerializeField] GameObject _lock = default;
    private TextMeshPro _levelLabel;
    private void Awake() => _levelLabel = GetComponentInChildren<TextMeshPro>();
    public void SetLevelLabel(int level) => _levelLabel.text = level + "";
    [ContextMenu("Unlock")]
    public void DisableLock() => _lock.SetActive(false);
    public void ResetStar()
    {
        for (int i = 0; i < _star.Length; i++)
        {
            _star[i].SetActive(false);
        }
    }
    public void SetStar(int star = 1)
    {
        if (_lock.activeSelf)
        {
            Debug.LogWarning($"{name} did not unlock yet.");
            return;
        }
        if (star > 3 || star < 0)
        {            
            Debug.LogWarning($"Star must be in range 1 - 3");
            return;
        }
        for (int i = 0; i < star; i++)
        {
            _star[i].SetActive(true);
        }
    }
}
