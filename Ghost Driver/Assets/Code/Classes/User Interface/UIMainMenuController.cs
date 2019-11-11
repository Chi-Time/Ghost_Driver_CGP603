using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    private void Awake ()
    {
        Time.timeScale = 1.0f;
        LogBook.ClearData ();
    }

    public void StartGame (string sceneName)
    {
        SceneLoader.Instance.Load (sceneName);
    }

    public void DisplayScreen (GameObject screenToDisplay)
    {
        this.gameObject.SetActive (false);
        screenToDisplay.SetActive (true);
    }

    public void QuitGame ()
    {
        Application.Quit ();
    }
}
