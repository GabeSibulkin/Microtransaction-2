using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickUp : MonoBehaviour
{
    public MoneyTracker tracker;
    public int value;

    public void Start()
    {
        tracker = GameObject.Find("GameManager").GetComponent<MoneyTracker>();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            
            tracker.AddMoney(value);
            Destroy(gameObject);
        }
    }
}
