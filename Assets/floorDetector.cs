using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorDetector : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (GetComponentInParent<SoldierEnemyScript>())
            {
                GetComponentInParent<SoldierEnemyScript>().flipMe();
            }
            else if (GetComponentInParent<MechEnemyScript>())
            {
                GetComponentInParent<MechEnemyScript>().flipMe();
            }    
        }
    }
}
