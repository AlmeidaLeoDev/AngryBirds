using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUDIOMANAGER : MonoBehaviour
{
    public static AUDIOMANAGER instance;

    public AudioClip[] clip;
    public AudioSource audioS;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioS.isPlaying)
        {
            audioS.clip = GetRandom();
            audioS.Play();
        }
    }

    AudioClip GetRandom()
    {
        return clip[Random.Range(0, clip.Length)]; 
    } 
}
