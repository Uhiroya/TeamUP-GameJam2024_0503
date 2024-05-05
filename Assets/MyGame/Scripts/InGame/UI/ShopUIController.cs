using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ShopType _shopType;
    [SerializeField] private Text _shopNameText;
    [SerializeField] public Text ParameterText;
    [SerializeField] public Text PriceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Text _currentLevelText;

    private ShopManager _shopManager;
    void Start()
    {
        _shopNameText.text = Enum.GetName(typeof(ShopType), _shopType);
        _shopManager = ShopManager.Instance;
        _currentLevelText.text = "レベル 1";
        ResourceManager.Instance.CurrentCoin.Subscribe(OnCoinChanged);
       
    }

    public void OnCoinChanged(int currentCoin)
    {
        ParameterText.text = _shopManager.GetParameterByShopType(_shopType);
        PriceText.text = _shopManager.LevelUpPrice(_shopType).ToString("0");
        _buyButton.interactable = _shopManager.CheckLevelUpAble(_shopType);
    }

    public void OnClick()
    {
        if (_shopManager.CheckLevelUpAble(_shopType))
        {
            _currentLevelText.text = $"レベル {_shopManager.LevelUp(_shopType)}";
        }
    }
    
}
