using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Implement failure state properly.
//TODO: Consider making custom monobehaviour class that has an override so that when it awakes it also awakes all inherited classes through a method call even if they're inactive.

class PuzzleController : GameController
{
    public static PuzzleController Instance { get; private set; }

    [Tooltip ("The song to play as the background music for the level.")]
    [SerializeField] private AudioClip _BGM = null;
    [Tooltip ("Reference to the game over screen.")]
    [SerializeField] private UIGameOverScreenController _GameOverScreenController = null;

    private void Awake ()
    {
        SetInstance ();
    }

    private void SetInstance ()
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
        ChangeMusicTrack ();

        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
        PuzzleSignals.OnPuzzleFailed += OnPuzzleFailed;
    }

    private void ChangeMusicTrack ()
    {
        if (MusicController.Instance != null)
        {
            var currentTrack = MusicController.Instance.GetCurrentTrackName ();

            if (currentTrack != _BGM.name)
                MusicController.Instance.ChangeTrack (_BGM);
        }
    }

    private void OnPuzzleReset ()
    {
        if (_IsPaused)
        {
            PauseGame ();
        }
    }

    private void OnPuzzleFailed ()
    {
        _PauseScreen.gameObject.SetActive (false);
        _GameOverScreenController.gameObject.SetActive (true);
    }

    private void OnDisable ()
    {
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
        PuzzleSignals.OnPuzzleFailed -= OnPuzzleFailed;
    }
}
