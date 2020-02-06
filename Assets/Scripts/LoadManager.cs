using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public void LoadLevel(string Level)
    {
        SceneManager.LoadScene(Level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
