using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public int damageValue;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyScript>())
        {
            Debug.Log("HIT!");
            collision.gameObject.GetComponent<EnemyScript>().Damage(damageValue, "Melee");
        }
    }
}
