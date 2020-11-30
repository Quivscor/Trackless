using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvasController : MonoBehaviour
{
    private void Awake()
    {
        DisplayCanvas(false);
    }

    public void DisplayCanvas(bool value)
    {
        GetComponent<Canvas>().enabled = value;

        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoadMapAgain()
    {
        TimeCounter.ResetTime();
        SceneManager.LoadScene("Map Generating");
    }

    public void BackToMainMenu()
    {
        TimeCounter.ResetTime();
        SceneManager.LoadScene("MainMenu");
    }
}
