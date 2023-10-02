using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConroller : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Breach");
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
