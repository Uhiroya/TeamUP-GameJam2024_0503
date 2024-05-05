using SoulRunProject.Common;
using UnityEngine;

public enum SoundEffectType
{
    Fire = 0,
    reload = 1,
    explotion = 2,
    reflectBullet = 3,
    StartArea = 4,
    SceneChange = 5,
    Start = 6,
    Sucseece = 7,
    Fail = 8,
    Fall = 9,
    Landing = 10
}

public enum BGMType
{
    Title = 0,
    InGame = 1,
    InGame2 = 2
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
