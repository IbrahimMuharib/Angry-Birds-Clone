using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour

{
    public static SoundManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    // Update is called once per frame
    public void PlayClip(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();

    }

    public void PlayRandomClip(AudioClip[] clips, AudioSource source)
    {
        int randomIndex = Random.Range(0, clips.Length);

        source.clip = clips[randomIndex];
        source.Play();

    }
}
