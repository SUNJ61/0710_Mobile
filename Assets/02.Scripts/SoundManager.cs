using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource source;
    public AudioClip hitClip;
    void Start()
    {
        source = GetComponent<AudioSource>();
        Instance = this;
    }

    void Update()
    {
        
    }

    public void HitAsteroid()
    {
        source.PlayOneShot(hitClip, 1.0f);
    }
}
