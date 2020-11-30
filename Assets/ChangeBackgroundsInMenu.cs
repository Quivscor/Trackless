using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeBackgroundsInMenu : MonoBehaviour
{
    public GameObject aboutObject;
    public GameObject mainObject;
    public GameObject instructionObject;
    public GameObject settingsObject;
    public Toggle oldControlsObject;
    public Toggle musicObject;
    public AudioSource music;

    private void OnEnable()
    {
        if(PlayerPrefs.HasKey("Controls"))
        {
            if (PlayerPrefs.GetInt("Controls") == 1)
                oldControlsObject.isOn = true;
            else
                oldControlsObject.isOn = false;
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                music.Play();
                musicObject.isOn = true;
            }  
            else
                musicObject.isOn = false;
        }

    }


public void OpenAboutMenu()
    {
        aboutObject.SetActive(true);
        mainObject.SetActive(false);
    }
    public void OpenInstructionMenu()
    {
        instructionObject.SetActive(true);
        mainObject.SetActive(false);
    }
    public void OpenSettingsMenu()
    {
        settingsObject.SetActive(true);
        mainObject.SetActive(false);
    }
    public void OpenMainMenu()
    {
        aboutObject.SetActive(false);
        instructionObject.SetActive(false);
        settingsObject.SetActive(false);
        mainObject.SetActive(true);
    }
    public void ChangeControls(bool turnedOn)
    {
        if(PlayerPrefs.HasKey("Controls"))
        {
            if (!turnedOn)
            {
                Debug.Log("New controls");
                PlayerPrefs.SetInt("Controls", 0);
            }
            else
            {
                Debug.Log("Old controls");
                PlayerPrefs.SetInt("Controls", 1);
            }
                    
        }
        else
        {
            PlayerPrefs.SetInt("Controls", 0);
        }
        PlayerPrefs.Save();
    }
    public void SetMusic(bool turnedOn)
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (!turnedOn)
            {
                music.Stop();
                PlayerPrefs.SetInt("Music", 0);
            }
            else
            {
                music.Play();
                PlayerPrefs.SetInt("Music", 1);
            }

        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        PlayerPrefs.Save();
    }

    public void Play()
    {
        SceneManager.LoadScene("Map Generating");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
