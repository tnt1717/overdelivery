using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AudioManager.Instance.PlaySound("tap2");
/// </summary>
public class AudioManager : MonoBehaviour
{
    // ����ģʽ
    public static AudioManager Instance;

    // �ֵ�탦����Ч
    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public AudioSource audioSource; // ��춲�����Ч�� AudioSource

    // �O����Ч�б�
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sounds;

    // Slider ��춿�������
    private Slider volumeSlider;


    private Coroutine motoCoroutine;

    // PlayerManager ����
    private PlayerManager playerManager;
    //public PlayerData playerData;
    private void Update()
    {
        UpdateMotoSound();
    }

    private void Awake()
    {
        // ��ʼ������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �ГQ�������N��
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ����Ч�����ֵ�
        foreach (Sound sound in sounds)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound.clip);
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
        BindVolumeSlider();

        // �O���������U
        InitializeVolumeSlider();

    }

    private void InitializeVolumeSlider()
    {
        if (volumeSlider != null)
        {
            // �� PlayerData ���d���������o�t�A�O�� 1�����������
            float savedVolume = playerManager.playerData.Volume;
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume;
            Debug.Log("savedVolume:" + savedVolume);
            // ��ӻ��U�¼��O 
            volumeSlider.onValueChanged.AddListener(UpdateVolume);

        }
        else
        {
            Debug.LogError("δ�O���������U��Slider����");
        }
    }

    private void BindVolumeSlider()
    {
        // �ӑB���һ��U���
        //GameObject sliderObject = GameObject.Find("vfx_Slider");
        GameObject sliderObject = FindInactiveObjectByName("vfx_Slider");
        if (sliderObject != null)
        {
            Slider newSlider = sliderObject.GetComponent<Slider>();
            if (newSlider != null)
            {
                volumeSlider = newSlider;

                // �O�û��U�ĳ�ʼֵ
                float savedVolume = playerManager != null ? playerManager.playerData.Volume : 1f;
                audioSource.volume = savedVolume;
                volumeSlider.value = savedVolume;

                // �Ƴ��f�ıO �����������}����
                volumeSlider.onValueChanged.RemoveAllListeners();

                // ��ӻ��U�¼��O 
                volumeSlider.onValueChanged.AddListener(UpdateVolume);
            }
            else
            {
                Debug.LogError("���ҵ�������ϛ]�� Slider �M����");
            }
        }
        else
        {
            Debug.LogWarning("δ�ҵ����Q�� 'VolumeSlider' ���������U�����");
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

    private void UpdateVolume(float volume)
    {
        audioSource.volume = volume;

        // ���� PlayerData �������O��
        if (playerManager != null )
        {
            playerManager.playerData.Volume = volume;
            
        }
        else
        {
            Debug.LogError("�o���惦������ PlayerData��");
        }
    }

    // �ܷ���Ч
    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"��Ч���Q '{soundName}' �����ڣ�");
        }
    }
    private bool isMotoPlaying = false; // �o䛮�ǰ�Ƿ��ڲ���Ħ����Ч

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
                Debug.LogError("��Ч 'moto' ��������ֵ��У�");
            }
        }
    }

    private void StopMotoSound()
    {
        if (audioSource.isPlaying && audioSource.clip == soundDictionary["moto"])
        {
            audioSource.Stop();
            audioSource.clip = null; // �����Ч
            isMotoPlaying = false;
        }
    }


}
