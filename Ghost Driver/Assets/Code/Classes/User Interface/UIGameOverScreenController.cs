using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UIGameOverScreenController : MonoBehaviour, IWakeable
{
    public void Waken ()
    {
        PuzzleSignals.OnPuzzleFailed += OnPuzzleFailed;
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;

        //Unity hack because it never calls ondestory on objects that are inactive for the whole scene.
        // It also doesn't fire off many other basic functions because it's a useless engine.
        this.gameObject.SetActive (true);
        this.gameObject.SetActive (false);
    }

    private void OnPuzzleFailed ()
    {
        this.gameObject.SetActive (true);
    }

    private void OnPuzzleReset ()
    {
        this.gameObject.SetActive (false);
    }

    public void Restart ()
    {
        PuzzleSignals.ResetPuzzle ();
    }

    public void BackToMenu ()
    {
        //TODO: Find a way to get the game controller to handle returning to the menu logic.
        SceneLoader.Instance.Load ("SC_Main_Menu");
    }

    private void OnDestroy ()
    {
        PuzzleSignals.OnPuzzleFailed -= OnPuzzleFailed;
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
    }
}
