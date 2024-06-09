using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Wrenches;
    public GameObject speedPowerUp;
    public GameObject waterWave;

    public Transform[] itemsSpawnPos;
    public Transform waterWaveLoopPos;

    public int wrenchCollected;
    public int startRemainingWrenches;
    // Start is called before the first frame update
    void Start()
    {
        Wrenches = GameObject.FindGameObjectsWithTag("Wrench");
        startRemainingWrenches = GameObject.FindGameObjectsWithTag("Wrench").Length;
        if (itemsSpawnPos.Length != 0)
        {
            InvokeRepeating("RandomItemSpawn", 2f, 10f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Wrenches = GameObject.FindGameObjectsWithTag("Wrench");
    }

    private void RandomItemSpawn()
    {
        int rndPos = Random.Range(0, itemsSpawnPos.Length);
        Instantiate(speedPowerUp, itemsSpawnPos[rndPos].position, Quaternion.identity).transform.SetParent(itemsSpawnPos[rndPos]);
    }
}
