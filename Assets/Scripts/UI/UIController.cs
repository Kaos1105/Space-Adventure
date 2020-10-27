using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIController : MonoBehaviour
{

    public Slider volumeSlider;

    public AudioMixer audioMixer;

    public AudioSource buttonClick;
    public AudioSource buttonClose;

    private Animator animator;

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        float volume = PlayerPrefs.GetFloat("MusicVol");
        volumeSlider.value = volume;
    }
    public void AdjustVolume(float value)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat("MusicVol", value);
    }
    IEnumerator LoadSceneAsync(string name)
    {
        Time.timeScale = 1.0f;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            animator.SetBool("isChangingScene", true);
            yield return new WaitForSeconds(1.5f);
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void LoadMenu()
    {
        buttonClick.Play();
        StartCoroutine(LoadSceneAsync("Menu"));

    }
    public void Replay()
    { 
        buttonClick.Play();
        StartCoroutine(LoadSceneAsync("Main"));

    }
    public void OpenPauseMenu()
    {
        buttonClick.Play();
        animator.SetBool("isPauseOpened", true);
        Time.timeScale = 0.0f;
    }
    private void Update()
    {

    }
    public void ClosePauseMenu()
    {
        buttonClose.Play();
        animator.SetBool("isPauseOpened", false);
        StartCoroutine(accelerateTimeScale());
    }
    IEnumerator accelerateTimeScale()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(2.5f * 0.2f);
        Time.timeScale = 1.0f;
    }

}
