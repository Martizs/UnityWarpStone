using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButonUI : MonoBehaviour
{
    public void TryAgainButton()
    {
        SceneManager.LoadScene(0);
    }

    public void EndGameButton()
    {
        Application.Quit();
    }
}
