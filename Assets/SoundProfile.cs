using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundProfile : MonoBehaviour
{
    private static readonly string firstPlay = "firstPlay";
    private static readonly string bgmPref = "bgmPref";
    private static readonly string sfxPref = "sfxPref";
    private int firstPlayIndex;

    public float bgmFloat, sfxFloat;
    
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        Debug.Log("first play: " + firstPlayIndex);

        firstPlayIndex = PlayerPrefs.GetInt(firstPlay);

        if (firstPlayIndex == 0)
        {
            bgmFloat = 0.5f;
            sfxFloat = 0.5f;
            bgmSlider.value = bgmFloat;
            sfxSlider.value = sfxFloat;
            PlayerPrefs.SetFloat(bgmPref, bgmFloat);
            PlayerPrefs.SetFloat(sfxPref, sfxFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
        }
        else
        {
            bgmFloat = PlayerPrefs.GetFloat(bgmPref);
            sfxFloat = PlayerPrefs.GetFloat(sfxPref);
            bgmSlider.value = bgmFloat;
            sfxSlider.value = sfxFloat;
        }
    }

    public void SaveSoundSetting()
    {
        PlayerPrefs.SetFloat(bgmPref, bgmSlider.value);
        PlayerPrefs.SetFloat(sfxPref, sfxSlider.value);
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSetting();
        }
    }

    public void VolumeControl_BGM(float value)
    {
        bgmFloat = value;
    }

    public void VolumeControl_SFX(float value)
    {
        sfxFloat = value;
    }
}
