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

    private DialogueManager _CurrentDialogueManager = null;
    private Message _CurrentMessage = null;
    private bool _CurrentPrinting = false;
    private FirstPersonController _Player = null;

    private void Awake ()
    {
        _Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController> ();
    }

    private void OnEnable ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _Player.enabled = false;
        NextLine ();
    }

    private IEnumerator DisplayMessage (Message message)
    {
        _RelicLabel.text = "";
        _CurrentMessage = message;
        _NameLabel.text = message.Name;

        foreach (char character in message.Text)
        {
            _RelicLabel.text += character;

            yield return new WaitForSeconds (_DelayTime);
        }
    }

    public void BeginDialogue (DialogueManager manager)
    {
        _CurrentDialogueManager = manager;

        this.gameObject.SetActive (true);
    }

    public void NextLine ()
    {
        StopAllCoroutines ();

        //TODO: Clean this up as it's messy as hell. We shouldn't be doing the same thing in two different if statements.
        if (_CurrentMessage == null)
        {
            var nextMessage = _CurrentDialogueManager.GetNextLine ();

            if (nextMessage == null)
            {
                _RelicLabel.text = _CurrentMessage.Text;
                return;
            }

            StartCoroutine (DisplayMessage (nextMessage));
            return;
        }

        if (_RelicLabel.text == _CurrentMessage.Text)
        {
            var nextMessage = _CurrentDialogueManager.GetNextLine ();

            if (nextMessage == null)
            {
                _RelicLabel.text = _CurrentMessage.Text;
                return;
            }

            StartCoroutine (DisplayMessage (nextMessage));
            return;
        }

        _RelicLabel.text = _CurrentMessage.Text;
    }

    public void LastLine ()
    {
        StopAllCoroutines ();

        if (_RelicLabel.text == _CurrentMessage.Text)
        {
            var lastMessage = _CurrentDialogueManager.GetLastLine ();

            if (lastMessage == null)
            {
                _RelicLabel.text = _CurrentMessage.Text;
                return;
            }

            StartCoroutine (DisplayMessage (lastMessage));
            return;
        }

        _RelicLabel.text = _CurrentMessage.Text;
    }

    public void Close ()
    {
        this.gameObject.SetActive (false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _Player.enabled = true;

        _CurrentDialogueManager = null;
    }
}
