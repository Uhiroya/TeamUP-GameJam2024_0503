using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class InputProvider : SingletonMonoBehavior<InputProvider>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private DropPortManager _dropPortManager;
    [SerializeField] private RectTransform _joyStick;
    private Vector3 _defaultJoyStickPosition;
    private Vector3 _currentInput;

    private bool _isJoyStickMove;
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
            if (!_isJoyStickMove)
            {
                _audioSource.Play();
            }
            
            _isJoyStickMove = true;
            _dropPortManager.MoveLaunchPort(_currentInput);
        }
        else
        {
            if (_isJoyStickMove)
            {
                _audioSource.Stop();
                AudioManager.Instance.PlaySe(SoundEffectType.機械停止);
                _isJoyStickMove = false;
            }
        }
    }
}
