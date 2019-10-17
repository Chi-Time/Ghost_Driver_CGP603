using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (Outline), typeof (Collider))]
class Alias : MonoBehaviour
{
    public bool IsFadeFinished { get; set; }

    [Tooltip ("How long the object should take to fade in.")]
    [SerializeField] private float _FadeLength = 2.0f;
    [SerializeField] private UIDialogueScreenController _DialogueScreen = null;

    /// <summary>Reference to the outline compnent added to the object.</summary>
    private Outline _Outline = null;
    private FadeIn _FadeIn = null;

    private void Awake ()
    {
        _Outline = GetComponent<Outline> ();
        _FadeIn = GetComponent<FadeIn> ();
        //GetComponent<Collider> ().isTrigger = true;
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _FadeIn.Enable (true);
            IsFadeFinished = true;
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _FadeIn.Enable (false);
            IsFadeFinished = false;
        }
    }

    private void OnMouseEnter ()
    {
        //_Outline.enabled = true;
    }

    private void OnMouseExit ()
    {
        //_Outline.enabled = false;
    }

    private void OnMouseOver ()
    {
        if (Input.GetKeyDown (KeyCode.E) | Input.GetMouseButtonDown (0))
        {
            if (IsFadeFinished == true)
                BeginDialogue ();
        }
    }

    private void BeginDialogue ()
    {
        //TODO: Implement relic collection logic here.
        _DialogueScreen.gameObject.SetActive (true);
    }
}
