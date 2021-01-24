using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private Transform _map = default;
    private Camera _mainCamera;
    private Vector3 _startPosition, _endPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        TouchController();
#if UNTIY_EDITOR
        MouseController();
#endif
    }
    private void TouchController()
    {
        if (Input.touchCount > 0)
        {
            var worldPosition = _mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position) + Vector3.forward * 10;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {   
                OnBeginDrag(worldPosition);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved 
                || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
                OnContinueDrag(worldPosition);
            }
        }
    }
#if UNTIY_EDITOR
    private void MouseController()
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10f;
        if (Input.GetMouseButtonDown(0))
        {
            OnBeginDrag(worldPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("UI");
                return;
            }
            OnContinueDrag(worldPosition);
        }
    }
#endif
    private void OnBeginDrag(Vector3 worldPosition)
    {
        _startPosition = worldPosition;
    }

    private void OnContinueDrag(Vector3 worldPosition)
    {
        _endPosition = worldPosition;
        var dragMagnitude = _endPosition.y - _startPosition.y;
        if (dragMagnitude == 0) return;
        Scroll(dragMagnitude);
    }

    private void Scroll(float scrollSpeed)
    {
        if ((_map.position.y > 0 && scrollSpeed > 0) || 
            (_map.position.y < -367 && scrollSpeed < 0)) 
            return;
        _map.position += Vector3.up * Time.deltaTime * 2 * scrollSpeed;
    }
}
