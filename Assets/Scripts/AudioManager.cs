using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] private Sound[] sounds;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    void Awake(){
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.volume = s.volume;
            s.source.clip = s.clip;
            s.source.loop = s.isLooping;
        }
    }

    public void PlaySound(string name){
        foreach(Sound s in sounds){
            if(s.name == name){
                s.source.Play();
            }
        }
    }
}

[System.Serializable]
public class Sound{
    public string name = "";
    public AudioClip clip;
    [HideInInspector]public AudioSource source;
    public float volume = 1.0f;
    public bool isLooping = false;
}