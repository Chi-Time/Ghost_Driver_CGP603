using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseScreenController : MonoBehaviour
{
    public void Continue ()
    {
        print ("Doing");
        ExplorationController.Instance.PauseGame ();
        this.gameObject.SetActive (false);
    }

    public void ShowScreen (GameObject screen)
    {
        this.gameObject.SetActive (false);
        screen.SetActive (true);
    }

    public void BackToMenu (string sceneName)
    {
        SceneManager.LoadScene (sceneName);
    }

    public void QuitGame ()
    {
        Application.Quit ();
    }
}
