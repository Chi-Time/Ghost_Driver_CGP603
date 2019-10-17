using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Signals
{
    public static event Action OnLevelTransition;
    public static void TransitionLevel () { OnLevelTransition?.Invoke (); }

    public static event Action OnTransitionFinished;
    public static void FinishTransition () { OnTransitionFinished?.Invoke (); }

    public static event Action OnPuzzleComplete;
    public static void CompletePuzzle () { OnPuzzleComplete?.Invoke (); }
}
