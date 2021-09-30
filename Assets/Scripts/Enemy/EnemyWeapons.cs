using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    private Vector3 dir;
    [SerializeField] private float range = 20f;
    [SerializeField] private float force;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPosition;
    private bool fired = false;

    private void Update()
    {
        EnemyGun();
    }

    void EnemyGun()
    {
        //Raycast in the forward vector.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range))
        {
            //If the raycast hits the player get the direction of the hit and start the shoot coroutine.
            if (hit.collider.CompareTag("Player") && !fired)
            {
                dir = (hit.point - transform.position).normalized;
                StartCoroutine(Shoot());
                fired = true;
            }
        }
    }

    //Spawn in a bullet and add force to it's rigidbody to move towards the player.
    //Then wait for the fire rate yield to allow the enemy to shoot again.
    IEnumerator Shoot()
    {
        GameObject bulletClone = Instantiate(bulletPrefab, gunPosition.transform.position, gunPosition.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().AddForce(100 * force * Time.deltaTime * dir);
        yield return new WaitForSeconds(fireRate);
        fired = false;
    }
}
