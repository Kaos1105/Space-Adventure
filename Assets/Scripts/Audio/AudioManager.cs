using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource buttonClickSound;
    public AudioSource buttonCloseSound;
    public AudioSource engineStartSound;
    public AudioSource themeMusic; 
    private Dictionary<string, AudioSource> identity;
    private void Start()
    {
        identity = new Dictionary<string, AudioSource>()
        {
            {"ButtonClick",buttonClickSound },
            {"ButtonCloseClick", buttonCloseSound },
            {"EngineStart",  engineStartSound },
            {"Theme", themeMusic },
        };
    } 
    public void SetVolume(float vol)
    {
        if(vol>=0.0f && vol <= 1.0f)
        {
           foreach(var entry in identity)
            {
                entry.Value.volume = vol;
            }
        }
    }
    public void FadeOut(string music, float time)
    {
        AudioSource audioSource;
        if (!identity.TryGetValue(music, out audioSource))
        {
            Debug.Log("Can't find the specified sound");
            return;
        }
        StartCoroutine(FadeOut(audioSource, time));
    }
    IEnumerator FadeOut(AudioSource audio, float time)
    {
        float startVolume = audio.volume;

        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / time;

            yield return null;
        }

        audio.Stop();
        audio.volume = startVolume;
    }
    public void Play(string sound)
    {
        AudioSource audioSource;
        if (!identity.TryGetValue(sound, out audioSource))
        {
            Debug.Log("Can't find the specified sound");
            return;
        }
        else if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
