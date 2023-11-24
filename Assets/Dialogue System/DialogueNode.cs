using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;

    public Character speaker;
    [TextArea(10,10)]public string dialogueLine;

    public override string GetString()
    {
        return "DialogueNode/" + speaker.characterName + "/" + dialogueLine + "/";
    }

    public override Sprite GetSprite()
    {
        return speaker.GetSprite();
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }

}