using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;
[Serializable]
public class PrizeManagerStatus
{
    [SerializeField, Header("発芽クールタイム")] public float SpawnCoolTime;
    [SerializeField, Header("発芽個数")] public int SpawnCount;
    [SerializeField, Header("成菌にかかる時間　(発芽クールタイム以下にはしちゃだめ)")] public float SpawnTime = 5f;
    [SerializeField, Header("発芽サイズ")] public float SpawnEndScale = 0.5f;
    [SerializeField, Header("毎秒の成長量")] public float GrowthRate = 0.01f;
    [SerializeField, Header("成長可能サイズ")] public float GrowthMaxSize = 1.2f;

    public PrizeManagerStatus(PrizeManagerStatus status)
    {
        SpawnCoolTime = status.SpawnCoolTime;
        SpawnCount = status.SpawnCount;
        SpawnTime = status.SpawnTime;
        SpawnEndScale = status.SpawnEndScale;
        GrowthRate = status.GrowthRate;
        GrowthMaxSize = status.GrowthMaxSize;
    }
    
}
public class PrizeManager : SingletonMonoBehavior<PrizeManager>
{
    [Header("進行によって変更されないデータ")] 
    [SerializeField, Header("景品の初期位置")] private Transform _defaultPrizes;
    [SerializeField , Header("スポーンする景品")] private GameObject _prize;
    [SerializeField , Header("スポーン範囲 左 , X-方向")] private Transform _spawnPositionLeft;
    [SerializeField , Header("スポーン範囲 右　X+方向")] private Transform _spawnPositionRight;
    [SerializeField, Header("スポーンしながら前進させる力")] public float SpawnFrontPower = 1f;
    [SerializeField, Header("スポーンしながら上に動かす力")] public float SpawnUpPower = 1f;
    [SerializeField, Header("初期スポーン開始サイズ")] public float SpawnStartScale = 0.05f;
    
    [SerializeField , Header("スポーン最大個数")] private int _spawnCountMax;
    [SerializeField, Header("育成上限サイズ")] public float PrizeLimitSize = 1.6f;
    [SerializeField, Header("デフォルトのステータス")] private PrizeManagerStatus _defaultStatus;
    
    [Header("進行によって変更するデータ")]
    [Header("現在のステータス")]
    public PrizeManagerStatus CurrentStatus;

    private bool _isSpawn;
    private float _spawnTimer;
    private float _growthTimer;
    public float SpawnTimer => _spawnTimer;
    public float GrowthTimer => _growthTimer;
    protected override void OnAwake()
    {
        CurrentStatus = new PrizeManagerStatus(_defaultStatus);
    }

    private ObjectPool<GameObject> _prizePool;
    void Start()
    {
        _prizePool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(_prize),
            actionOnGet: target => target.SetActive(true),
            actionOnRelease: target => target.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 60,
            maxSize: 100
        );

        foreach (Transform defaultPrize in _defaultPrizes)
        {
            var obj = _prizePool.Get();
            obj.transform.position = defaultPrize.position;
            obj.transform.localScale = defaultPrize.localScale;
            //シーン上の景品はルートの回転を触ってしまっているため、rotationを子オブジェクトに移動する。
            foreach (Transform child in obj.transform)
            {
                child.transform.rotation = defaultPrize.rotation;
            }
            defaultPrize.transform.rotation = Quaternion.identity;
            obj.GetComponentInChildren<PrizeController>().SetKinokoHead(Random.Range(0 , 3));
        }
    }
    private void Update()
    {
        if (!_isSpawn)
        {
            _growthTimer = 0f;
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer > CurrentStatus.SpawnCoolTime)
            {
                Spawn();
                _spawnTimer= 0f;
            }
        }
        else
        {
            _growthTimer += Time.deltaTime;
        }
    }
    
    public void ReleasePrize(GameObject obj)
    {
        _prizePool.Release(obj);
    }
    
    public async void Spawn()
    {
        if (_isSpawn) return;
        var leftSidePos = _spawnPositionLeft.position;
        var rightSidePos = _spawnPositionRight.position;
        float spawnXLength = rightSidePos.x - leftSidePos.x;
        _isSpawn = true;
        var tasks = new List<UniTask>(); 
        for (int i = 0; i < CurrentStatus.SpawnCount; i++)
        {
            var interval = spawnXLength / (CurrentStatus.SpawnCount + 1) * (i+1);
            var spawnPosX = _spawnPositionLeft.position.x + interval;
            var obj = _prizePool.Get();
            obj.transform.position = new Vector3(spawnPosX, leftSidePos.y, leftSidePos.z);
            tasks.Add(obj.GetComponentInChildren<PrizeController>().Spawn(Random.Range(0 , 3)));
        }
        await tasks;
        _isSpawn = false;
    }
    
    
}
