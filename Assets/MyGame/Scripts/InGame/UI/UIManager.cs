using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts.InGame.UI
{
    public class UIManager : SingletonMonoBehavior<UIManager>
    {
        [SerializeField] private Text SpawnCountText;
        [SerializeField] private Text SpawnTimeText;
        [SerializeField] private Text SpawnEndScaleText;
        [SerializeField] private Text GrowthTimeText;
        [SerializeField] private Text GrowthMaxSize;
    }
}
