using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseScreenController : MonoBehaviour, IWakeable
{
    public void Waken ()
    {
        GameSignals.OnLevelPaused += OnLevelPaused;
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;

        //Unity hack because it never calls ondestory on objects that are inactive for the whole scene.
        // It also doesn't fire off many other basic functions because it's a useless engine.
        this.gameObject.SetActive (true);
        this.gameObject.SetActive (false);
    }

    private void OnLevelPaused (bool isPaused)
    {
        if (isPaused)
            this.gameObject.SetActive (true);
        else
            this.gameObject.SetActive (false);
    }

    private void OnPuzzleReset ()
    {
        this.gameObject.SetActive (false);
    }

    public void OnDestroy ()
    {
        GameSignals.OnLevelPaused -= OnLevelPaused;
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
    }

    public void ContinueExploration ()
    {
        GameSignals.PauseLevel (false);
        this.gameObject.SetActive (false);
    }

    public void ContinuePuzzle ()
    {
        GameSignals.PauseLevel (false);
        this.gameObject.SetActive (false);
    }

    public void ShowScreen (GameObject screen)
    {
        this.gameObject.SetActive (false);
        screen.SetActive (true);
    }

    //TODO: Find a way to get the loading screen to do this job maybe make it react to a state.
    public void BackToMenu (string sceneName)
    {
        SceneManager.LoadScene (sceneName);
    }

    public void Reset ()
    {
        PuzzleSignals.ResetPuzzle ();
    }

    public void QuitGame ()
    {
        Application.Quit ();
    }
}
