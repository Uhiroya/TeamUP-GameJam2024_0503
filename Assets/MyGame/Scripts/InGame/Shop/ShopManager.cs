using System;
using System.Collections;
using System.Collections.Generic;
using SoulRunProject.Common;
using UnityEngine;


public enum ShopType
{
    発芽間隔 = 0,
    発芽個数 = 1,
    成菌にかかる時間 = 2 ,
    発芽サイズ = 3,
    
    毎秒の成長量 = 4 ,
    成長可能サイズ = 5,
    
    栄養剤クールダウン = 6,
    栄養を吸収する時間 = 7,
    与える成長量 = 8,
    滴の大きさ = 9,
    //滴の個数 = 10,

}

[Serializable]
public class LevelUpInfo
{
    [SerializeField, Header("レベルアップの上限")] public int LevelUpLimit = 5;
    [SerializeField, Header("レベルアップのベースの費用")] public int BasePrice = 100;
    [SerializeField, Header("費用にかける補正値")] public int PriceCompensation = 1;
    [SerializeField , Header("レベルアップの上昇値")] public float IncreasedValue = 1;
}

public class ShopManager : SingletonMonoBehavior<ShopManager>
{
    [SerializeField , Header("各レベルアップ情報") , EnumDrawer(typeof(ShopType))] 
    private List<LevelUpInfo> _levelUpInfos ;

    [SerializeField, Header("レベルごとの購入価格にかける累乗補正")]
    private float _pricePower = 1.15f; 

    private readonly Dictionary<ShopType, int> _currentShopLevel = new();
    protected override void OnAwake()
    {
        foreach (ShopType shopType in Enum.GetValues(typeof(ShopType)))
        {
            _currentShopLevel.Add(shopType , 0);
        }
    }

    /// <summary>
    /// レベルアップにかかる費用の計算
    /// </summary>
    public int LevelUpPrice(ShopType shopType)
    {
        int nextLevel = _currentShopLevel[shopType] + 1;

        float price = _levelUpInfos[(int)shopType].PriceCompensation * 
                      _levelUpInfos[(int)shopType].BasePrice *
                      Mathf.Pow(_pricePower, nextLevel);
        return (int)price;
    }
    
    /// <summary>
    /// レベルアップ可能か
    /// </summary>
    public bool CheckLevelUpAble(ShopType shopType)
    {
        return ResourceManager.Instance.CurrentCoin.Value >= LevelUpPrice(shopType)
             && (_currentShopLevel[shopType] < _levelUpInfos[(int)shopType].LevelUpLimit);
    }

    /// <summary>
    /// レベルアップ
    /// </summary>
    public int LevelUp(ShopType shopType)
    {
        var value = _levelUpInfos[(int)shopType].IncreasedValue;
        switch (shopType)
        {
            case ShopType.発芽間隔:
                PrizeManager.Instance.CurrentStatus.SpawnCoolTime += value;
                break;
            case ShopType.発芽個数:
                PrizeManager.Instance.CurrentStatus.SpawnCount += (int)value;
                break;
            case ShopType.成菌にかかる時間:
                PrizeManager.Instance.CurrentStatus.SpawnTime += value;
                break;
            case ShopType.発芽サイズ:
                PrizeManager.Instance.CurrentStatus.SpawnEndScale += value;
                break;
            case ShopType.毎秒の成長量:
                PrizeManager.Instance.CurrentStatus.GrowthRate += value;
                break;
            case ShopType.成長可能サイズ:
                PrizeManager.Instance.CurrentStatus.GrowthMaxSize += value;
                break;
            case ShopType.栄養剤クールダウン:
                DropPortManager.Instance.CurrentStatus.SupplementCoolTime += value;
                break;
            case ShopType.滴の大きさ:
                DropPortManager.Instance.CurrentStatus.SupplementSize += value;
                break;
            // case ShopType.滴の個数:
            //     DropPortManager.Instance.CurrentStatus.SupplementCount += value;
            //     break;
            case ShopType.栄養を吸収する時間:
                DropPortManager.Instance.CurrentStatus.ReinForceTime += value;
                break;
            case ShopType.与える成長量:
                DropPortManager.Instance.CurrentStatus.ReinForceAmount += value;
                break;
        }
        _currentShopLevel[shopType]++;
        ResourceManager.Instance.CurrentCoin.Value -= LevelUpPrice(shopType);
        return _currentShopLevel[shopType] + 1;
    }

    public string GetParameterByShopType(ShopType shopType)
    {
        string value = "";
        switch (shopType)
        {
            case ShopType.発芽間隔:
                value = PrizeManager.Instance.CurrentStatus.SpawnCoolTime.ToString("0.0"); 
                break;
            case ShopType.発芽個数:
                value = PrizeManager.Instance.CurrentStatus.SpawnCount.ToString("0"); 
                break;
            case ShopType.成菌にかかる時間:
                value = PrizeManager.Instance.CurrentStatus.SpawnTime.ToString("0.0"); 
                break;
            case ShopType.発芽サイズ:
                value = PrizeManager.Instance.CurrentStatus.SpawnEndScale.ToString("0.0"); 
                break;
            case ShopType.毎秒の成長量:
                value = PrizeManager.Instance.CurrentStatus.GrowthRate.ToString("0.0"); 
                break;
            case ShopType.成長可能サイズ:
                value = PrizeManager.Instance.CurrentStatus.GrowthMaxSize.ToString("0.0"); 
                break;
            case ShopType.栄養剤クールダウン:
                value = DropPortManager.Instance.CurrentStatus.SupplementCoolTime.ToString("0.0"); 
                break;
            case ShopType.滴の大きさ:
                value = DropPortManager.Instance.CurrentStatus.SupplementSize.ToString("0.0"); 
                break;
            // case ShopType.滴の個数:
            //     value = DropPortManager.Instance.CurrentStatus.SupplementCount.ToString("0.0"); 
            //     break;
            case ShopType.栄養を吸収する時間:
                value = DropPortManager.Instance.CurrentStatus.ReinForceTime.ToString("0.0"); 
                break;
            case ShopType.与える成長量:
                value = DropPortManager.Instance.CurrentStatus.ReinForceAmount.ToString("0.0"); 
                break;
        }

        return value;
    }
}
