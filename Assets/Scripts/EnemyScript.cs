using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 3;
    public int minMoneySpawned;
    public int maxMoneySpawned;
    public List<GameObject> cashOnDeath;
    public int multiplier = 1;

    virtual public void Damage(int damage, string damageType)
    {
        health -= damage;
        if (health <= 0)
        {
            if (damageType == "Melee")
            {
                multiplier = 2;
            }
            for (int i = 0;i < Random.Range(minMoneySpawned * multiplier, maxMoneySpawned * multiplier);i++)
            {
                GameObject spawnedMoney = Instantiate(cashOnDeath[Random.Range(0, cashOnDeath.Count)], transform.position, Quaternion.identity);
                spawnedMoney.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(200, 400)));
            }
            Destroy(gameObject);
        }
    }
}
