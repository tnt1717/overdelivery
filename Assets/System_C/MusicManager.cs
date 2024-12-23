using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    // 字典用於存放各場景的音樂
    private Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();
    public AudioSource musicSource; // 用於播放背景音樂的 AudioSource

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName; // 場景名稱
        public AudioClip musicClip; // 對應音樂
    }

    public SceneMusic[] sceneMusicArray;
    private Slider musicVolumeSlider;


    // PlayerManager 引用
    private PlayerManager playerManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切換場景時保留
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 將場景音樂加入字典
        foreach (SceneMusic sceneMusic in sceneMusicArray)
        {
            if (!musicDictionary.ContainsKey(sceneMusic.sceneName))
            {
                musicDictionary.Add(sceneMusic.sceneName, sceneMusic.musicClip);
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

        // 初始化音量滑桿
        BindMusicVolumeSlider();
        InitializeMusicVolumeSlider();

        // 監聽場景切換事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeMusicVolumeSlider()
    {
        if (musicVolumeSlider != null)
        {
            // 從 PlayerData 加載音量，若無則預設為 1（最大音量）
            float savedVolume = playerManager.playerData.MusicVolume;
            musicSource.volume = savedVolume;
            musicVolumeSlider.value = savedVolume;

            // 添加滑桿事件監聽
            musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }
        else
        {
            
        }
    }

    private void UpdateMusicVolume(float volume)
    {
        musicSource.volume = volume;

        // 更新 PlayerData 的音量設置
        if (playerManager != null )
        {
            playerManager.playerData.MusicVolume = volume;
        }
        else
        {
            Debug.LogError("無法存儲音樂音量至 PlayerData！");
        }
    }

    // 當場景載入時觸發
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 切換場景後播放對應的音樂
        PlayMusicForScene(scene.name);

        // 動態尋找並綁定新的音量滑桿
        BindMusicVolumeSlider();
    }
    private void BindMusicVolumeSlider()
    {
        // 根據名稱尋找滑桿
        //GameObject sliderObject = GameObject.Find("BGM_Slider");
        GameObject sliderObject = FindInactiveObjectByName("BGM_Slider");
        if (sliderObject != null)
        {
            Slider newSlider = sliderObject.GetComponent<Slider>();
            if (newSlider != null)
            {
                musicVolumeSlider = newSlider;

                // 設置滑桿的初始值
                float savedVolume = playerManager.playerData.MusicVolume;
                musicSource.volume = savedVolume;
                musicVolumeSlider.value = savedVolume;

                // 移除舊的監聽器，避免重複綁定
                musicVolumeSlider.onValueChanged.RemoveAllListeners();

                // 添加滑桿事件監聽
                musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
            }
            else
            {
                Debug.LogError("尋找到的物件上沒有 Slider 組件！");
            }
        }
        else
        {
            Debug.LogWarning("未找到名稱為 'MusicVolumeSlider' 的音量滑桿物件！");
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
    // 播放對應場景的音樂
    private void PlayMusicForScene(string sceneName)
    {
        if (musicDictionary.TryGetValue(sceneName, out AudioClip clip))
        {
            if (musicSource.clip != clip) // 確保不重複播放相同音樂
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"場景 '{sceneName}' 沒有指定的背景音樂！");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
