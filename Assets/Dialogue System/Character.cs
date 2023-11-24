using System.Collections;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Custom/Character")]
public class Character : ScriptableObject
{
    [SerializeField] public string characterName;

    [SerializeField] Emotions emotion = Emotions.Neutral;
    [SerializeField] Sprite[] sprites = new Sprite[4];

    public Sprite GetSprite(Emotions _emotion)
    {
        return sprites[((int)_emotion)];
    }

    public Sprite GetSprite()
    {
        return sprites[(int)emotion];
    }
}

public enum Emotions
{
    Neutral,
    Happy,
    Sad,
    Angry
}
