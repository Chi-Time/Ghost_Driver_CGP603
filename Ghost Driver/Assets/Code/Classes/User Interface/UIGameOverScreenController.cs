using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UIGameOverScreenController : MonoBehaviour
{
    private SceneLoader _SceneLoader = null;

    private void Awake ()
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
