using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class AudioNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;

   [SerializeField] string soundName = "";

    public override string GetString()
    {
        return "AudioNode/";
    }

    public override string GetSoundName()
    {
        return soundName;
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }
}