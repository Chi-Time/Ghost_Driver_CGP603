using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
class DialogueManager
{
    [Tooltip ("The current scene for this character to talk with.")]
    [SerializeField] private TextAsset _Scene = null;

    private int _Index = -1;
    private DialogueScene _CurrentScene = null;
    private UIDialogueScreenController _DialogueScreenController = null;

    public void Constructor (UIDialogueScreenController dialogueScreen)
    {
        _DialogueScreenController = dialogueScreen;

        if (_DialogueScreenController == null)
            Debug.LogError ("Dialogue manager could not be found!!!");

        _CurrentScene = JsonUtility.FromJson<DialogueScene> (_Scene.text);

        if (_CurrentScene == null)
            Debug.LogError ("Dialogue scene could not be parsed!!!");

        Debug.Log (_CurrentScene);
        Debug.Log (_CurrentScene.Messages);
    }

    public void BeginDialogue ()
    {
        _DialogueScreenController.BeginDialogue (this);
    }

    public Message GetLastLine ()
    {
        if (_Index <= 0)
            return null;

        _Index--;
        var message = _CurrentScene.Messages[_Index];

        return message;
    }

    public Message GetNextLine ()
    {
        if (_Index >= _CurrentScene.Messages.Count - 1)
            return null;

        _Index++;
        var message = _CurrentScene.Messages[_Index];

        return message;
    }
}

[System.Serializable]
public class DialogueScene
{
    public List<Message> Messages;
}

[System.Serializable]
public class Message
{
    public string Name = "";
    public string Text = "";
}
