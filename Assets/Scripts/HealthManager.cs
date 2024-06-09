using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = playerHealth;
    }
    public void HealPlayer(float healAmount)
    {
        playerHealth += healAmount / 100f;
    }
    public void DamagePlayer(float damageAmount)
    {
        playerHealth -= damageAmount * Time.deltaTime/100f;
    }
}
