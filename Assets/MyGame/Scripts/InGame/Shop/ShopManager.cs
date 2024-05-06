using System;
using System.Collections;
using System.Collections.Generic;
using SoulRunProject.Common;
using UnityEngine;

public enum ShopType
{
    はえやすさ = 0,
    はえるりょう = 1,
    はえるはやさ = 2 ,
    しょきサイズ = 3,
    
    せいちょう = 4 ,
    さいだいサイズ = 5,
    
    そくど = 6,
    ききやすさ = 7,
    つよさ = 8,
    おおきさ = 9,

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
            case ShopType.はえやすさ:
                PrizeManager.Instance.CurrentStatus.SpawnCoolTime += value;
                break;
            case ShopType.はえるりょう:
                PrizeManager.Instance.CurrentStatus.SpawnCount += (int)value;
                break;
            case ShopType.はえるはやさ:
                PrizeManager.Instance.CurrentStatus.SpawnTime += value;
                break;
            case ShopType.しょきサイズ:
                PrizeManager.Instance.CurrentStatus.SpawnEndScale += value;
                break;
            case ShopType.せいちょう:
                PrizeManager.Instance.CurrentStatus.GrowthRate += value;
                break;
            case ShopType.さいだいサイズ:
                PrizeManager.Instance.CurrentStatus.GrowthMaxSize += value;
                break;
            case ShopType.そくど:
                DropPortManager.Instance.CurrentStatus.SupplementCoolTime += value;
                break;
            case ShopType.おおきさ:
                DropPortManager.Instance.CurrentStatus.SupplementSize += value;
                break;
            // case ShopType.滴の個数:
            //     DropPortManager.Instance.CurrentStatus.SupplementCount += value;
            //     break;
            case ShopType.ききやすさ:
                DropPortManager.Instance.CurrentStatus.ReinForceTime += value;
                break;
            case ShopType.つよさ:
                DropPortManager.Instance.CurrentStatus.ReinForceAmount += value;
                break;
        }
        var cost = LevelUpPrice(shopType);
        //クレジット更新とUI更新を同タイミングで行っているため
        //この処理を交換したらレベルアップのUIコスト表記がが一つ前のレベルアップコストになってしまう。
        _currentShopLevel[shopType]++;
        ResourceManager.Instance.CurrentCoin.Value -= cost;
        
        return _currentShopLevel[shopType] + 1;
    }

    public string GetParameterByShopType(ShopType shopType)
    {
        string value = "";
        switch (shopType)
        {
            case ShopType.はえやすさ:
                value = PrizeManager.Instance.CurrentStatus.SpawnCoolTime.ToString("0.0"); 
                break;
            case ShopType.はえるりょう:
                value = PrizeManager.Instance.CurrentStatus.SpawnCount.ToString("0"); 
                break;
            case ShopType.はえるはやさ:
                value = PrizeManager.Instance.CurrentStatus.SpawnTime.ToString("0.0"); 
                break;
            case ShopType.しょきサイズ:
                value = PrizeManager.Instance.CurrentStatus.SpawnEndScale.ToString("0.0"); 
                break;
            case ShopType.せいちょう:
                value = PrizeManager.Instance.CurrentStatus.GrowthRate.ToString("0.0"); 
                break;
            case ShopType.さいだいサイズ:
                value = PrizeManager.Instance.CurrentStatus.GrowthMaxSize.ToString("0.0"); 
                break;
            case ShopType.そくど:
                value = DropPortManager.Instance.CurrentStatus.SupplementCoolTime.ToString("0.0"); 
                break;
            case ShopType.おおきさ:
                value = DropPortManager.Instance.CurrentStatus.SupplementSize.ToString("0.0"); 
                break;
            // case ShopType.滴の個数:
            //     value = DropPortManager.Instance.CurrentStatus.SupplementCount.ToString("0.0"); 
            //     break;
            case ShopType.ききやすさ:
                value = DropPortManager.Instance.CurrentStatus.ReinForceTime.ToString("0.0"); 
                break;
            case ShopType.つよさ:
                value = DropPortManager.Instance.CurrentStatus.ReinForceAmount.ToString("0.0"); 
                break;
        }

        return value;
    }
}
