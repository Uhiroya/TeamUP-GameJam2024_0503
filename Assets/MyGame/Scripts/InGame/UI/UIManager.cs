using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts.InGame.UI
{
    public class UIManager : SingletonMonoBehavior<UIManager>
    {
        [SerializeField] private Text SpawnCoolTimeText;
        [SerializeField] private Text SpawnCountText;
        [SerializeField] private Text SpawnTimeText;
        [SerializeField] private Text SpawnEndScaleText;
        [SerializeField] private Text GrowthRateText;
        [SerializeField] private Text GrowthMaxSizeText;
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

            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnCoolTime).Subscribe(x =>
            {
                _currentSpawnCoolTime = x;
                SpawnCoolTimeText.text = x.ToString("0.0");
            });
            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnCount).Subscribe(x => SpawnCountText.text = x.ToString("0"));
            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnTime).Subscribe(x =>
            {
                _currentSpawnTime = x;
                SpawnTimeText.text = x.ToString("0.0");
            });
            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.SpawnEndScale).Subscribe(x => SpawnEndScaleText.text = x.ToString("0.0"));
            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.GrowthRate).Subscribe(x => GrowthRateText.text = x.ToString("0.00"));
            _prizeManager.CurrentStatus.ObserveEveryValueChanged(x => x.GrowthMaxSize).Subscribe(x => GrowthMaxSizeText.text = x.ToString("0.0"));
            
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
    }
}
