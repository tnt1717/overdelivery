using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AudioManager.Instance.PlaySound("tap2");
/// </summary>
public class AudioManager : MonoBehaviour
{
    // 單例模式
    public static AudioManager Instance;

    // 字典來儲存音效
    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public AudioSource audioSource; // 用於播放音效的 AudioSource

    // 設定音效列表
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sounds;

    // Slider 用於控制音量
    private Slider volumeSlider;


    private Coroutine motoCoroutine;

    // PlayerManager 引用
    private PlayerManager playerManager;
    //public PlayerData playerData;
    private void Update()
    {
        UpdateMotoSound();
    }

    private void Awake()
    {
        // 初始化單例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切換場景不銷毀
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 將音效加入字典
        foreach (Sound sound in sounds)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound.clip);
            }
        }

        // 初始化 PlayerManager
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("在 'PlayerSys' 上未找到 PlayerManager 組件！");
                return;
            }
        }
        else
        {
            Debug.LogError("未找到名稱為 'PlayerSys' 的物件！");
            return;
        }
        BindVolumeSlider();

        // 設置音量滑桿
        InitializeVolumeSlider();

    }

    private void InitializeVolumeSlider()
    {
        if (volumeSlider != null)
        {
            // 從 PlayerData 加載音量，若無則預設為 1（最大音量）
            float savedVolume = playerManager.playerData.Volume;
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume;
            Debug.Log("savedVolume:" + savedVolume);
            // 添加滑桿事件監聽
            volumeSlider.onValueChanged.AddListener(UpdateVolume);

        }
        else
        {
            Debug.LogError("未設置音量滑桿（Slider）！");
        }
    }

    private void BindVolumeSlider()
    {
        // 動態尋找滑桿物件
        //GameObject sliderObject = GameObject.Find("vfx_Slider");
        GameObject sliderObject = FindInactiveObjectByName("vfx_Slider");
        if (sliderObject != null)
        {
            Slider newSlider = sliderObject.GetComponent<Slider>();
            if (newSlider != null)
            {
                volumeSlider = newSlider;

                // 設置滑桿的初始值
                float savedVolume = playerManager != null ? playerManager.playerData.Volume : 1f;
                audioSource.volume = savedVolume;
                volumeSlider.value = savedVolume;

                // 移除舊的監聽器，避免重複綁定
                volumeSlider.onValueChanged.RemoveAllListeners();

                // 添加滑桿事件監聽
                volumeSlider.onValueChanged.AddListener(UpdateVolume);
            }
            else
            {
                Debug.LogError("尋找到的物件上沒有 Slider 組件！");
            }
        }
        else
        {
            Debug.LogWarning("未找到名稱為 'VolumeSlider' 的音量滑桿物件！");
        }
    }
    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.hideFlags == HideFlags.None) // 確保不是隱藏物件
            {
                return obj;
            }
        }
        Debug.LogError($"未找到名為 '{name}' 的物件！");
        return null;
    }

    private void UpdateVolume(float volume)
    {
        audioSource.volume = volume;

        // 更新 PlayerData 的音量設置
        if (playerManager != null )
        {
            playerManager.playerData.Volume = volume;
            
        }
        else
        {
            Debug.LogError("無法存儲音量至 PlayerData！");
        }
    }

    // 撥放音效
    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"音效名稱 '{soundName}' 不存在！");
        }
    }
    private bool isMotoPlaying = false; // 紀錄當前是否在播放摩托音效

    public void UpdateMotoSound()
    {
        if (PlayerController.isRiding && !isMotoPlaying)
        {
            PlayMotoSound();
        }
        else if (!PlayerController.isRiding && isMotoPlaying)
        {
            StopMotoSound();
        }
    }

    private void PlayMotoSound()
    {
        if (!audioSource.isPlaying || audioSource.clip != soundDictionary["moto"])
        {
            if (soundDictionary.TryGetValue("moto", out AudioClip motoClip))
            {
                audioSource.clip = motoClip;
                audioSource.loop = true;
                audioSource.Play();
                isMotoPlaying = true;
            }
            else
            {
                Debug.LogError("音效 'moto' 不存在於字典中！");
            }
        }
    }

    private void StopMotoSound()
    {
        if (audioSource.isPlaying && audioSource.clip == soundDictionary["moto"])
        {
            audioSource.Stop();
            audioSource.clip = null; // 清除音效
            isMotoPlaying = false;
        }
    }


}
