using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioClip[] bgmList;

    public static SoundManager instance;

    public enum Sounds
    {
        PlayerMove,

    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnsceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for(int i = 0; i < bgmList.Length; i++)
        {
            if(scene.name == bgmList[i].name)
            {
                PlayBGM(bgmList[i]);
            }
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        bgm.clip = audioClip;
        bgm.loop = true;
        bgm.Play();
    }

    public void PlaySound(string soundName, AudioClip audioClip, float volume, Vector3 position)
    {
        Debug.Log("사운드 재생" + audioClip);

        GameObject soundObject = new GameObject(soundName + " Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        soundObject.transform.position = position;  // 소리가 날 위치

        audioSource.maxDistance = 15f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;

        audioSource.Play();

        Destroy(soundObject, audioClip.length);
    }

    public void PlaySound(string soundName, AudioClip audioClip, float volume)
    {
        Debug.Log("사운드 재생" + audioClip);

        GameObject soundObject = new GameObject(soundName + " Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(soundObject, audioClip.length);
    }
}
