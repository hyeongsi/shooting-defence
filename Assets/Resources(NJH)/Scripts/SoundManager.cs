using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgm;
    [SerializeField] AudioClip[] bgmList;

    public float bgmVolume;
    public float sfxVolume;

    [SerializeField] SoundProfile soundProfile;

    public enum Sounds
    {
        PlayerMove,

    }
    
    private void Awake()
    {
        if (instance == null)
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
        Debug.Log("BGM Started");

        soundProfile = FindObjectOfType<SoundProfile>();

        string sceneName;
        sceneName = scene.name;

        if (sceneName != "Loading")
        {
            int bgmIndex = 0;

            if (sceneName == "MainMenu")
            {
                bgmIndex = 0;
            }
            else if (sceneName == "InGameScene")
            {
                bgmIndex = Random.Range(1, bgmList.Length - 1);
            }

            PlayBGM(bgmList[bgmIndex]);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(soundProfile != null)
        {
            bgm.volume = soundProfile.bgmFloat;
            sfxVolume = soundProfile.sfxFloat;
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        Debug.Log(audioClip);

        bgm.clip = audioClip;
        bgm.loop = true;
        bgm.Play();
    }

    public void PlaySound(string soundName, AudioClip audioClip, float volume, Vector3 position)
    {
        // 캐릭터와의 거리가 적용되는 소리(거리에따라 소리 크기 다름)

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

    public void PlaySound(string soundName, AudioClip audioClip)
    {
        Debug.Log("사운드 재생" + audioClip);

        GameObject soundObject = new GameObject(soundName + " Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = sfxVolume;
        audioSource.Play();

        Destroy(soundObject, audioClip.length);
    }
}
