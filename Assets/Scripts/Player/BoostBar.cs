using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private GameObject player;
    [SerializeField] WeaponSwitcher WeaponSwitcher;

    [SerializeField] private int maxBoost = 100;
    [SerializeField] private float boostRegenAmount;
    [SerializeField] private float m_boostAmount;
    [SerializeField] private float boostCost;
    public float currentBoost;

    private bool boosting = false;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        SetEnergy(GetBoostNormalized());
        ApplyBoost();
        BoostRegen();
    }

    public void SetMaxBoost(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
    }

    //Updates the UI with the current value
    public void SetEnergy(float energy)
    {
        slider.value = energy;
    }

    public void ApplyBoost()
    {
        //Apply boost
        if (Input.GetKey(KeyCode.LeftShift) && currentBoost >= 5f && WeaponSwitcher.weaponOne)
        {
            boosting = true;
            player.GetComponent<Rigidbody>().AddForce(m_boostAmount * Time.deltaTime * player.transform.forward, ForceMode.Impulse);
        }
        else
        {
            boosting = false;
        }
    }

    void BoostRegen()
    {
        //If boosting increment current boost down. If not boosting increment it back up.
        if (boosting)
        {
            currentBoost -= boostCost * Time.deltaTime;
            currentBoost = Mathf.Clamp(currentBoost, 0, maxBoost);
        }
        if (!boosting && currentBoost < maxBoost)
        {
            currentBoost += boostRegenAmount * Time.deltaTime;
            currentBoost = Mathf.Clamp(currentBoost, 0, maxBoost);
        }
    }

    //Returns current boost between 0 and 1.
    public float GetBoostNormalized()
    {
        return currentBoost / maxBoost;
    }
}
