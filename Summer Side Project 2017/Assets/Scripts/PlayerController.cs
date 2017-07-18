using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float jumpHeight;
    public float moveSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    public GameObject bullet;
    public float bulletSpeed;
    public GameObject bulletAimer;
    public GameObject aimerDirectionPoint;
    private float aimerAngle;
    private bool aiming;

    private bool grounded;
    private bool doubleJumped;

    private Animator anim;
    private ParticleController particle;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        particle = FindObjectOfType<ParticleController>();
	}
	
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Update is called once per frame
    void Update() {

        #region JumpAndMovement

        // Jumps when jump button is pressed.
        if (Input.GetButtonDown("Jump")) Jump();

        // Moves in axis direction and stops when nothing is pressed.
        if (Input.GetAxisRaw("Horizontal") > 0) MovePlayer(1);
        else if ((Input.GetAxisRaw("Horizontal") < 0)) MovePlayer(-1);
        else MovePlayer(0);
        #endregion

        #region Aiming

        bulletAimer.transform.position = this.transform.position;
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"), 0);

        aiming = !(direction.x == 0 && direction.y == 0);

        if (aiming)
        {
            aimerAngle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            bulletAimer.transform.eulerAngles = new Vector3(0, 0, aimerAngle);
        }
        else
            bulletAimer.transform.eulerAngles = new Vector3(0, 90, 0);
        #endregion

        #region Attacks

        // For shooting and aiming the bullet attack.
        if (Input.GetButtonDown("FireBullet")) FireBullet();
        /*
        bulletAimer.transform.RotateAround(transform.position,
            new Vector3(0, 0, Input.GetAxis("VerticalRight")),
            aimerRotationSpeed * Time.deltaTime);
            */


        if (Input.GetButtonDown("FireSlash")) SwordAttack();
        #endregion

        #region Animator
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        anim.SetBool("Grounded", grounded);

        if (GetComponent<Rigidbody2D>().velocity.x > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (GetComponent<Rigidbody2D>().velocity.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        #endregion

        
    }

    #region MovementFunctions
    void Jump()
    {
        if (grounded)
        {
            doubleJumped = false;
        }
        else if (!doubleJumped)
        {
            doubleJumped = true;
        }
        else
        {
            return;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
        particle.JumpParticleRelease(this);
    }

    void MovePlayer(int direction)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed*direction, GetComponent<Rigidbody2D>().velocity.y);
    }
    #endregion

    #region Attacks
    void SwordAttack()
    {
    }

    void FireBullet()
    {
        GameObject projectile;
        projectile = Instantiate(bullet, new Vector3
            (this.transform.position.x, this.transform.position.y, this.transform.position.z),
            this.transform.rotation) as GameObject;
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());


        if (!aiming)
            projectile.GetComponent<Bullet>().Fire(bulletSpeed * transform.localScale.x);
            // The transform.localScale.x makes bullet travel backwards if facing backwards.
        else
            projectile.GetComponent<Bullet>().Fire((Vector2)projectile.transform.position, 
                (Vector2)aimerDirectionPoint.transform.position, bulletSpeed);
    }
    #endregion
}
