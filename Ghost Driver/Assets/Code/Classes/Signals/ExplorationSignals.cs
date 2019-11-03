using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ExplorationSignals
{
    public static event Action<float> OnLevelTransition;
    public static void TransitionLevel (float length) { OnLevelTransition?.Invoke (length); }

    public static event Action OnTransitionFinished;
    public static void FinishTransition () { OnTransitionFinished?.Invoke (); }
}
