using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float Speed = 10f;
    public int damageValue = 1;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * Speed;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<EnemyScript>())
        {
            
            collision.collider.gameObject.GetComponent<EnemyScript>().Damage(damageValue, "Projectile");
        }
        else if (collision.collider.gameObject.GetComponent<BossHurtBox>())
        {

            collision.collider.gameObject.GetComponent<BossHurtBox>().DealDamage(damageValue, "Projectile");
        }
        Destroy(gameObject);

    }
    
}
