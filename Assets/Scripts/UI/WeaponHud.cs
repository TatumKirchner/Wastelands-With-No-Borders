using UnityEngine;

public class WeaponHud : MonoBehaviour
{
    [SerializeField] private GameObject boost;
    [SerializeField] private GameObject ketBlaster;
    [SerializeField] private GameObject ketShield;
    [SerializeField] private WeaponSwitcher WeaponSwitcher;

    public void UpdateWeaponHud()
    {
        switch (WeaponSwitcher.activeWeapon)
        {
            case 0:
                boost.SetActive(true);
                ketBlaster.SetActive(false);
                ketShield.SetActive(false);
                break;
            case 1:
                boost.SetActive(false);
                ketBlaster.SetActive(true);
                ketShield.SetActive(false);
                break;
            case 2:
                boost.SetActive(false);
                ketBlaster.SetActive(false);
                ketShield.SetActive(true);
                break;
        }
    }
}
