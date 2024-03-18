using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorDetector : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GetComponentInParent<SoldierEnemyScript>().flipMe();
        }
    }
}
