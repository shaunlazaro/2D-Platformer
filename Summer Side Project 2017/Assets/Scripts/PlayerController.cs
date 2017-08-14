using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float jumpHeight;
    public float moveSpeed;
    private float dashSpeedMultiplier = 1.5f; // How much moveSpeed is multiplied by during the dash.

    // Used to check if the player is on the ground.
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    // Used for shooting+aiming.
    public GameObject bullet;
    public float bulletSpeed;
    public GameObject bulletAimer;
    public GameObject aimerDirectionPoint;
    private float aimerAngle;
    public float shootingCooldownLength; // In Seconds.
    private float shootingCooldownTime;
    private int shootingCooldownLeft;

    private bool aiming;
    private bool dashing;
    
    private bool grounded;
    private int airJumpsLeft;
    private int airDashesLeft;

    private Animator anim;
    private ParticleController particle;

    private PlayerStatsManager save;

    private LevelUIController ui;

	void Start () {
        // Initiate the animator.
        anim = GetComponent<Animator>();

        // Initiate the particle effect controller.
        particle = FindObjectOfType<ParticleController>();

        // Initiate the Save Manager.
        save = FindObjectOfType<PlayerStatsManager>();

        // Initiate the UI Controller.
        ui = FindObjectOfType<LevelUIController>();

    }

    // FixedUpdate is called on a different interval than Update; it's used for physics.
    void FixedUpdate()
    {
        // Checks if the player is grounded by looking at a circle collider under the player.
        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer))
        {
            grounded = true;
            airJumpsLeft = save.MaxJumps;
            airDashesLeft = save.MaxDashes;
        }
        else
        {
            grounded = false;
        }
    }

    // Update is called once per frame
    void Update() {

        // Features:
        // - Makes the aimer visible when right stick is in use.
        // - Rotates the aimer to point in same direction as stick.
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

        // Features:
        // - Moves based on input
        // - Jumps when jump button is pressed
        // - Dashes when dash button is pressed
        #region JumpAndMovementInput

        // Jumps when jump button is pressed.
        if (Input.GetButtonDown("Jump")) Jump();
        // Dashes when dash is pressed, tells dash method if aiming or not.
        if (Input.GetButtonDown("Dash")) Dash();

        // Moves in axis direction and stops when nothing is pressed.
        else if (Input.GetAxisRaw("Horizontal") > 0 && !dashing) MovePlayer(1);
        else if (Input.GetAxisRaw("Horizontal") < 0 && !dashing) MovePlayer(-1);
        else if (!dashing) MovePlayer(0);

        #endregion

        // Features:
        // - Shooting
        #region Attacks
        // For shooting and aiming the bullet attack.
        if (Input.GetButtonDown("FireBullet")) FireBullet();
        #endregion

        // Prob have to redo this when I add animations.
        #region Animator Code

        // Sets speed (for movement animation)
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        
        // Tells animator if guy is on ground.
        anim.SetBool("Grounded", grounded);

        // Flips player if he is moving backwards (helpful for shooting).
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (GetComponent<Rigidbody2D>().velocity.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        #endregion

        #region UI

        if (ui.AirDashNum != airDashesLeft) ui.UpdateDashText(airDashesLeft);
        if (ui.AirJumpNum != airJumpsLeft) ui.UpdateJumpText(airJumpsLeft);

        shootingCooldownLeft = Mathf.CeilToInt(shootingCooldownTime - Time.time);
        if (shootingCooldownLeft < 0) shootingCooldownLeft = 0;
        if (ui.ShotCooldownNum != shootingCooldownLeft) ui.UpdateShotCooldownText(shootingCooldownLeft);

        #endregion
    }

    // Stores the functions triggered by "JumpAndMovement"
    #region MovementFunctions

    // Jumping while not grounded reduces "jumps left"
    void Jump()
    {
        if (grounded)
        {
            // Do nothing.
        }
        else if (airJumpsLeft > 0 && !grounded)
        {
            airJumpsLeft--;
        }
        else
        {
            return;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
        particle.JumpParticleRelease(this);
    }

    // Move player forward/back depending on direction passed.
    // Takes inputs of -1, 0, and 1 usually.
    void MovePlayer(int direction)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed*direction, GetComponent<Rigidbody2D>().velocity.y);
    }

    // More like a backstep, dashes away from aimer or backwards.
    void Dash()
    {
        if (!dashing)
        {
            // Handles # Of Air Dashes Allowed.
            if(!grounded)
            {
                if (airDashesLeft < 1) return;
                else airDashesLeft--;
            }

            // Dashes
            dashing = true;
            StartCoroutine(HandleDash());
        }
    }

    IEnumerator HandleDash(float dashTime = 0.5f)
    {
        if (!grounded) airJumpsLeft++;
        if (!dashing) yield break; // If dash was cancelled already, don't redo the function when the dash would naturally end.

        // Set the and remove gravity before waiting the full dash duration.
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * dashSpeedMultiplier * transform.localScale.x, 0);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(dashTime); // Lets you control how long dash lasts.

        // Stop all of the velocity then wait, suspending the player in the air for a fifth of the dashTime.
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(dashTime / 5); // If stoptime is 0.5, becomes 0.1 secs.  If 0, then no wait.

        // After being suspended, the player regains control and gravity returns.
        GetComponent<Rigidbody2D>().gravityScale = 3;
        dashing = false;

        // Release particles to make exiting the dash more impactful.
        particle.DashParticleRelease(this.transform.position);
    }
    #endregion

    // Stores the shooting functions.
    #region Attacks

    void FireBullet()
    {
        if (shootingCooldownTime > Time.time) return;
        shootingCooldownTime = Time.time + shootingCooldownLength;

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
    public void ResetShootingCooldown()
    {
        shootingCooldownTime = Time.time;
    }
    #endregion
}
