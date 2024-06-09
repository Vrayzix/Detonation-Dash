using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public Image energyBar;
    public float energyAmount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        energyBar.fillAmount = energyAmount;
    }
    public void RechargeEnergy(float rechargeAmount)
    {
        energyAmount += rechargeAmount * Time.fixedDeltaTime / 100f;
    }
    public void ConsumeEnergy(float consumeAmount)
    {
        energyAmount -= consumeAmount * Time.fixedDeltaTime / 100f;
    }
}
