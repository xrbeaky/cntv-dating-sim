using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class FadeNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;

    public override string GetString()
    {
        return "FadeNode/";
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }

}