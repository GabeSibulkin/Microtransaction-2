using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHurtBox : MonoBehaviour
{
    public BossEnemyScript parent;
    BoxCollider2D hurtbox;

    private void Start()
    {
        hurtbox= GetComponent<BoxCollider2D>();
    }

    public void DealDamage(int damage, string AttackType)
    {
        parent.Damage(damage, AttackType);
    }
}
