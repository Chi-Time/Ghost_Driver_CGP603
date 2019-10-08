using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    public void StartGame (string sceneName)
    {
        SceneManager.LoadScene (sceneName);
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
