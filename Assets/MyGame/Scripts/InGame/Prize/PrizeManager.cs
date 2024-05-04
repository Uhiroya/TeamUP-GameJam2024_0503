using System;
using UnityEngine;
/// <summary>
/// ここでは 発芽　→ Spawn
/// 成長 → Growth
/// 栄養剤での成長 → ReinForce
/// とする
/// </summary>
[Serializable]
public class PrizeManagerStatus
{
    [SerializeField, Header("スポーン個数")] public int SpawnCount;
    [SerializeField, Header("スポーンしてから成長し終わるまでにかかる時間")] public float SpawnTime = 5f;
    [SerializeField, Header("初期スポーン終了サイズ")] public float SpawnEndScale = 0.5f;
    //TODO ここで必要とされているものは違う
    [SerializeField, Header("栄養剤で成長完了にかかる時間")] public float ReinForceTime = 10f;
    [SerializeField, Header("育成可能サイズ")] public float GrowthMaxSize = 1.2f;

    public PrizeManagerStatus(PrizeManagerStatus status)
    {
        SpawnCount = status.SpawnCount;
        SpawnTime = status.SpawnTime;
        SpawnEndScale = status.SpawnEndScale;
        ReinForceTime = status.ReinForceTime;
        GrowthMaxSize = status.GrowthMaxSize;
    }
}
public class PrizeManager : SingletonMonoBehavior<PrizeManager>
{
    [Header("進行によって変更されないデータ")]
    [SerializeField , Header("スポーンする景品")] private GameObject _prize;
    [SerializeField , Header("スポーン範囲 左 , X-方向")] private Transform _spawnPositionLeft;
    [SerializeField , Header("スポーン範囲 右　X+方向")] private Transform _spawnPositionRight;
    [SerializeField , Header("スポーン最大個数")] private int _spawnCountMax;
    [SerializeField, Header("スポーンしながら前進させる力")] public float SpawnFrontPower = 1f;
    [SerializeField, Header("スポーンしながら上に動かす力")] public float SpawnUpPower = 1f;
    [SerializeField, Header("初期スポーン開始サイズ")] public float SpawnStartScale = 0.05f;
    
    [SerializeField, Header("育成上限サイズ")] public float PrizeLimitSize = 1.6f;
    
    [Header("進行によって変更されるデータ")]
    [SerializeField, Header("デフォルトのステータス")] private PrizeManagerStatus _defaultStatus;

    public PrizeManagerStatus CurrentStatus { get; set; }

    protected override void OnAwake()
    {
        CurrentStatus = new PrizeManagerStatus(_defaultStatus);
    }
    
    public void Spawn()
    {
        var leftSidePos = _spawnPositionLeft.position;
        var rightSidePos = _spawnPositionRight.position;
        
        float spawnXLength = rightSidePos.x - leftSidePos.x;

        for (int i = 0; i < CurrentStatus.SpawnCount; i++)
        {
            var interval = spawnXLength / (CurrentStatus.SpawnCount + 1) * (i+1);
            var spawnPosX = _spawnPositionLeft.position.x + interval;

            var obj = Instantiate(_prize, new Vector3(spawnPosX, leftSidePos.y, leftSidePos.z), Quaternion.identity, null);
            obj.GetComponentInChildren<PrizeController>().Spawn();
            
        }
        
    }
    


    // Update is called once per frame
    void Update()
    {
        
    }
}
