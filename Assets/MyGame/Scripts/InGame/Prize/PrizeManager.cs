using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrizeManager : SingletonMonoBehavior<PrizeManager>
{
    [SerializeField , Header("スポーンする景品")] private GameObject _prize;
    [SerializeField , Header("スポーン位置")] private Transform[] _spawnPositions;
    [SerializeField, Header("スポーンしてから成長し終わるまでにかかる時間")] public float SpawnTime = 5f;
    [SerializeField, Header("初期スポーンサイズ")] public float SpawnScale = 0.05f;
    [SerializeField, Header("スポーンしながら前進させる力")] public float SpawnFrontPower = 1f;
    [SerializeField, Header("スポーンしながら上に動かす力")] public float SpawnUpPower = 1f;
    [SerializeField, Header("成長完了にかかる時間")] public float GrowthTime = 10f;
    

    private void Start()
    {
        //_spawnPositions　= _spawnTransform.GetComponentsInChildren<Transform>();
    }
    
    public void Spawn()
    {
        foreach (var trams in _spawnPositions)
        {
           var obj = Instantiate(_prize, trams.position, Quaternion.identity, null);
           obj.GetComponentInChildren<PrizeController>().Spawn();
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        
    }
}
