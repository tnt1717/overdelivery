using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    // �ֵ���춴�Ÿ�����������
    private Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();
    public AudioSource musicSource; // ��춲��ű��������� AudioSource

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName; // �������Q
        public AudioClip musicClip; // ��������
    }

    public SceneMusic[] sceneMusicArray;
    private Slider musicVolumeSlider;


    // PlayerManager ����
    private PlayerManager playerManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �ГQ�����r����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ���������������ֵ�
        foreach (SceneMusic sceneMusic in sceneMusicArray)
        {
            if (!musicDictionary.ContainsKey(sceneMusic.sceneName))
            {
                musicDictionary.Add(sceneMusic.sceneName, sceneMusic.musicClip);
            }
        }

        // ��ʼ�� PlayerManager
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("�� 'PlayerSys' ��δ�ҵ� PlayerManager �M����");
                return;
            }
        }
        else
        {
            Debug.LogError("δ�ҵ����Q�� 'PlayerSys' �������");
            return;
        }

        // ��ʼ���������U
        BindMusicVolumeSlider();
        InitializeMusicVolumeSlider();

        // �O �����ГQ�¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeMusicVolumeSlider()
    {
        if (musicVolumeSlider != null)
        {
            // �� PlayerData ���d���������o�t�A�O�� 1�����������
            float savedVolume = playerManager.playerData.MusicVolume;
            musicSource.volume = savedVolume;
            musicVolumeSlider.value = savedVolume;

            // ��ӻ��U�¼��O 
            musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }
        else
        {
            
        }
    }

    private void UpdateMusicVolume(float volume)
    {
        musicSource.volume = volume;

        // ���� PlayerData �������O��
        if (playerManager != null )
        {
            playerManager.playerData.MusicVolume = volume;
        }
        else
        {
            Debug.LogError("�o���惦���������� PlayerData��");
        }
    }

    // �������d��r�|�l
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �ГQ�����Ქ�Ō���������
        PlayMusicForScene(scene.name);

        // �ӑB���ҁK�����µ��������U
        BindMusicVolumeSlider();
    }
    private void BindMusicVolumeSlider()
    {
        // �������Q���һ��U
        //GameObject sliderObject = GameObject.Find("BGM_Slider");
        GameObject sliderObject = FindInactiveObjectByName("BGM_Slider");
        if (sliderObject != null)
        {
            Slider newSlider = sliderObject.GetComponent<Slider>();
            if (newSlider != null)
            {
                musicVolumeSlider = newSlider;

                // �O�û��U�ĳ�ʼֵ
                float savedVolume = playerManager.playerData.MusicVolume;
                musicSource.volume = savedVolume;
                musicVolumeSlider.value = savedVolume;

                // �Ƴ��f�ıO �����������}����
                musicVolumeSlider.onValueChanged.RemoveAllListeners();

                // ��ӻ��U�¼��O 
                musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
            }
            else
            {
                Debug.LogError("���ҵ�������ϛ]�� Slider �M����");
            }
        }
        else
        {
            Debug.LogWarning("δ�ҵ����Q�� 'MusicVolumeSlider' ���������U�����");
        }
    }


    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.hideFlags == HideFlags.None) // �_�������[�����
            {
                return obj;
            }
        }
        Debug.LogError($"δ�ҵ����� '{name}' �������");
        return null;
    }
    // ���Ō�������������
    private void PlayMusicForScene(string sceneName)
    {
        if (musicDictionary.TryGetValue(sceneName, out AudioClip clip))
        {
            if (musicSource.clip != clip) // �_�������}������ͬ����
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"���� '{sceneName}' �]��ָ���ı���������");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
