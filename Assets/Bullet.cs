using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isFacingLeft = true;

    public Transform firePoint;
    public GameObject bullet;

    void Start()
    {
    }

    private void Update()
    {
        // If user left click..
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }    
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
