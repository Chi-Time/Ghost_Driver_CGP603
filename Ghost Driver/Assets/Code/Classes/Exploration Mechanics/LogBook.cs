using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class LogBook
{
    public static List<RelicInfo> Relics { get { return _Relics; } }
    public static List<DialogueInfo> Dialogues { get { return _Dialogues; } }

    private static List<RelicInfo> _Relics = new List<RelicInfo> ();
    private static List<DialogueInfo> _Dialogues = new List<DialogueInfo> ();

    public static void AddNewRelic (RelicInfo relic)
    {
        //foreach (RelicInfo relicData in _Relics)
        //    if (relicData == relic)
        //        return;

        _Relics.Add (relic);

        foreach (RelicInfo relicData in _Relics)
            Debug.Log (relicData.Name);
    }

    public static void AddNewDialogue (DialogueInfo dialogue)
    {
        // Loop through and compare every dialogue with that of the given one.
        // If we find that they are the same, don't add it.
        foreach (DialogueInfo dialogueData in _Dialogues)
            if (dialogueData.Messages[0] == dialogue.Messages[0])
                return;

        _Dialogues.Add (dialogue);
    }
}
