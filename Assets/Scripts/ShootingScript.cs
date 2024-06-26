using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{

    public GameObject bullet;
    public Transform bulletPos;

    private float timer;
    public float bulletspeed = 10f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>2)
        {
            timer = 0;
            shoot();
           
        }
    }

    void shoot()
    {
        if (transform.localScale.x > 0)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = (-(bulletPos.right)) * bulletspeed;
        }
    }
}
