using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[System.Serializable]
public class AudioFX
{
    public string name;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume = 0.8f;
    public bool randomizePitch;
    public Vector2 randomizePitchValues = new(0.8f, 1.3f);
    public bool loop;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer Settings")]
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioMixerGroup m_musicMixer, m_sfxMixer;

    [Header("Music Settings")]
    [SerializeField] private List<AudioFX> m_musicList;

    [Header("SFX Settings")]
    [SerializeField] private List<AudioFX> m_sfxList;
    [SerializeField] private int m_sfxPoolSize = 10;
    [SerializeField] private bool m_willGrowSFXPool = true;

    private GameObject m_musicSourceObject;  // Central object for managing music
    private AudioSource m_currentMusicSource;

    private List<GameObject> sfxPool = new List<GameObject>();  // Pre-pooled SFX objects

    protected override void Awake()
    {
        base.Awake();
        InitializeMusicSource();
        InitializeSFXPool();
    }

    #region Music Management

    private void InitializeMusicSource()
    {
        if (m_musicSourceObject == null)
        {
            m_musicSourceObject = new GameObject("MusicSource", typeof(AudioSource));
            m_musicSourceObject.transform.SetParent(transform);
            m_currentMusicSource = m_musicSourceObject.GetComponent<AudioSource>();
            m_currentMusicSource.outputAudioMixerGroup = m_musicMixer;
        }
    }

    public void PlayMusic(string musicName, bool fade = false, float fadeDuration = 1.0f)
    {
        AudioFX music = GetAudioByName(musicName, m_musicList);
        if (music != null)
        {
            if (fade && m_currentMusicSource.isPlaying)
            {
                StartCoroutine(FadeOutMusic(m_currentMusicSource, fadeDuration, () => StartNewMusic(music)));
            }
            else
            {
                StartNewMusic(music);
            }
        }
    }

    private void StartNewMusic(AudioFX music)
    {
        m_currentMusicSource.clip = music.audioClip;
        m_currentMusicSource.volume = music.volume;
        m_currentMusicSource.loop = music.loop;
        m_currentMusicSource.Play();
    }

    public void StopMusic(bool fade = false, float fadeDuration = 1.0f)
    {
        if (fade)
        {
            StartCoroutine(FadeOutMusic(m_currentMusicSource, fadeDuration));
        }
        else
        {
            m_currentMusicSource.Stop();
        }
    }

    private IEnumerator FadeOutMusic(AudioSource source, float duration, System.Action onComplete = null)
    {
        float startVolume = source.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
        onComplete?.Invoke();
    }

    #endregion

    #region SFX Management

    private void InitializeSFXPool()
    {
        foreach (var sfx in m_sfxList)
        {
            for (int i = 0; i < m_sfxPoolSize; i++)
            {
                CreatePooledSFX(sfx);
            }
        }
    }

    private void CreatePooledSFX(AudioFX sfx)
    {
        GameObject sfxObject = new GameObject(sfx.name + "_SFX", typeof(AudioSource));
        sfxObject.transform.SetParent(transform);

        AudioSource audioSource = sfxObject.GetComponent<AudioSource>();
        audioSource.clip = sfx.audioClip;
        audioSource.volume = sfx.volume;
        audioSource.loop = sfx.loop;
        audioSource.outputAudioMixerGroup = m_sfxMixer;
        audioSource.playOnAwake = false;

        sfxObject.SetActive(false);
        sfxPool.Add(sfxObject);
    }

    public void PlaySFX(string sfxName, Vector3 position, bool fadeIn = false, float fadeDuration = 1.0f)
    {
        GameObject sfxObject = GetPooledSFX(sfxName);
        if (sfxObject != null)
        {
            sfxObject.SetActive(true);
            AudioSource sfxSource = sfxObject.GetComponent<AudioSource>();
            sfxSource.transform.position = position;
            sfxSource.spatialBlend = 1f;

            if (fadeIn)
            {
                sfxSource.volume = 0;
                StartCoroutine(FadeInSFX(sfxSource, sfxSource.volume, fadeDuration));
            }

            sfxSource.Play();
            StartCoroutine(ReturnSFXToPoolAfterPlay(sfxSource));
        }
        else
        {
            Debug.LogWarning("No available SFX source in the pool!");
        }
    }

    public void StopSFX(string sfxName, bool fadeOut = false, float fadeDuration = 1.0f)
    {
        foreach (var sfxObject in sfxPool)
        {
            if (sfxObject.name == sfxName + "_SFX" && sfxObject.activeInHierarchy)
            {
                AudioSource sfxSource = sfxObject.GetComponent<AudioSource>();
                if (fadeOut)
                {
                    StartCoroutine(FadeOutSFX(sfxSource, fadeDuration));
                }
                else
                {
                    sfxSource.Stop();
                    sfxObject.SetActive(false);
                }
                break;
            }
        }
    }

    private GameObject GetPooledSFX(string sfxName)
    {
        foreach (var sfxObject in sfxPool)
        {
            if (!sfxObject.activeInHierarchy && sfxObject.name.Contains(sfxName))
            {
                return sfxObject;
            }
        }

        // If pool is set to grow, create new SFX object
        if (m_willGrowSFXPool)
        {
            AudioFX sfx = GetAudioByName(sfxName, m_sfxList);
            if (sfx != null)
            {
                CreatePooledSFX(sfx);
                return GetPooledSFX(sfxName);
            }
        }

        return null;
    }

    private IEnumerator ReturnSFXToPoolAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.gameObject.SetActive(false);
    }

    private IEnumerator FadeInSFX(AudioSource source, float targetVolume, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0, targetVolume, time / duration);
            yield return null;
        }
    }

    private IEnumerator FadeOutSFX(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.Stop();
        source.gameObject.SetActive(false);
    }

    #endregion

    #region Utility Methods

    private AudioFX GetAudioByName(string name, List<AudioFX> audioList)
    {
        foreach (AudioFX audio in audioList)
        {
            if (audio.name == name)
                return audio;
        }
        return null;
    }

    #endregion
}
