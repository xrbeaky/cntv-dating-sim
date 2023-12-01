using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitNode : BaseNode
{
   [Input] public int entry;

   [SerializeField] DialogueGraph nextGraph;

    public override string GetString()
    {
        return "Exit";
    }

    public override DialogueGraph GetNextGraph()
    {
        return nextGraph;
    }
}
