using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PrizeController : MonoBehaviour
{
    public PrizeData PrizeData;
    private bool _isSpawn = false;
    
    private void Start()
    {
        //
        //_isSpawn = true;
    }
    
    public async void Spawn(float spawnScale , float spawnTime , float forwardPower)
    {
        var rigidbody = GetComponentInChildren<Rigidbody>();
        _isSpawn = true;
        //_rigidbody.isKinematic = true;
        transform.localScale = Vector3.one * spawnScale;
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one, spawnTime));
        sequence.Join(transform.DOBlendableMoveBy(Vector3.back * forwardPower, spawnTime));
        
        await sequence;

       // _rigidbody.isKinematic = false;
        _isSpawn = true;
    }
}
