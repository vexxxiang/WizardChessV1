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

        // Jeœli ju¿ w zbudowanej aplikacji, zamykamy grê
        Application.Quit();

    }
}