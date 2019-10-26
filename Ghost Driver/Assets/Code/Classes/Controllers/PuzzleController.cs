using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class PuzzleController : GameController
{
    private string _Scene = "";
    public static PuzzleController Instance { get; private set; }

    private void Awake ()
    {
        // If more than one instance exists, delete any extras.
        if (Instance != null && Instance != this)
        {
            Destroy (this);
            return;
        }

        Instance = this;
    }

    private void OnEnable ()
    {
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
        PuzzleSignals.OnPuzzleComplete += OnPuzzleComplete;
    }

    private void OnPuzzleReset ()
    {
        if (_IsPaused)
        {
            PauseGame ();
        }
    }

    private void OnPuzzleComplete ()
    {
        SceneManager.LoadScene (_Scene);
    }

    private void OnDisable ()
    {
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
        PuzzleSignals.OnPuzzleComplete += OnPuzzleComplete;
    }
}
