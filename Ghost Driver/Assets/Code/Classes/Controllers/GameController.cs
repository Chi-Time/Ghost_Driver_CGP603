using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

//TODO: Consolidate puzzle complete and transition into 1 thing.
//TODO: Find a better way to handle the level transition, the current fade thing feels bad.
//TODO: Make it so that log menu can't re-activate pause menu whilst in the log screen.

[RequireComponent (typeof (SceneController))]
class GameController : MonoBehaviour
{
    [Tooltip ("The song to play as the background music for the level.")]
    [SerializeField] protected AudioClip _BGM = null;
    [Tooltip ("The next scene to transfer to.")]
    [SerializeField] protected string _NextScene = "";
    [Tooltip ("The main menu scene.")]
    [SerializeField] protected string _MenuScene = "SC_Main_Menu";

    protected Blur _Blur = null;
    protected bool _IsPaused = false;
    protected GameStates _CurrentState = GameStates.Playing;

    protected virtual void Start ()
    {
        Time.timeScale = 1.0f;
        _Blur = gameObject.FindFirstObjectOfType<Blur> ();
        _Blur.enabled = false;
    }

    protected virtual void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            if (_CurrentState == GameStates.Playing || _CurrentState == GameStates.Paused)
            {
                _IsPaused = !_IsPaused;
                GameSignals.PauseLevel (_IsPaused);
            }
        }

        if (Input.GetKeyDown (KeyCode.R))
        {
            if (_CurrentState == GameStates.Playing)
            {
                PuzzleSignals.ResetPuzzle ();
            }
        }
    }

    protected virtual void OnApplicationFocus (bool focus)
    {
        //TODO: Make it so that when the application is re-focused
        // We fix the mouse cursor by checking if we're in a menu or not
        // And then locking or unlocking the mouse.
    }

    protected virtual void OnEnable ()
    {
        ChangeMusicTrack ();

        GameSignals.OnLevelPaused += OnLevelPaused;
        ExplorationSignals.OnRelicCollected += OnRelicCollected;
        ExplorationSignals.OnRelicQuit += OnRelicQuit;
        ExplorationSignals.OnDialogueStarted += OnDialogueStarted;
        ExplorationSignals.OnDialogueQuit += OnDialogueQuit;
        PuzzleSignals.OnPuzzleFailed += OnPuzzleFailed;
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
        PuzzleSignals.OnPuzzleComplete += OnPuzzleComplete;
        ExplorationSignals.OnTransitionFinished += OnTransitionFinished;
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

    protected virtual void OnLevelPaused (bool shouldPause)
    {
        _CurrentState = GameStates.Paused;
        _IsPaused = shouldPause;

        if (_IsPaused)
        {
            FreezeGame ();
        }
        else
        {
            ResumeGame ();
        }
    }

    private void FreezeGame ()
    {
        _Blur.enabled = true;
        Time.timeScale = 0.0f;

        CursorLock (false);
    }

    private void ResumeGame ()
    {
        _Blur.enabled = false;
        Time.timeScale = 1.0f;

        CursorLock (true);
    }

    private void CursorLock (bool shouldLock)
    {
        if (shouldLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    protected virtual void OnRelicCollected (RelicInfo relic)
    {
        _CurrentState = GameStates.Relic;
        FreezeGame ();
    }

    protected virtual void OnRelicQuit ()
    {
        _CurrentState = GameStates.Playing;
        ResumeGame ();
    }

    protected virtual void OnDialogueStarted (DialogueManager dialogue)
    {
        _CurrentState = GameStates.Dialogue;
        CursorLock (false);
    }

    protected virtual void OnDialogueQuit ()
    {
        _CurrentState = GameStates.Playing;
        ResumeGame ();
    }

    protected virtual void OnPuzzleFailed ()
    {
        _CurrentState = GameStates.Failed;
        FreezeGame ();
    }

    protected virtual void OnPuzzleReset ()
    {
        _CurrentState = GameStates.Playing;
        _IsPaused = false;
        ResumeGame ();
    }

    private void OnPuzzleComplete ()
    {
        FreezeGame ();
        SceneLoader.Instance.Load (_NextScene);
    }

    private void OnTransitionFinished ()
    {
        FreezeGame ();
        SceneLoader.Instance.Load (_NextScene);
    }

    protected virtual void OnDisable ()
    {
        GameSignals.OnLevelPaused -= OnLevelPaused;
        ExplorationSignals.OnRelicCollected -= OnRelicCollected;
        ExplorationSignals.OnRelicQuit -= OnRelicQuit;
        ExplorationSignals.OnDialogueStarted -= OnDialogueStarted;
        ExplorationSignals.OnDialogueQuit -= OnDialogueQuit;
        PuzzleSignals.OnPuzzleFailed -= OnPuzzleFailed;
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
        PuzzleSignals.OnPuzzleComplete -= OnPuzzleComplete;
        ExplorationSignals.OnTransitionFinished -= OnTransitionFinished;
    }

    protected IEnumerator SpreadTo (float length, float endSpread)
    {
        float timer = 0.0f;
        float startSpread = _Blur.blurSpread;

        while (timer <= length)
        {
            timer += Time.fixedDeltaTime;
            _Blur.blurSpread = Mathf.Lerp (startSpread, endSpread, LerpCurves.EaseIn (timer / length));

            yield return new WaitForFixedUpdate ();
        }

        _Blur.blurSpread = endSpread;
        Time.timeScale = 0.0f;
    }
}
