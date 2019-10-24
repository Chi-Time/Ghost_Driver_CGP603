using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
