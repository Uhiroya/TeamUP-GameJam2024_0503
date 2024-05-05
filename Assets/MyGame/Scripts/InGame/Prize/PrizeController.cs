using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PrizeController : MonoBehaviour
{
    public PrizeData PrizeData;
    
    private Transform _parent;
    private PrizeManager _prizeManager;
    private bool _isSpawn = false;
    private bool _isGrowth = false;

    private float _growthTime;

    // Instantiateで呼ぶとUpdate回る前にStart呼ばれない
    private void Start()
    {
        _parent = transform.parent;
        _prizeManager = PrizeManager.Instance;
    }

    private void Update()
    {
        if (_isSpawn)
        {
            var newSize = _parent.localScale.x + Time.deltaTime * _prizeManager.CurrentStatus.GrowthRate;
            if (newSize < _prizeManager.CurrentStatus.GrowthMaxSize)
            {
                _parent.localScale = Vector3.one * newSize;
            }
        }
    }
    
    public async UniTask Spawn()
    {
        _parent = transform.parent;
        _prizeManager = PrizeManager.Instance;
        var rigidbody = GetComponentInChildren<Rigidbody>();
        _isSpawn = true;
        rigidbody.isKinematic = true;
        _parent.localScale = Vector3.one * _prizeManager.SpawnStartScale;

        var sequence = DOTween.Sequence();
        sequence.Append(_parent.DOScale(Vector3.one *  _prizeManager.CurrentStatus.SpawnEndScale, _prizeManager.CurrentStatus.SpawnTime));
        sequence.Join(_parent.DOBlendableMoveBy(Vector3.back * _prizeManager.SpawnFrontPower, _prizeManager.CurrentStatus.SpawnTime));
        sequence.Join(_parent.DOBlendableMoveBy(Vector3.up * _prizeManager.SpawnUpPower, _prizeManager.CurrentStatus.SpawnTime));

        await sequence;

        rigidbody.isKinematic = false;
        _isSpawn = false;
    }

    private async void OnTriggerEnter(Collider other)
    {

        if (_isSpawn) return;
        if(other.gameObject.TryGetComponent<DropController>(out var drop))
        {
            _isGrowth = true;
            if (_parent.localScale.x + DropPortManager.Instance.CurrentStatus.ReinForceAmount < _prizeManager.CurrentStatus.GrowthMaxSize)
            {
                await _parent.DOScale(_parent.localScale + Vector3.one * DropPortManager.Instance.CurrentStatus.ReinForceAmount, DropPortManager.Instance.CurrentStatus.ReinForceTime);
            }
            else
            {
                await _parent.DOScale( Vector3.one * _prizeManager.CurrentStatus.GrowthMaxSize, DropPortManager.Instance.CurrentStatus.ReinForceTime);
            }
            _isGrowth = false;
        }
    }
}
