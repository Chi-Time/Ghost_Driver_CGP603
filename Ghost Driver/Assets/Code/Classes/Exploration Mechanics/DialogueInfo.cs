using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DialogueInfo
{
    public string Name;
    public Message[] Messages;

    public DialogueInfo () { }

    public DialogueInfo (DialogueScene dialogueData)
    {
        Name = dialogueData.Messages[0].Name;
        Messages = dialogueData.Messages;
    }
}
