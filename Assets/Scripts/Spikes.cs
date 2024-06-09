using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameObject[] spikes;
    private SoundsManager soundsManager;
    // Start is called before the first frame update
    void Start()
    {
        soundsManager = GameObject.Find("Sounds Manager").GetComponent<SoundsManager>();
        StartCoroutine(RunSpike());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RunSpike()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            for(int i = 0; i< spikes.Length; i++)
            {
                spikes[i].SetActive(false);
            }
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < spikes.Length; i++)
            {
                spikes[i].SetActive(true);
            }
            soundsManager.audioSource.PlayOneShot(soundsManager.spikeSound, 0.2f);
        }
    }
}
