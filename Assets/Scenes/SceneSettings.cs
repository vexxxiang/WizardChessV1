using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneSettings: MonoBehaviour
{
    public void Online()
    {
        SceneManager.LoadScene("Offline");
    }
    public void Offline()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LeaveToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}