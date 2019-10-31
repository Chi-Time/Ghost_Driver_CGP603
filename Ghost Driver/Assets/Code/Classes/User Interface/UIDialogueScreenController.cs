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

    private FirstPersonController _FPSController = null;
    private DialogueManager _CurrentDialogueManager = null;

    public void Waken ()
    {
        _FPSController = FindObjectOfType<FirstPersonController> ().GetComponent<FirstPersonController> ();
    }

    private void OnEnable ()
    {
        Setup ();
        NextLine ();
    }

    private void Setup ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _FPSController.enabled = false;
    }

    /// <summary>Begin's a dialogue scene and enables the menu.</summary>
    /// <param name="manager">The manager of the current dialogue scene.</param>
    public void Begin (DialogueManager manager)
    {
        if (manager == null)
            return;

        _CurrentDialogueManager = manager;

        this.gameObject.SetActive (true);
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
    }

    private void Disable ()
    {
        this.gameObject.SetActive (false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _FPSController.enabled = true;
    }

    private void EndDialogue ()
    {
        _CurrentDialogueManager.EndDialogue ();
        _CurrentDialogueManager = null;
    }
}
