using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PrizeController : MonoBehaviour
{
    [SerializeField] private GameObject _purplekinoko;
    [SerializeField] private GameObject _redkinoko;
    [SerializeField] private GameObject _whitekinoko;
    [SerializeField] private Transform _hitEffectTransform;
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private ParticleSystem _growEffect;
    public int GetCoin = 300;
    
    private Transform _parent;
    private PrizeManager _prizeManager;
    private bool _isSpawn = false;
    private bool _isGrowth = false;

    private float _growthTime;

    private int _defaultGetCoin;
    // Instantiateで呼ぶとUpdate回る前にStart呼ばれない
    private void Start()
    {
        _growEffect.Pause();
        _parent = transform.parent;
        _prizeManager = PrizeManager.Instance;
        _defaultGetCoin = GetCoin;
    }

    private void Update()
    {
        Growing(Time.deltaTime);
    }

    private void OnDisable()
    {
        GetCoin = _defaultGetCoin;
        _growEffect.Pause();
    }

    /// <summary>
    /// 自動成長
    /// </summary>
    public void Growing(float deltaTime)
    {
        if (!_isSpawn)
        {
            var newSize = _parent.localScale.x + deltaTime * _prizeManager.CurrentStatus.GrowthRate;
            if (newSize < _prizeManager.CurrentStatus.GrowthMaxSize)
            {
                _parent.localScale = Vector3.one * newSize;
            }
        }
    }



    public void SetKinokoHead(int kinokoType)
    {
        float coinRate = 1;
        switch (kinokoType)
        {
            case 0:
                _whitekinoko.SetActive(true);
                coinRate = 1;
                break;
            case 1:
                _purplekinoko.SetActive(true);
                coinRate = 1.5f;
                break;
            case 2:
                _redkinoko.SetActive(true);
                coinRate = 2f;
                break;
        }
        GetCoin = (int)(GetCoin * coinRate);
    }
    
    public async UniTask Spawn(int kinokoType)
    {
        SetKinokoHead(kinokoType);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        _parent = transform.parent;
        var rigidbody = GetComponentInChildren<Rigidbody>();
        _isSpawn = true;
        rigidbody.isKinematic = true;
        _parent.localScale = Vector3.one * PrizeManager.Instance.SpawnStartScale;
        var sequence = DOTween.Sequence();
        sequence.Append(_parent.DOScale(Vector3.one *  PrizeManager.Instance.CurrentStatus.SpawnEndScale, PrizeManager.Instance.CurrentStatus.SpawnTime));
        sequence.Join(_parent.DOBlendableMoveBy(Vector3.back * PrizeManager.Instance.SpawnFrontPower, PrizeManager.Instance.CurrentStatus.SpawnTime));
        sequence.Join(_parent.DOBlendableMoveBy(Vector3.up * PrizeManager.Instance.SpawnUpPower, PrizeManager.Instance.CurrentStatus.SpawnTime));

        await sequence;

        rigidbody.isKinematic = false;
        _isSpawn = false;
    }

    private async void OnTriggerEnter(Collider other)
    {

        if (_isSpawn) return;
        if(other.gameObject.TryGetComponent<DropController>(out var drop))
        {
            var hitEffect = Instantiate(_hitEffectPrefab, _hitEffectTransform);
            Destroy(hitEffect,3);
            _isGrowth = true;
            if (_parent.localScale.x + DropPortManager.Instance.CurrentStatus.ReinForceAmount < _prizeManager.CurrentStatus.GrowthMaxSize)
            {
                _growEffect.gameObject.SetActive(true);
                _growEffect.Play();
                await _parent.DOScale(_parent.localScale + Vector3.one * DropPortManager.Instance.CurrentStatus.ReinForceAmount, DropPortManager.Instance.CurrentStatus.ReinForceTime);
                _growEffect.Pause();
                _growEffect.gameObject.SetActive(false);
            }
            else
            {
                await _parent.DOScale( Vector3.one * _prizeManager.CurrentStatus.GrowthMaxSize, DropPortManager.Instance.CurrentStatus.ReinForceTime);
            }
            _isGrowth = false;
        }
    }
}
