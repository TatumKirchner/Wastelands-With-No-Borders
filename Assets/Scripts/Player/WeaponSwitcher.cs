using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [HideInInspector] public int activeWeapon = 0;
    private float m_scrollWheel;

    [HideInInspector] public bool weaponOne;
    [HideInInspector] public bool weaponTwo;
    [HideInInspector] public bool weaponThree;

    private WeaponHud weaponHud;

    // Start is called before the first frame update
    void Start()
    {
        weaponHud = FindObjectOfType<WeaponHud>();
        SwitchWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    //When the player rolls the mouse wheel or presses an alpha key switch weapons.
    void SwitchWeapons()
    {
        if (activeWeapon > 2) //must match the number of cases
        {
            activeWeapon = 0; //leave this number - allows the rotation to start over
        }
        if (activeWeapon < 0) //leave this number - allows the rotation to start over
        {
            activeWeapon = 2; //must match the number of cases
        }

        switch (activeWeapon)
        {
            case 0:
                weaponOne = true;
                weaponTwo = false;
                weaponThree = false;
                break;
            case 1:
                weaponOne = false;
                weaponTwo = true;
                weaponThree = false;
                break;
            case 2:
                weaponOne = false;
                weaponTwo = false;
                weaponThree = true;
                break;
        }
    }

    void GetInputs()
    {
        m_scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        //Increase active weapon
        if (m_scrollWheel > 0f)
        {
            activeWeapon++;
            SwitchWeapons();
            weaponHud.UpdateWeaponHud();
        }

        //Decreases active weapon
        if (m_scrollWheel < 0f)
        {
            activeWeapon--;
            SwitchWeapons();
            weaponHud.UpdateWeaponHud();
        }

        //Directly switches the weapon with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeWeapon = 0;
            SwitchWeapons();
            weaponHud.UpdateWeaponHud();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeWeapon = 1;
            SwitchWeapons();
            weaponHud.UpdateWeaponHud();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeWeapon = 2;
            SwitchWeapons();
            weaponHud.UpdateWeaponHud();
        }
    }
}
