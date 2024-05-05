using System;
using System.Collections.Generic;
using SoulRunProject.Common;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts.InGame.UI
{
    enum LevelUpType
    {
        きのこ = 0,
        きかい = 1,
        えいよう = 2,
    }

    [Serializable]
    public class LevelUpCanvas
    {
        public Text NameText;
        public CanvasGroup LevelUpCanvasGroup;
    }
    public class UIManager : SingletonMonoBehavior<UIManager>
    {
        [SerializeField , EnumDrawer(typeof(LevelUpType))] private List<LevelUpCanvas> _levelUpCanvasList;
        [Header("発芽クールタイムスライダー")]
        [SerializeField] private CanvasGroup CoolTimeSliderGroup;
        [SerializeField] private Slider CoolTimeSlider;
        [SerializeField] private Text CoolTimerText;
        [Header("発芽時間スライダー")]
        [SerializeField] private CanvasGroup SpawnTimeSliderGroup;
        [SerializeField] private Slider SpawnTimeSlider;
        [SerializeField] private Text SpawnTimerText;
        
        private ResourceManager _resourceManager;
        private PrizeManager _prizeManager;
        private DropPortManager _dropPortManager;

        float _currentSpawnCoolTime;
        float _currentSpawnTime;


        
        
        private void Start()
        {
            _resourceManager = ResourceManager.Instance;
            _prizeManager = PrizeManager.Instance;
            _dropPortManager = DropPortManager.Instance;

            for (int i = 0; i < _levelUpCanvasList.Count; i++)
            {
                _levelUpCanvasList[i].NameText.text = Enum.GetName(typeof(LevelUpType), i);
            }


            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnCoolTime).Subscribe(x =>
            {
                _currentSpawnCoolTime = x;
                //SpawnCoolTimeText.text = x.ToString("0.0");
            });
            //_prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnCount).Subscribe(x => SpawnCountText.text = x.ToString("0"));
            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnTime).Subscribe(x =>
            {
                _currentSpawnTime = x;
                //SpawnTimeText.text = x.ToString("0.0");
            });
            // _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnEndScale).Subscribe(x => SpawnEndScaleText.text = x.ToString("0.0"));
            // _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.GrowthRate).Subscribe(x => GrowthRateText.text = x.ToString("0.00"));
            // _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.GrowthMaxSize).Subscribe(x => GrowthMaxSizeText.text = x.ToString("0.0"));
            //
            _prizeManager.ObserveEveryValueChanged(x => x.SpawnTimer).Subscribe(x =>
            {
                var remainingTime = (_currentSpawnCoolTime - x);
                CoolTimerText.text = remainingTime.ToString("0.0");
                CoolTimeSlider.value = 1 - remainingTime / _currentSpawnCoolTime;
                if (remainingTime < 0.1f)
                {
                    CoolTimeSliderGroup.gameObject.SetActive(false);
                    SpawnTimeSliderGroup.gameObject.SetActive(true);
                }
            });
            
            _prizeManager.ObserveEveryValueChanged(x => x.GrowthTimer).Subscribe(x =>
            {
                var remainingTime = (_currentSpawnTime - x);
                SpawnTimerText.text = remainingTime.ToString("0.0");
                SpawnTimeSlider.value = 1 - remainingTime / _currentSpawnTime;
                if (remainingTime < 0.1f)
                {
                    CoolTimeSliderGroup.gameObject.SetActive(true);
                    SpawnTimeSliderGroup.gameObject.SetActive(false);
                }
            });
        }

        private int _currentCanvasIndex = 0;
        
        public void LevelUpCanvasSwitchLeft()
        {
            if (_currentCanvasIndex == 0)
            {
                _currentCanvasIndex = _levelUpCanvasList.Count - 1;
            }
            else
            {
                _currentCanvasIndex--;
            }

            _levelUpCanvasList.ForEach(x => x.LevelUpCanvasGroup.gameObject.SetActive(false));
            _levelUpCanvasList[_currentCanvasIndex].LevelUpCanvasGroup.gameObject.SetActive(true);

        }
        public void LevelUpCanvasSwitchRight()
        {
            if (_currentCanvasIndex == _levelUpCanvasList.Count - 1)
            {
                _currentCanvasIndex = 0;
            }
            else
            {
                _currentCanvasIndex++;
            }

            _levelUpCanvasList.ForEach(x => x.LevelUpCanvasGroup.gameObject.SetActive(false));
            _levelUpCanvasList[_currentCanvasIndex].LevelUpCanvasGroup.gameObject.SetActive(true);
        }
        
    }
}
