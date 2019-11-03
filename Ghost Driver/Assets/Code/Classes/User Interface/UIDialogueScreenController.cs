using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIDialogueScreenController : MonoBehaviour, IWakeable
{
    [Tooltip ("The label used to display the current speaker's name.")]
    [SerializeField] private Text _NameLabel = null;
    [Tooltip ("The label used to display the current speaker's text.")]
    [SerializeField] private Text _RelicLabel = null;

    private DialogueManager _CurrentDialogueManager = null;

    public void Waken ()
    {
        ExplorationSignals.OnDialogueStarted += OnDialogueStarted;

        //Unity hack because it never calls ondestory on objects that are inactive for the whole scene.
        // It also doesn't fire off many other basic functions because it's a useless engine.
        this.gameObject.SetActive (true);
        this.gameObject.SetActive (false);
    }

    private void OnDialogueStarted (DialogueManager manager)
    {
        this.gameObject.SetActive (true);

        if (manager == null)
            return;

        _CurrentDialogueManager = manager;
        NextLine ();
    }

    public void NextLine ()
    {
        _CurrentDialogueManager.DisplayNextLine (_RelicLabel, _NameLabel, this);
    }

    public void LastLine ()
    {
        _CurrentDialogueManager.DisplayLastLine (_RelicLabel, _NameLabel, this);
    }

    public void Close ()
    {
        Disable ();
        EndDialogue ();
        ExplorationSignals.QuitDialogue ();
    }

    private void Disable ()
    {
        this.gameObject.SetActive (false);
    }

    private void EndDialogue ()
    {
        _CurrentDialogueManager.EndDialogue ();
        _CurrentDialogueManager = null;
    }

    private void OnDestroy ()
    {
        ExplorationSignals.OnDialogueStarted -= OnDialogueStarted;
    }
}
