using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    [SerializeField, Header("現在のコインを表示するテキスト")] private Text _coinText;
    [SerializeField , Header("商品のゲットを判定するコライダー")] private Collider _getArea;
    [SerializeField, Header("初期コイン")] private int _defaultCoin;

    private ReactiveProperty<int> _currentCoin = new();
    // Start is called before the first frame update
    void Start()
    {
        _currentCoin.Value = _defaultCoin;
        _currentCoin.Subscribe(x => _coinText.text = x.ToString());
        _getArea.OnTriggerEnterAsObservable().Subscribe(OnTouchGetArea);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTouchGetArea(Collider col)
    {
        if (col.transform.TryGetComponent<PrizeController>(out var prize))
        {
            _currentCoin.Value += prize.PrizeData.Coin;
        }
        Destroy(col.gameObject);
    }
}
