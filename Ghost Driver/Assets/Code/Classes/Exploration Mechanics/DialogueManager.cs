using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class DialogueManager
{
    [Tooltip ("The current scene for this character to talk with.")]
    [SerializeField] private TextAsset _Scene = null;
    [Tooltip ("How long the delay between each character print is.")]
    [SerializeField] public float _DelayTime = 0.005f;

    /// <summary>The current index of the dialogue scene.</summary>
    private int _MessageIndex = -1;
    private Message _CurrentMessage = null;
    private DialogueScene _CurrentScene = null;
    private UIDialogueScreenController _DialogueScreenController = null;

    /// <summary>Faux constructor to setup the class with it's dependencies.</summary>
    /// <param name="dialogueScreen">The UI screen responsible for dialogue processing.</param>
    public void Constructor (UIDialogueScreenController dialogueScreen)
    {
        _DialogueScreenController = dialogueScreen;

        if (_DialogueScreenController == null)
            Debug.LogError ("Dialogue manager could not be found!!!");

        _CurrentScene = JsonUtility.FromJson<DialogueScene> (_Scene.text);

        if (_CurrentScene == null)
            Debug.LogError ("Dialogue scene could not be parsed!!!");
    }

    /// <summary>Begins dialogue by displaying dialogue manager to screen.</summary>
    public void BeginDialogue ()
    {
        _DialogueScreenController.Begin (this);

        var dialogueInfo = new DialogueInfo (_CurrentScene);
        LogBook.AddNewDialogue (dialogueInfo);
    }

    private IEnumerator DisplayMessage (Text relicLabel, Text nameLabel, Message message)
    {
        relicLabel.text = "";
        _CurrentMessage = message;
        nameLabel.text = message.Name;

        foreach (char character in message.Text)
        {
            relicLabel.text += character;

            yield return new WaitForSeconds (_DelayTime);
        }
    }

    public void DisplayNextLine (Text relicLabel, Text nameLabel, MonoBehaviour behaviour)
    {
        behaviour.StopAllCoroutines ();

        //TODO: Clean this up as it's messy as hell. We shouldn't be doing the same thing in two different if statements.

        if (_CurrentMessage == null)
        {
            var nextMessage = GetNextLine ();

            if (nextMessage == null)
            {
                relicLabel.text = _CurrentMessage.Text;
                return;
            }

            behaviour.StartCoroutine (DisplayMessage (relicLabel, nameLabel, nextMessage));
            return;
        }

        if (relicLabel.text == _CurrentMessage.Text)
        {
            var nextMessage = GetNextLine ();

            if (nextMessage == null)
            {
                relicLabel.text = _CurrentMessage.Text;
                return;
            }

            behaviour.StartCoroutine (DisplayMessage (relicLabel, nameLabel, nextMessage));
            return;
        }

        relicLabel.text = _CurrentMessage.Text;
    }

    /// <summary>Get's the next line of text in the conversation. Returns null if no line is found.</summary>
    public Message GetNextLine ()
    {
        if (_MessageIndex >= _CurrentScene.Messages.Length - 1)
            return null;

        _MessageIndex++;
        var message = _CurrentScene.Messages[_MessageIndex];

        return message;
    }

    public void DisplayLastLine (Text relicLabel, Text nameLabel, MonoBehaviour behaviour)
    {
        behaviour.StopAllCoroutines ();

        if (relicLabel.text == _CurrentMessage.Text)
        {
            var lastMessage = GetLastLine ();

            if (lastMessage == null)
            {
                relicLabel.text = _CurrentMessage.Text;
                return;
            }

            behaviour.StartCoroutine (DisplayMessage (relicLabel, nameLabel, lastMessage));
            return;
        }

        relicLabel.text = _CurrentMessage.Text;
    }

    /// <summary>Get's the last line of text in the conversation. Returns null if no line is found.</summary>
    public Message GetLastLine ()
    {
        if (_MessageIndex <= 0)
            return null;

        _MessageIndex--;
        var message = _CurrentScene.Messages[_MessageIndex];

        return message;
    }

    /// <summary>Ends the dialogue and resets the index so that it can be repeated next time.</summary>
    public void EndDialogue ()
    {
        _MessageIndex = -1;
    }
}
