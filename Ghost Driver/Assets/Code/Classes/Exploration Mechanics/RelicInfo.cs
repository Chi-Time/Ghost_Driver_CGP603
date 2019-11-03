using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RelicInfo
{
    public string Name;
    public string Description;

    public RelicInfo () { }

    public RelicInfo (string relicName, string relicDescription)
    {
        Name = relicName;
        Description = relicDescription;
    }

    public RelicInfo (DialogueScene relicData)
    {
        Name = relicData.Messages[0].Name;
        Description = relicData.Messages[0].Text;
    }
}
