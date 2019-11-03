using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ExplorationSignals
{
    public static event Action<RelicInfo> OnRelicCollected;
    public static void CollectRelic (RelicInfo relic) { OnRelicCollected?.Invoke (relic); }

    public static event Action OnRelicQuit;
    public static void QuitRelic () { OnRelicQuit?.Invoke (); }

    public static event Action<DialogueManager> OnDialogueStarted;
    public static void StartDialogue (DialogueManager dialogueManager) { OnDialogueStarted?.Invoke (dialogueManager); }

    public static event Action OnDialogueQuit;
    public static void QuitDialogue () { OnDialogueQuit?.Invoke (); }

    public static event Action<float> OnLevelTransition;
    public static void TransitionLevel (float length) { OnLevelTransition?.Invoke (length); }

    public static event Action OnTransitionFinished;
    public static void FinishTransition () { OnTransitionFinished?.Invoke (); }
}
