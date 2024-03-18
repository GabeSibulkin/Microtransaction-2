using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.U2D;
using UnityEngine;

public enum States { Idle, Engage, Dead, IdleWalk, Hurt}
public class SoldierEnemyScript : EnemyScript
{

    
    public States states;
    public GameObject projectile;
    public GameObject player;
    public Transform projectileFirePoint;
    public float MinStateTime;
    public float MaxStateTime;
    public int shotsFiredMax;
    public float FireCooldown;
    float FireCurrentCooldown;
    float stateTimer;
    float currentStateTime;
    bool startOfState, invincible;
    Animator anim;
    public Rigidbody2D rb;
    public float distanceToPlayer;
    public float maxDistanceToPlayer = 2f;
    bool firing, isHurt;
    public float TimeBetweenShots;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        states = States.Idle;
        startOfState = true;
        stateTimer = Random.Range(MinStateTime, MaxStateTime);
        currentStateTime = stateTimer;
        FireCurrentCooldown = FireCooldown;
        firing = false;
        isHurt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt)
        {
            switch (states)
            {
                case States.Idle:
                    rb.velocity = new Vector2(0, 0);
                    if (startOfState) startOfState = false;
                    if (currentStateTime <= 0)
                    {
                        states = States.IdleWalk;
                        currentStateTime = Random.Range(MinStateTime, MaxStateTime);
                        startOfState = true;
                    }
                    break;
                case States.Engage:
                    if (startOfState)
                    {
                        startOfState = false;
                    }
                    distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
                    if (distanceToPlayer > maxDistanceToPlayer)
                    {
                        Debug.Log(distanceToPlayer);
                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(-10f, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(10f, rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                    if ((player.transform.position.x > gameObject.transform.position.x && transform.localScale.x > 0) || (player.transform.position.x < gameObject.transform.position.x && transform.localScale.x < 0))
                    {
                        rb.velocity = Vector2.zero;
                        flipMe();
                    }
                    if (FireCurrentCooldown == FireCooldown && !firing)
                    {
                        StartCoroutine(firingCoroutine());
                    }
                    if (!firing)
                    {
                        FireCurrentCooldown -= Time.deltaTime;
                    }
                    if (FireCurrentCooldown <= 0)
                    {
                        FireCurrentCooldown = FireCooldown;
                    }
                    break;
                case States.Dead:
                    break;
                case States.IdleWalk:
                    if (startOfState)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            flipMe();
                        }
                        startOfState = false;
                    }
                    if (transform.localScale.x > 0)
                    {
                        rb.velocity = new Vector2(-5, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(5, rb.velocity.y);
                    }
                    if (currentStateTime <= 0)
                    {
                        currentStateTime = Random.Range(MinStateTime, MaxStateTime);
                        states = States.Idle;
                        startOfState = true;
                    }
                    break;
                case States.Hurt:
                    break;
                default:
                    break;
            }
        }
        currentStateTime -= Time.deltaTime;
    }
    public override void Damage(int damage, string damageType)
    {
        isHurt = true;
        if (!invincible)
        {
            StopAllCoroutines();
            if (firing)
            {
                firing = false;
            }
            health -= damage;
            if (health <= 0)
            {
                states = States.Dead;
                StartCoroutine(deathCoroutine(damageType));
            }
            else
            {
                StartCoroutine(hurtCoroutine());
            }
        }
    }
    public IEnumerator hurtCoroutine()
    {
        states= States.Hurt;
        invincible = true;
        //anim.SetBool("Damaged", true);
        yield return new WaitForSeconds(0.4f);
        //anim.SetBool("Damaged", false);
        invincible = false;
        isHurt = false;
        player = GameObject.FindGameObjectWithTag("Player");
        states = States.Engage;
    }

    public IEnumerator firingCoroutine()
    {
        firing = true;
        for (int i = 0; i < shotsFiredMax; i++)
        {
            if (transform.localScale.x > 0)
            {
                Instantiate(projectile, projectileFirePoint.position, Quaternion.Euler(0, 180, 0));
            }
            else
            {
                Instantiate(projectile, projectileFirePoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(TimeBetweenShots);
        }
        FireCurrentCooldown -= Time.deltaTime;
        firing = false;
    }

    public IEnumerator deathCoroutine(string damageType)
    {
        states = States.Dead;
        invincible = true;
        //anim.SetTrigger("Death");
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(3f);
        if (damageType == "Melee")
        {
            multiplier = 2;
        }
        for (int i = 0; i < Random.Range(minMoneySpawned * multiplier, maxMoneySpawned * multiplier); i++)
        {
            GameObject spawnedMoney = Instantiate(cashOnDeath[Random.Range(0, cashOnDeath.Count)], transform.position, Quaternion.identity);
            spawnedMoney.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(200, 400)));
        }
        Destroy(gameObject);
    }
    public void playerFound(GameObject detectedPlayer)
    {
        player = detectedPlayer;
        states = States.Engage;
        startOfState = true;
    }
    public void flipMe()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
