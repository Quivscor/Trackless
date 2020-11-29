using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeBackgroundsInMenu : MonoBehaviour
{
    public GameObject aboutObject;
    public GameObject mainObject;

    public void OpenAboutMenu()
    {
        aboutObject.SetActive(true);
        mainObject.SetActive(false);
    }

    public void OpenMainMenu()
    {
        aboutObject.SetActive(false);
        mainObject.SetActive(true);
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
