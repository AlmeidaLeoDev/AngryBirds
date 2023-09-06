using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcoAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource aSource;
    [SerializeField]
    private AudioClip[] aClips;

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!aSource.isPlaying) //Se não estiver tocando
        {
            StartCoroutine(Espera());
        }
    }

    AudioClip GetRandom()
    {
        return aClips[Random.Range(0, aClips.Length)];
    }

    IEnumerator Espera()
    {
        yield return new WaitForSeconds(1);
        aSource.Play();
        aSource.clip = GetRandom();
    }
}
