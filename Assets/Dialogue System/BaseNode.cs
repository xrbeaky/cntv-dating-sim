using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class BaseNode : Node {

	public virtual string GetString(){
        return null;
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }

    public virtual Emotions GetEmotion()
    {
        return Emotions.Neutral;
    }

    public virtual string GetSoundName()
    {
        return null;
    }

    public virtual Sprite GetBackground(){
        return null;
    }

    public virtual Choice[] GetChoices()
    {
        return null;
    }

    public virtual DialogueGraph GetNextGraph()
    {
        return null;
    }
}