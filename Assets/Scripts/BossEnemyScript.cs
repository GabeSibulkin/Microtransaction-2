using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossStates
{
    Idle, HighPunch, LowPunch, Laser, Summon, Dead
}
public class BossEnemyScript : EnemyScript
{
    public BossStates BossState;
    public bool inCoroutine;
    Animator animator;
    public GameObject[] unitsToSpawn;
    public Sprite[] damagedSprites;
    public SpriteRenderer head;
    int MaxHealth;
    int chosenAttack, previousAttack;
    public bool isDead;
    public GameObject spawnLocation;
    int timeInIdle;

    private void Start()
    {
        inCoroutine = false;
        BossState = BossStates.Idle;
        MaxHealth = health;
        isDead = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!inCoroutine && !isDead)
        {
            switch (BossState)
            {
                case BossStates.Idle:
                    StartCoroutine(idleCoroutine());
                    break;
                case BossStates.HighPunch:
                    StartCoroutine(punchCoroutine(true));
                    break;
                case BossStates.LowPunch:
                    StartCoroutine(punchCoroutine(false));
                    break;
                case BossStates.Summon:
                    StartCoroutine(summonCoroutine());
                    break;
                case BossStates.Laser:
                    StartCoroutine(laserCoroutine());
                    break;
                default:
                    break;
            }
        }
        
    }

    public IEnumerator idleCoroutine()
    {
        inCoroutine = true;   
        yield return 0;
    }

    public IEnumerator punchCoroutine(bool highPunch)
    {
        
        
        if (highPunch)
        {
            animator.SetBool("HighPunch", true);
        }
        else
        {
            animator.SetBool("LowPunch", true);
        }
        yield return 0;
    }

    public IEnumerator laserCoroutine()
    {
        animator.SetBool("Laser", true) ;
        yield return 0;
    }
    public IEnumerator summonCoroutine()
    {
        animator.SetBool("Summon", true);
        yield return 0;
    }

    public IEnumerator deathCoroutine(string damageType)
    {
        inCoroutine = true;
        isDead = true;
        animator.SetBool("Laser", false);
        animator.SetBool("HighPunch", false);
        animator.SetBool("LowPunch", false);
        animator.SetBool("Summon", false);
        animator.SetBool("Dead", true);
        BossState = BossStates.Dead;
        if (damageType == "Melee")
        {
            multiplier = 2;
        }
        yield return new WaitForSeconds(3f);
        Die();
    }

    public void SummonEnemy()
    {
        //int enemyToSummon = Random.Range(0, unitsToSpawn.Length);
        //Instantiate(unitsToSpawn[enemyToSummon], spawnLocation.transform.position, Quaternion.identity);
    }

    public override void Damage(int damage, string damageType)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(deathCoroutine(damageType));
        }
    }

    public void endAnimation(string animationName)
    {
        Debug.Log("Ending animation");
        inCoroutine = false;
        animator.SetBool(animationName, false);
        timeInIdle = Random.Range(2, 4);
        BossState = BossStates.Idle;
    }

    public void startAnimation(string animationName)
    {
        inCoroutine = true;
        if (animationName != "Idle")
        {
            Debug.Log("Starting coroutine for " + animationName);
            animator.SetBool(animationName, true);
        }
    }

    public void endIdle()
    {
        Debug.Log(timeInIdle);
        if (timeInIdle <= 0)
        {
            inCoroutine = false;
            chosenAttack = Random.Range(1, 4);
            Debug.Log(chosenAttack);
            switch (chosenAttack)
            {
                case 1:
                    BossState
                        = BossStates.HighPunch;
                    break;
                case 2:
                    BossState
                        = BossStates.LowPunch;
                    break;
                case 3:
                    BossState
                        = BossStates.Laser;
                    
                    break;
                default:
                    break;
            }
        }
        else
        {
            timeInIdle--;
        }
    }

    public void Die()
    {

        Destroy(gameObject);

        for (int i = 0; i < Random.Range(minMoneySpawned * multiplier, maxMoneySpawned * multiplier); i++)
        {
            GameObject spawnedMoney = Instantiate(cashOnDeath[Random.Range(0, cashOnDeath.Count)], transform.position, Quaternion.identity);
            spawnedMoney.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(200, 400)));
        }
    }
}
