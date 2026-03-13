using UnityEngine;
using System.Collections;

public class AudioClipChanger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip nextClip;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            StartCoroutine(CheckAudioEnd());
        }
    }

    private IEnumerator CheckAudioEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        PlayNextClip();
    }

    private void PlayNextClip()
    {
        if (nextClip != null)
        {
            
            audioSource.clip = nextClip;
            audioSource.Play();
            audioSource.loop = true;
        }
        else
        {
            Debug.LogWarning("Next audio clip is not set!");
        }
    }
}
