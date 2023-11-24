using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioSource introSource, loopSource;
    // Start is called before the first frame update
    void Start()
    {
        introSource.Play();
        Debug.Log("This is Intro");
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
        Debug.Log("This is loop");       
    }

    
}
