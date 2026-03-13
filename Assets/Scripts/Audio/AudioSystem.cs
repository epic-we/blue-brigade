using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance { get; private set; }

    private Queue<AudioSource> audioPool = new Queue<AudioSource>();
    private List<AudioSource> activeSources = new List<AudioSource>();
    private GameObject poolContainer;

    [Range(0.8f, 1.2f)] public float minPitch = 0.95f;
    [Range(0.8f, 1.2f)] public float maxPitch = 1.05f;

    [SerializeField]
    private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        poolContainer = new GameObject("AudioPool");
        poolContainer.transform.SetParent(transform);
    }

    private AudioSource GetAudioSource()
    {
        if (audioPool.Count > 0)
        {
            var source = audioPool.Dequeue();
            source.gameObject.SetActive(true);
            activeSources.Add(source);
            return source;
        }

        var newSource = new GameObject("PooledAudioSource").AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        newSource.transform.SetParent(poolContainer.transform);
        newSource.spatialBlend = 0f; // 2D Sound
        activeSources.Add(newSource);
        return newSource;
    }

    private void ReturnToPool(AudioSource source)
    {
        source.Stop();
        source.gameObject.SetActive(false);
        activeSources.Remove(source);
        audioPool.Enqueue(source);
    }

    public static void PlaySound(AudioClip[] clips, float volume = 1f, bool randomPitch = true)
    {
        if (Instance == null || clips == null || clips.Length == 0) return;

        var clip = clips[Random.Range(0, clips.Length)];
        var source = Instance.GetAudioSource();

        source.clip = clip;
        source.volume = volume;
        source.pitch = randomPitch ? Random.Range(Instance.minPitch, Instance.maxPitch) : 1f;
        source.Play();

        Instance.StartCoroutine(Instance.ReleaseAfterPlay(source));
    }

    private IEnumerator ReleaseAfterPlay(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        ReturnToPool(source);
    }
}
