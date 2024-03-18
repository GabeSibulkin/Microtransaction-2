using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPlayerDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<SoldierEnemyScript>().states != States.Hurt || GetComponentInParent<SoldierEnemyScript>().states != States.Dead)
        {
            if (collision.tag == "Player")
            {
                GetComponentInParent<SoldierEnemyScript>().states = States.Engage;
                GetComponentInParent<SoldierEnemyScript>().player = collision.gameObject;
            }
        }
    }
}
