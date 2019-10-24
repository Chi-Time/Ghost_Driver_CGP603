using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Consider creating states and just improving the Alias in general as it's fade system currently sucks.
[RequireComponent (typeof (Outline), typeof (FadeIn), typeof (Collider))]
class Alias : MonoBehaviour
{
    [SerializeField] private UIDialogueScreenController _DialogueScreen = null;
    [SerializeField] private DialogueManager _DialogueManager = new DialogueManager ();

    /// <summary> Reference to the fade in component.</summary>
    private FadeIn _FadeIn = null;
    /// <summary>Reference to the outline compnent added to the object.</summary>
    private Outline _Outline = null;
    private bool _IsHidden = false;

    private void Awake ()
    {
        _FadeIn = GetComponent<FadeIn> ();
        _Outline = GetComponent<Outline> ();
        _DialogueManager.Constructor (_DialogueScreen);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _FadeIn.Enable (true);
            _IsHidden = false;
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _FadeIn.Enable (false);
            _IsHidden = true;
        }
    }

    private void OnMouseEnter ()
    {
        if (_IsHidden == false && _Outline.enabled == false)
        {
            _Outline.enabled = true;
        }
    }

    private void OnMouseExit ()
    {
        if (_IsHidden == false && _Outline.enabled == true)
        {
            _Outline.enabled = false;
        }
    }

    private void OnMouseOver ()
    {
        // Check if the alias has hidden themselves, if so, we should override and hide the highlight.
        if (_IsHidden == true && _Outline.enabled == true)
        {
            _Outline.enabled = false;
        }

        if (Input.GetKeyDown (KeyCode.E) | Input.GetMouseButtonDown (0))
        {
            if (_IsHidden == true)
                _DialogueManager.BeginDialogue ();
        }
    }
}
