using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    private SceneLoader _Loader = null;

    private void Awake ()
    {
        var loaders = gameObject.FindAllObjectsOfType<SceneLoader> ();
        _Loader = loaders[0];
    }

    public void StartGame (string sceneName)
    {
        _Loader.Load ();
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
