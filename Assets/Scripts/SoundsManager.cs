using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioClip playerDeathSound;
    public AudioClip wrenchCollectSound;
    public AudioClip hitSound;
    public AudioClip spikeSound;
    public AudioClip BGMMusicv1;
    public AudioClip BGMMusicv2;
    public AudioSource audioSource;
    public AudioSource engineRunningAudioSource;
    public AudioSource brakingAudioSource;
    public AudioSource BGMAudioSource;
    public static SoundsManager instance; 

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
