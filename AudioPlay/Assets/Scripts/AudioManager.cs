using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] AudioClipArray;                      //音频数组

    private static Dictionary<string, AudioClip> _DicAudio; //音频库(字典)
    private static AudioSource audioBGM;                    //音频源
    private static AudioSource[] audioSources;

    //[Header("VOL")]
    //[Range(0, 1)]
    //public float volumeOfBGM;
    //[Range(0, 1)]
    //public float volumeOfEffect;

    //[Header("VOL Slider")]
    public Slider volumeSlider;
    public float Volume { get; set; }

    void Awake()
    {
        //初始化音频库：创建空间，把音频数组里所有元素放入音频库
        _DicAudio = new Dictionary<string, AudioClip>();
        foreach (var item in AudioClipArray)
        {
            _DicAudio.Add(item.name, item);
        }

        //指定背景音乐的音频源
        audioBGM = GetComponent<AudioSource>();
        if (audioBGM == null)
            audioBGM = gameObject.AddComponent<AudioSource>();

        audioSources = GetComponents<AudioSource>();

        Volume = volumeSlider.value;
    }


    //播放特效音乐函数：
    public void PlayEffect(string acName)
    {
        //当传进来的名字不为空且在音频库中
        if (_DicAudio.ContainsKey(acName) && !string.IsNullOrEmpty(acName))
        {
            AudioClip ac = _DicAudio[acName];
            PlayEffect(ac);
        }
    }

    private void PlayEffect(AudioClip ac)
    {
        if (ac)
        {
            //遍历当前持有的AudioSource组件
            audioSources = gameObject.GetComponents<AudioSource>();

            //audioSources[0]被BGM的播放占用，因此从[1]开始
            for (int i = 1; i < audioSources.Length; i++)
            {
                //当有音频源空闲时，则用其播放
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].loop = false;
                    audioSources[i].clip = ac;
                    audioSources[i].volume = Volume;
                    audioSources[i].Play();
                    return;
                }
            }

            //当没有多余的音频源空闲时，则创建新的音频源
            AudioSource newAs = gameObject.AddComponent<AudioSource>();
            newAs.loop = false;
            newAs.clip = ac;
            newAs.volume = Volume;
            newAs.Play();
        }
    }

    //播放BGM函数：
    public void BGMPlay(string acName)
    {
        //当传进来的名字不为空且在音频库中
        if (_DicAudio.ContainsKey(acName) && !string.IsNullOrEmpty(acName))
        {
            AudioClip ac = _DicAudio[acName];
            BGMPlay(ac);
        }
    }

    private void BGMPlay(AudioClip ac)
    {
        if (ac)
        {
            audioBGM.clip = ac;
            audioBGM.loop = true;
            audioBGM.volume = Volume;
            audioBGM.Play();
        }
    }

    //停止当前BGM的播放函数：
    public void StopBGMPlay()
    {
        audioBGM.Stop();
    }

    //设置音量函数：
    public void SetVolume()
    {
        Volume = volumeSlider.value;
        for (int i = 0; i < audioSources.Length; i++)
            audioSources[i].volume = Volume;
    }
}