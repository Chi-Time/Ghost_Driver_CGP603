using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ExplorationSignals
{
    public static event Action OnLevelTransition;
    public static void TransitionLevel () { OnLevelTransition?.Invoke (); }

    public static event Action OnTransitionFinished;
    public static void FinishTransition () { OnTransitionFinished?.Invoke (); }
}
