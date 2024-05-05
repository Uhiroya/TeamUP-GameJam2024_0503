using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotator : MonoBehaviour , IDragHandler
{
    public Vector2 RotationSpeed;

    [SerializeField]  private float _threshold;
    private Vector2 _lastMousePosition;
    private Vector2 _newAngle = new Vector2(0, 0);

    private Quaternion _defaultRotation;
    [SerializeField] float _limitRotationX = 80f;
    [SerializeField] float _limitRotationY = 80f;
    
    bool _isDrag;
    private void Start()
    {
        _defaultRotation = transform.rotation;
    }
    
    void Update()
    {
        if (!_isDrag && EventSystem.current.IsPointerOverGameObject()) return;

        // 左クリックした時
        if (Input.GetMouseButtonDown(0))
        {
            _isDrag = true;
            _lastMousePosition = Input.mousePosition;
        }
        // 左ドラッグしている間
        if (Input.GetMouseButton(0) && _isDrag)
        {
            if (((Vector2)Input.mousePosition - _lastMousePosition).magnitude < _threshold) return;
            _newAngle.x -= (Input.mousePosition.y - _lastMousePosition.y) * RotationSpeed.x;
            _newAngle.y -= (_lastMousePosition.x - Input.mousePosition.x) * RotationSpeed.y;
            
            _newAngle.x = Mathf.Clamp(_newAngle.x, -_limitRotationX,   _limitRotationX);
            _newAngle.y = Mathf.Clamp(_newAngle.y,  -_limitRotationY,  _limitRotationY);

            transform.rotation = _defaultRotation * Quaternion.Euler(_newAngle.x , _newAngle.y , 0f);
            _lastMousePosition = Input.mousePosition;
        }
        else
        {
            _isDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("ドラッグ中");
    }
}
