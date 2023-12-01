using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class BlankNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;

    public Sprite background;

    public override string GetString()
    {
        return "BlankNode/";
    }

    public override Sprite GetBackground()
    {
        return background;
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }

}