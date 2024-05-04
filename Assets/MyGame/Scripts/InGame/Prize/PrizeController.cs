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

    // Instantiateで呼ぶとUpdate回る前にStart呼ばれないってまじ？
    private void Start()
    {
        _parent = transform.parent;
        _prizeManager = PrizeManager.Instance;
    }

    public async void Spawn()
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
        //スポーン中または成長中は処理を行わない。
        if (_isSpawn || _isGrowth) return;
        if (other.CompareTag("Drop"))
        {
            _isGrowth = true;
            var growthScale =  other.gameObject.GetComponent<DropController>().GrowthAmount;
            if (_parent.localScale.x + growthScale < _prizeManager.CurrentStatus.GrowthMaxSize)
            {
                await _parent.DOScale(_parent.localScale + Vector3.one * growthScale, _prizeManager.CurrentStatus.ReinForceTime);
            }
            else
            {
                await _parent.DOScale( Vector3.one * _prizeManager.CurrentStatus.GrowthMaxSize, _prizeManager.CurrentStatus.ReinForceTime);
                print("最大サイズになりました");
            }
            _isGrowth = false;
        }
    }
}
