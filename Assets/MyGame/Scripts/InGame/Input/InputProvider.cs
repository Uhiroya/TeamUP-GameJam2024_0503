using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class InputProvider : SingletonMonoBehavior<InputProvider>
{
    [SerializeField] private LaunchPortManager _launchPortManager;
    [SerializeField] private RectTransform _joyStick;
    private Vector3 _defaultJoyStickPosition;
    private Vector3 _currentInput;
    void Start()
    {
        _defaultJoyStickPosition = _joyStick.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        _currentInput = (_joyStick.position - _defaultJoyStickPosition) / _joyStick.rect.width;
        //Debug.Log(_currentInput);
        //Debug.Log(_joyStick.position);
        //Debug.Log(_defaultJoyStickPosition);
        if (_currentInput.magnitude > 0.05f)
        {
            _launchPortManager.MoveLaunchPort(_currentInput);
        }
    }
}
