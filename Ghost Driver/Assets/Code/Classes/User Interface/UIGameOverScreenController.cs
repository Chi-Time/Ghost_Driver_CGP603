using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UIGameOverScreenController : MonoBehaviour, IWakeable
{
    private SceneLoader _SceneLoader = null;

    public void Waken ()
    {
        var loaders = gameObject.FindAllObjectsOfType<SceneLoader> ();
        _SceneLoader = loaders[0];
    }

    public void Restart ()
    {
        PuzzleSignals.ResetPuzzle ();
    }

    public void BackToMenu ()
    {
        _SceneLoader.Scene = "SC_Main_Menu";
        PuzzleSignals.CompletePuzzle ();
    }
}
