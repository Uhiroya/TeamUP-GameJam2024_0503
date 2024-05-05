using SoulRunProject.Common;
using UnityEngine;

public enum SoundEffectType
{
    コイン使用1 = 0,
    コイン使用2 = 1,
    コインゲット1 = 2,
    コインゲット2 = 3,
    ボタン1 = 4,
    ボタン2 = 5,
    ボタン3 = 6,
    機械始動 = 7,
    機械停止 = 8,
    機械作動中 = 9,
    水落下1 = 10,
    水落下2 = 12,
    水落下3 = 13,
    水落下4 = 14,
    水落下5 = 15,
    水落下6 = 16,
}

public enum BGMType
{
    InGame = 1,
    InGame2 = 2,
}
public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    public AudioSource AudioSeSource;
    public AudioSource AudioBGMSource;
    [SerializeField , EnumDrawer(typeof(SoundEffectType))] private AudioClip[] _audioSeClips;
    [SerializeField, EnumDrawer(typeof(BGMType))] private AudioClip[] _audioBGMClips;

    public void PlaySe(SoundEffectType soundEffectIndex)
    {
        AudioSeSource.PlayOneShot(_audioSeClips[(int)soundEffectIndex]);
    }

    public void PlaySe(int soundIndex)
    {
        AudioSeSource.PlayOneShot(_audioSeClips[soundIndex]);
    }

    public void PlayBGM(BGMType soundIndex)
    {
        AudioBGMSource.clip = _audioBGMClips[(int)soundIndex];
        AudioBGMSource.Play();
    }
}
