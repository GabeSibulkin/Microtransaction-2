using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.gameObject.name);
        if(coll.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hitting player");
            coll.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
    
}
