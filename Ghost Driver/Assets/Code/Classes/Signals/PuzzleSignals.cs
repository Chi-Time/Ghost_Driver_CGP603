using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class PuzzleSignals
{
    public static event Action OnPuzzleComplete;
    public static void CompletePuzzle () { OnPuzzleComplete?.Invoke (); }

    public static event Action OnPuzzleReset;
    public static void ResetPuzzle () { OnPuzzleReset?.Invoke (); }

    public static event Action OnPuzzleFailed;
    public static void FailPuzzle () { OnPuzzleFailed?.Invoke (); }

    public static event Action OnPlayerSighted;
    public static void SightPlayer () { OnPlayerSighted?.Invoke (); }

    public static event Action<Vector3> OnMovementPressed;
    public static void MovementPressed (Vector3 direction) { OnMovementPressed?.Invoke (direction); }
}
