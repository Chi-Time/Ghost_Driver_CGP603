using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIDialogueScreenController : MonoBehaviour
{
    [Tooltip ("The label used to display the current speaker's name.")]
    [SerializeField] private Text _NameLabel = null;
    [Tooltip ("The label used to display the current speaker's text.")]
    [SerializeField] private Text _RelicLabel = null;
    [Tooltip ("How long the delay between each character print is.")]
    [SerializeField] public float _DelayTime = 0.025f;

    private bool _IsSetup = false;
    private Message _CurrentMessage = null;
    private FirstPersonController _Player = null;
    private DialogueManager _CurrentDialogueManager = null;

    private void OnDisable ()
    {
        print ("Disabled!");

        if (_IsSetup == false)
        {
            _IsSetup = true;
            this.gameObject.SetActive (true);
            this.gameObject.SetActive (false);
        }
    }

    private void Awake ()
    {
        _Player = FindObjectOfType<FirstPersonController> ().GetComponent<FirstPersonController> ();
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
        _Player.enabled = false;
    }

    //private IEnumerator DisplayMessage (Message message)
    //{
    //    _RelicLabel.text = "";
    //    _CurrentMessage = message;
    //    _NameLabel.text = message.Name;

    //    foreach (char character in message.Text)
    //    {
    //        _RelicLabel.text += character;

    //        yield return new WaitForSeconds (_DelayTime);
    //    }
    //}

    public void Begin (DialogueManager manager)
    {
        _CurrentDialogueManager = manager;

        this.gameObject.SetActive (true);
    }

    public void NextLine ()
    {
        _CurrentDialogueManager.DisplayNextLine (_RelicLabel, _NameLabel, this);
        //StopAllCoroutines ();

        ////TODO: Clean this up as it's messy as hell. We shouldn't be doing the same thing in two different if statements.
        //if (_CurrentMessage == null)
        //{
        //    var nextMessage = _CurrentDialogueManager.GetNextLine ();

        //    if (nextMessage == null)
        //    {
        //        _RelicLabel.text = _CurrentMessage.Text;
        //        return;
        //    }

        //    StartCoroutine (DisplayMessage (nextMessage));
        //    return;
        //}

        //if (_RelicLabel.text == _CurrentMessage.Text)
        //{
        //    var nextMessage = _CurrentDialogueManager.GetNextLine ();

        //    if (nextMessage == null)
        //    {
        //        _RelicLabel.text = _CurrentMessage.Text;
        //        return;
        //    }

        //    StartCoroutine (DisplayMessage (nextMessage));
        //    return;
        //}

        //_RelicLabel.text = _CurrentMessage.Text;
    }

    public void LastLine ()
    {
        _CurrentDialogueManager.DisplayLastLine (_RelicLabel, _NameLabel, this);
        //StopAllCoroutines ();

        //if (_RelicLabel.text == _CurrentMessage.Text)
        //{
        //    var lastMessage = _CurrentDialogueManager.GetLastLine ();

        //    if (lastMessage == null)
        //    {
        //        _RelicLabel.text = _CurrentMessage.Text;
        //        return;
        //    }

        //    StartCoroutine (DisplayMessage (lastMessage));
        //    return;
        //}

        //_RelicLabel.text = _CurrentMessage.Text;
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
        _Player.enabled = true;
    }

    private void EndDialogue ()
    {
        _CurrentDialogueManager.EndDialogue ();
        _CurrentDialogueManager = null;
    }
}
