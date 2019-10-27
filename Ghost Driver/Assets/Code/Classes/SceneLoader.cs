﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class SceneLoader : MonoBehaviour
{
    [Tooltip ("The scene to load.")]
    [SerializeField] private string _Scene = "SC_Main_Menu";
    [Tooltip ("The progress bar to update.")]
    [SerializeField] private Image _ProgressBar = null;

    private void Awake ()
    {
        PuzzleSignals.OnPuzzleComplete += OnPuzzleComplete;
        ExplorationSignals.OnTransitionFinished += OnTransitionFinished;
    }

    private void OnTransitionFinished ()
    {
        Setup ();
    }
    
    private void OnPuzzleComplete ()
    {
        Setup ();
    }

    private void Setup ()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive (true);
        }

        StartCoroutine (LoadNewScene ());
    }

    private void OnDestroy ()
    {
        PuzzleSignals.OnPuzzleComplete -= OnPuzzleComplete;
        ExplorationSignals.OnTransitionFinished -= OnTransitionFinished;
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene ()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync (_Scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            _ProgressBar.fillAmount = async.progress;
            yield return new WaitForEndOfFrame ();
        }
    }
}