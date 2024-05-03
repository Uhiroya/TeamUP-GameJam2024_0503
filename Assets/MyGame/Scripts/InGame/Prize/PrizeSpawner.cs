using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrizeSpawner : SingletonMonoBehavior<PrizeSpawner>
{
    [SerializeField , Header("スポーンする景品")] private GameObject _prize;
    [SerializeField , Header("スポーン位置")] private Transform[] _spawnPositions;
    [SerializeField, Header("スポーンしてから成長し終わるまでにかかる時間")] private float _spawnTime = 5f;
    [SerializeField, Header("初期スポーンサイズ")] private float _spawnScale = 0.05f;
    [SerializeField, Header("スポーンしながら前進させる力")] private float _spawnForwardPower = 1f;
    

    private void Start()
    {
        //_spawnPositions　= _spawnTransform.GetComponentsInChildren<Transform>();
    }
    
    public void Spawn()
    {
        foreach (var trams in _spawnPositions)
        {
           var obj = Instantiate(_prize, trams.position, Quaternion.identity, null);
           obj.GetComponent<PrizeController>().Spawn(_spawnScale , _spawnTime , _spawnForwardPower);
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        
    }
}
