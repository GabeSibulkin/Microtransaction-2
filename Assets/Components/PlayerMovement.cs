using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float Move;
    public float jump;
    public bool isJumping;
    public bool isCrouching;
    public float crouchDuration = 2.0f; // Adjust this value to control how long sliding lasts
    public Transform firingPosition;
    public GameObject bulletType;
    public float RateOfFire = 0.3f;
    float currentCooldown;
    bool fireCooldown = false;

    private Rigidbody2D rb;
    public Animator animator;

    private Vector3 originalScale; // Store the original scale
    private float crouchTimer;

    // Declare a static instance variable to hold the singleton instance
    private static PlayerMovement _instance;

    // Declare a public property to access the singleton instance
    public static PlayerMovement Instance
    {
        get { return _instance; }
    }



    private void Awake()
    {
        // Check if an instance already exists
        if (_instance == null)
        {
            // If not, set the instance to this object
            _instance = this;
            // Make sure the object persists between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale; // Store the original scale at the start
        isCrouching = false; // Initialize isCrouching to false
        crouchTimer = 0f;
        currentCooldown = RateOfFire;
    }

    void Update()
    {
        if (fireCooldown == true)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                fireCooldown = false;
                currentCooldown = RateOfFire;
            }
        }
        Move = Input.GetAxis("Horizontal");

        // Check if crouching and adjust movement
        if (isCrouching)
        {
            // Update crouch timer
            crouchTimer += Time.deltaTime;

            // Check if sliding duration is over
            if (crouchTimer >= crouchDuration)
            {
                // Stop updating velocity but continue crouching animation
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
        else
        {
            // Reset crouch timer when not crouching
            crouchTimer = 0f;

            // Update velocity based on the direction
            rb.velocity = new Vector2(speed * Move, rb.velocity.y);

            // Flip the player sprite if moving in the opposite direction
            if (Move > 0)
            {
                transform.localScale = originalScale; // Use the original scale for right movement
                
            }
            else if (Move < 0)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z); // Flip for left movement
            }
        }

        // Update animation speed based on the absolute value of Move
        animator.SetFloat("Speed", Mathf.Abs(speed * Move));

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(fireCooldown);
            animator.SetBool("Firing", true);
            if (fireCooldown == false)
            {
                if (transform.localScale.x < 0)
                {
                    Instantiate(bulletType, firingPosition.position, Quaternion.Euler(0, 180, 0));
                }
                else
                {
                    Instantiate(bulletType, firingPosition.position, Quaternion.identity);
                }
                fireCooldown = true;
            }
            else
            {
                Debug.Log("On cooldown");
            }
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("Firing", false);
        }
    }

    // Rest of the code remains unchanged

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }
    }
}