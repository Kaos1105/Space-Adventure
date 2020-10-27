using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioMixer audioMixer;
    public Slider slider; 

    private Animator animator;
    private GameObject ship;
    

    public void Start()
    {
        animator = this.GetComponent<Animator>();
        ship = GameObject.FindGameObjectWithTag("Player");
        slider.value = PlayerPrefs.GetFloat("MusicVol");
        Time.timeScale = 1.0f;
    }
    public void StartGame()
    {
        StartCoroutine(LoadScene());
    }

    public void AdjustVolume(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol)*20);
        PlayerPrefs.SetFloat("MusicVol", vol);
        Debug.Log(vol*1000);
    }

    public void OpenQuitWindow()
    {
        audioManager.Play("ButtonClick");
        animator.SetBool("isQuitOpen", true);
    }
    public void CloseQuitWindow()
    {
        audioManager.Play("ButtonCloseClick");
        animator.SetBool("isQuitOpen", false);
    }

    public void OpenOptionWindow()
    {
        audioManager.Play("ButtonClick");
        animator.SetBool("isOptionOpen", true);
    }
    public void CloseOptionWindow()
    { 
        audioManager.Play("ButtonCloseClick");
        animator.SetBool("isOptionOpen", false);
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
    IEnumerator LoadScene()
    {
        audioManager.Play("ButtonClick");
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            bool isMenuOpen = animator.GetBool("isMenuOpen");
            if (isMenuOpen)
                animator.SetBool("isMenuOpen", false);
            audioManager.FadeOut("Theme", 1.85f);
            audioManager.Play("EngineStart");
            yield return new WaitForSeconds(1.35f);
            ship.GetComponent<ShipFloating>().enabled = false;
            ship.GetComponent<Rigidbody>().velocity = new Vector3(0.5f, 2, 5);
            ship.GetComponent<Rigidbody>().angularVelocity = new Vector3(-0.2f, 0, 0);
            yield return new WaitForSeconds(2f);
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            Debug.Log(asyncOperation.progress);
            yield return null; 
        }

    }
}
