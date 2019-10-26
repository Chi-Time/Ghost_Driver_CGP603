using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
