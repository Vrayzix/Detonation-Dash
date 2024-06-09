using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaves : MonoBehaviour
{
    private float speed = 5f;
    private LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if(transform.position.x >= levelManager.waterWaveLoopPos.position.x)
        {
            transform.position = new Vector2(-levelManager.waterWaveLoopPos.position.x, levelManager.waterWaveLoopPos.position.y);
        }
    }
}
