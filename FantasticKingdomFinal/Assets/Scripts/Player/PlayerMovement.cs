using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //Mouse control character look
    public float horizontalSensitivity = 250f;

    //Movement stats
    public float speed = 6f;
    public float gravity = 3f;
    public float jumpVelocity = 6f;
    public float rollVelocity = 150f;
    public float attackWalkSpeed = 4;
    public float defaultSpeed = 6f;
    public float sprintSpeed = 9f;

	// Cooldown fields
	public Image CDBar;
	private float currentCD = 0;
	private float maxCD = 1;
	private float LIGHT_ATK_CD = 35;
	private float HEAVY_ATK_CD = 60;
    float ROLL_CD = 50;
    float JUMP_CD = 50;
    float jumpCD = 0;
    bool attacking = false;
    bool jumping = false;

    //Character look
    public float rotationSpeed = 12f;
    float HorizontalDirection = 0f;
    float previousDirection = 0f;
    bool dirLocked = true;

    //
    Vector3 movement;
    float distanceToGround;

    //Components
    Rigidbody playerRigidBody;
    Animator anim;
    Collider playerCollider;
    NavMeshAgent agent;
    PlayerSwordAttack playerSword;
    ThirdPersonCamera cam;

    //Audio Components
    AudioSource playerSound;
    public AudioClip[] attackNoises;
    private AudioClip selectedSound;
    

    

    enum Action {JUMP, ROLL, LIGHT_ATTACK, HEAVY_ATTACK };

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.AddForce(Physics.gravity * gravity);
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        cam = GetComponentInChildren<ThirdPersonCamera>();
        distanceToGround = playerCollider.bounds.extents.y;
        playerSword = GetComponentInChildren<PlayerSwordAttack>();

        //Character look fields
        Vector3 angles = transform.eulerAngles;
        HorizontalDirection = angles.y;

        //Audio
        playerSound = GetComponent<AudioSource>();

    }

    //----------------------------------------------------------------------------||
    // Update Functions
    //----------------------------------------------------------------------------||


    //Called every physics step
	void FixedUpdate()
    {
        float forward = Input.GetAxisRaw("Vertical");
        float strafe = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey("left shift"))
        {
            speed = sprintSpeed;
            anim.SetFloat("RunSpeed", 2);
        }
        else if (attacking)
        {
            speed = attackWalkSpeed;
            anim.SetFloat("RunSpeed", 0.8f);
        }
        else
        {
            speed = defaultSpeed;
            anim.SetFloat("RunSpeed", 1);
        }
        //if(!attacking)
            Move(forward, strafe, cam.transform.forward, cam.transform.right);
        Animating(forward, strafe);
     
    }

    //called every frame
    void Update()
    {

        
        if (jumpCD > 0)
        {
            jumpCD -= 1;
        }
        else if(jumpCD == 0 && IsGrounded())
        {
            jumping = false;
            if(agent)
                agent.enabled = true;
        }
        
        if (Input.GetButtonDown("Jump") && IsGrounded() && !jumping)
        {
            jumping = true;
            jumpCD = JUMP_CD;
            if (agent)
                agent.enabled = false;
            playerRigidBody.velocity = new Vector3(0, jumpVelocity, 0);
        }
		if (Input.GetKeyDown(KeyCode.LeftControl) && IsGrounded())
        {
            if(currentCD == 0)
                Roll();
        }
        Attack (HorizontalDirection);
    }

    //Resets status of
    void Reset()
    {
        if (!jumping)
        {

        }
    }


    //----------------------------------------------------------------------------||
    // Action Functions
    //----------------------------------------------------------------------------||

    /** Checks for cooldown. If no cooldown and an attack button is being pressed then perform the attack */
    void Attack(float dir){
		// Cooldown
		if(currentCD > 0){
			float CDRatio = 50 * currentCD / maxCD;
            if (CDBar != null)
            {
                CDBar.rectTransform.sizeDelta = new Vector2(50, CDRatio);
                //CDBar.rectTransform.localPosition = new Vector3(0, 25 - CDRatio / 2, 0);
                CDBar.rectTransform.localPosition = new Vector3(-26, 51 - CDRatio / 2, 0);
            }
			currentCD--;
			return;
		}
        if (CDBar != null)
            CDBar.rectTransform.sizeDelta = new Vector2 (0, 50); // Clean up
        if (currentCD == 0)
        {
            dirLocked = true;
            attacking = false;
            if (playerSword)
            {
                playerSword.FinishAttack();
            }
        }

        // Attack
        if (Input.GetMouseButtonDown(0) || Input.GetAxis("JoyAttack") < -0.5)
		{   
            selectedSound = attackNoises[0];
            playerSound.PlayOneShot(selectedSound);
            previousDirection = HorizontalDirection;
            dirLocked = false;
            attacking = true;
            ActionAnim(Action.LIGHT_ATTACK);
			maxCD = LIGHT_ATK_CD;
			currentCD = LIGHT_ATK_CD;
            if (playerSword)
            {
                playerSword.QuickAttack();
            }
        }
		if(Input.GetMouseButtonDown(1) || Input.GetAxis("JoyAttack") > 0.5)
		{   
 
            dirLocked = false;
            attacking = true;
            ActionAnim (Action.HEAVY_ATTACK);
			maxCD = HEAVY_ATK_CD;
			currentCD = HEAVY_ATK_CD;
            if (playerSword)
            {
                playerSword.PowerAttack();
            }

            selectedSound = attackNoises[1];
            playerSound.PlayOneShot(selectedSound);

        }
	}

    //control player movement
    void Move(float forward, float right, Vector3 fCam, Vector3 rCam)
    {
        //TODO fix vertical dependancy, and flying
        //movement = (transform.forward * forward) + (transform.right * right);
        fCam.y = 0;
        fCam.Normalize();
        movement = (fCam * forward) + (rCam * right);
        Vector3 rotDir = new Vector3(movement.x, 0, movement.z);

        if (rotDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(rotDir),
                Time.deltaTime * rotationSpeed
            );
        }
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidBody.MovePosition(transform.position + movement);
    }

    //player carry out jump action
    void Jump()
    {
        dirLocked = false;
        attacking = true;
        ActionAnim(Action.JUMP);
        playerRigidBody.AddForce(new Vector3(0, jumpVelocity, 0), ForceMode.VelocityChange);
    }

    //Player carry out roll action
    void Roll()
    {
        Vector3 direction;

        direction = transform.forward * speed * Time.deltaTime;
        dirLocked = false;
        attacking = true;
        maxCD = ROLL_CD;
        currentCD = ROLL_CD;
        ActionAnim(Action.ROLL);
        playerRigidBody.AddForce(direction * rollVelocity, ForceMode.VelocityChange);

    }

    //Checks that player is on the ground (within error)
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.005f);
    }


    //----------------------------------------------------------------------------||
    // Animations
    //----------------------------------------------------------------------------||

    //Sets animation controller states
    void Animating(float f, float s)
    {
        bool walking = f != 0f || s != 0f;
        anim.SetBool("IsWalking", walking);
    }

	void ActionAnim(Action action)
	{
		if(action == Action.HEAVY_ATTACK)
		{
			anim.SetTrigger ("HeavyAttack");
		}
		else if(action == Action.LIGHT_ATTACK)
		{
			anim.SetTrigger ("LightAttack");
		}
        else if (action == Action.ROLL)
        {
            anim.SetTrigger("Roll");
        }
			
	}


}
