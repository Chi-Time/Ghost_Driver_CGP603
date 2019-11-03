using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Alias currently enables and disables highlight while talking, this should be frozen so you need to find a way to track whether it's currently talking or not.
//TODO: Consider creating states and just improving the Alias in general as it's fade system currently sucks.
[RequireComponent (typeof (Outline), typeof (FadeIn), typeof (Collider))]
class Alias : MonoBehaviour
{
    enum AliasStates { Hidden, Shown, Talking }

    [Tooltip ("The current state of the alias.")]
    [SerializeField] private AliasStates _CurrentState = AliasStates.Hidden;
    [Tooltip ("The dialogue manager for this specific alias and it's dialogue scene file.")]
    [SerializeField] private DialogueManager _DialogueManager = new DialogueManager ();

    /// <summary> Reference to the fade in component.</summary>
    private FadeIn _FadeIn = null;
    /// <summary>Reference to the outline compnent added to the object.</summary>
    private Outline _Outline = null;

    private void Awake ()
    {
        _FadeIn = GetComponent<FadeIn> ();
        _Outline = GetComponent<Outline> ();

        var dialogueScreen = gameObject.FindFirstObjectOfType<UIDialogueScreenController> ();

        if (dialogueScreen == null)
            Debug.LogError ("Dialogue screen not found!!, Please ensure there is one in the scene");

        _DialogueManager.Constructor ();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _FadeIn.Enable (true);
            _CurrentState = AliasStates.Shown;
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _FadeIn.Enable (false);
            _CurrentState = AliasStates.Hidden;
        }
    }

    private void OnMouseEnter ()
    {
        if (_CurrentState == AliasStates.Shown && _Outline.enabled == false)
        {
            _Outline.enabled = true;
        }
    }

    private void OnMouseExit ()
    {
        if (_CurrentState == AliasStates.Shown && _Outline.enabled == true)
        {
            _Outline.enabled = false;
        }
    }

    private void OnMouseOver ()
    {
        // Check if the alias has hidden themselves, if so, we should override and hide the highlight.
        if (_CurrentState == AliasStates.Hidden && _Outline.enabled == true)
        {
            _Outline.enabled = false;
        }

        if (Input.GetKeyUp (KeyCode.E) | Input.GetMouseButtonUp (0))
        {
            if (_CurrentState == AliasStates.Shown)
            {
                _DialogueManager.BeginDialogue ();
            }
        }
    }
}
