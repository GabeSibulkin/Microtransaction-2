using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.U2D;
using UnityEngine;


public enum MechStates { Inactive, Active, Aggro, Hurt, Dead }
public class MechEnemyScript : EnemyScript
{

    public MechStates states;
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
    public bool standingStill;
    Animator anim;
    public Rigidbody2D rb;
    public float distanceToPlayer;
    public float maxDistanceToPlayer = 2f;
    bool firing, isHurt;
    public float TimeBetweenShots;
    public bool startsActive;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (startsActive)
        {
            states = MechStates.Active;
        }
        else
        {
            states = MechStates.Inactive;
        }
        startOfState = true;
        stateTimer = Random.Range(MinStateTime, MaxStateTime);
        currentStateTime = stateTimer;
        FireCurrentCooldown = FireCooldown;
        firing = false;
        isHurt = false;
        standingStill = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt)
        {
            switch (states)
            {
                case MechStates.Inactive:
                    break;
                case MechStates.Active:
                    if (startOfState)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            flipMe();
                        }
                        startOfState = false;
                    }
                    anim.SetFloat("MechMoving", Mathf.Abs(rb.velocity.x));
                    if (transform.localScale.x > 0)
                    {
                        rb.velocity = new Vector2(-20, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(20, rb.velocity.y);
                    }
                    if (currentStateTime <= 0)
                    {
                        currentStateTime = Random.Range(MinStateTime, MaxStateTime);
                        states = MechStates.Inactive;
                        startOfState = true;
                    }
                    break;
                case MechStates.Aggro:
                    if (startOfState)
                    {
                        
                        startOfState = false;
                    }
                    if (!standingStill)
                    {
                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(-30f, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(30f, rb.velocity.y);
                        }
                    }
                    anim.SetFloat("MechMoving", Mathf.Abs(rb.velocity.x));
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
                case MechStates.Dead:
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
            if (health <= 0)
            {
                states = MechStates.Dead;
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
        states = MechStates.Hurt;
        invincible = true;
        //anim.SetBool("Damaged", true);
        yield return new WaitForSeconds(0.4f);
        //anim.SetBool("Damaged", false);
        invincible = false;
        isHurt = false;
        player = GameObject.FindGameObjectWithTag("Player");
        states = MechStates.Aggro;
    }

    public IEnumerator firingCoroutine()
    {
        firing = true;
        Debug.Log("Starting to Fire");
        for (int i = 0; i < shotsFiredMax; i++)
        {
            if (transform.localScale.x > 0)
            {
                Instantiate(projectile, projectileFirePoint.position, Quaternion.Euler(0, 0, -140));
            }
            else
            {
                Instantiate(projectile, projectileFirePoint.position, Quaternion.Euler(0, -180, -140));
            }
            yield return new WaitForSeconds(TimeBetweenShots);
        }
        FireCurrentCooldown -= Time.deltaTime;
        Debug.Log("Firing stopped");
        firing = false;
    }
    
    public IEnumerator deathCoroutine(string damageType)
    {
        states = MechStates.Dead;
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
        states = MechStates.Aggro;
        startOfState = true;
    }
    public void flipMe()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
