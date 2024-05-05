using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ButtonAnimation : SingletonMonoBehavior<ButtonAnimation>
{
    [SerializeField] private Transform _buttonObject;
    [SerializeField] private float _duration;
    [SerializeField] private float defaultY;
    [SerializeField] private float pushEndY;
    private bool _isPushed;
    public bool IsPushed => _isPushed;
    
    public async void OnButtonClick()
    {
        if (_isPushed) return;
        _isPushed = true;
        _buttonObject.DOLocalMoveY(pushEndY, _duration);
    }
    public async void OnButtonReset()
    {
        await _buttonObject.DOLocalMoveY(defaultY, _duration);
        _isPushed = false;
    }
    
}
