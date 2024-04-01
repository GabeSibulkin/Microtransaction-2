using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPlayerDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<SoldierEnemyScript>())
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
        else if(GetComponentInParent<MechEnemyScript>())
        {
            if (GetComponentInParent<MechEnemyScript>())
            {
                if (GetComponentInParent<MechEnemyScript>().states != MechStates.Hurt || GetComponentInParent<MechEnemyScript>().states != MechStates.Dead)
                {
                    if (collision.tag == "Player")
                    {
                        GetComponentInParent<MechEnemyScript>().states = MechStates.Aggro;
                        GetComponentInParent<MechEnemyScript>().player = collision.gameObject;
                    }
                }
            }
        }
    }
}
