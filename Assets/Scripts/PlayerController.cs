using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    
   /* float verticalMove;
    float horizontalMove;
    float mouseInputX;
    float mouseInputY;
    float rollInput;*/
    
    [Header("=== Ship Movement Settings ===")]
    [SerializeField] private float yawTorque = 500f;
    [SerializeField] private float pitchTorque = 1000f;
    [SerializeField] private float rollTorque = 1000f;
    [SerializeField] private float thrust = 100f;
    [SerializeField] private float upThrust = 50f;
    [SerializeField] private float strafeThrust = 50f;

    [Header("=== Ship Boost Settings ===")]
    [SerializeField] private float maxBoostAmmount=2f;
    [SerializeField] private float boostDeprecationRate = 0.25f;
    [SerializeField] private float boostRechargeRate = 0.5f;
    [SerializeField] private float boostMultiplier = 5f;
    
    public bool boosting = false;
    public float currentBoostAmmount;


    [SerializeField, Range(0.001f, 0.999f)] private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)] private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] private float leftRightGlideReduction = 0.111f;
    float glide = 0f;
    float verticalGlide = 0f;
    float horizontalGlide = 0f;
    Rigidbody rb;

    //Input Values
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private Vector2 pitchYaw;

    public GameObject laserPrefab;
    //public ParticleSystem rightEngine;
    //public ParticleSystem leftEngine;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        //rightEngine.Stop();
        //leftEngine.Stop(); 
        currentBoostAmmount = maxBoostAmmount;
    }

    void FixedUpdate()
    {

        HandleMovement();
        HandleBoosting();

      /* horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        rollInput = Input.GetAxis("Roll");


        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");*/

        
  
      /*   if((Input.GetKeyDown(KeyCode.W) ||  Input.GetKeyDown(KeyCode.UpArrow)))
        {
           leftEngine.Play();
           rightEngine.Play();
        }
         if(Input.GetKeyUp(KeyCode.W))
        {
           leftEngine.Stop();
           rightEngine.Stop();
        }
        

      if(Input.GetKeyDown(KeyCode.Space))
        {
            //Launch a projectile from the player
            Instantiate(laserPrefab, transform.position, transform.rotation);
        }

        */
    }
    
    void HandleBoosting()
    {
        if (boosting && currentBoostAmmount > 0f)
        {
            currentBoostAmmount -= boostDeprecationRate;
            if(currentBoostAmmount <= 0f)
            {
                boosting = false;
            
            }
        }
        else
        {
            if(currentBoostAmmount < maxBoostAmmount)
            {
                currentBoostAmmount += boostRechargeRate;
            }
        }
        

    }

    void HandleMovement()
    {
        //Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);
        //Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        //Yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);
        //Thrust
        if(thrust1D > 0.1f || thrust1D < -0.1f)
        {

            float currentThrust = thrust;
            if(boosting)
            {
                currentThrust = thrust * boostMultiplier;
            }
            else
            {
                currentThrust = thrust;
            }
            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            glide = thrust;

        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }
        //UpDown
        if(upDown1D > 0.1f || upDown1D < -0.1f)
        {

            
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.fixedDeltaTime);
            verticalGlide = upDown1D * upThrust;

        }
        else
        {
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.fixedDeltaTime);
            verticalGlide *= upDownGlideReduction;
        }
        //Strafing
        if(strafe1D > 0.1f || strafe1D < -0.1f)
        {

            rb.AddRelativeForce(Vector3.right * strafe1D * upThrust * Time.fixedDeltaTime);
            horizontalGlide = strafe1D * strafeThrust;

        }
        else
        {
            rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.fixedDeltaTime);
            horizontalGlide *= leftRightGlideReduction;
        }
    }

    #region Input Methods

    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }
    
    public void OnBoost(InputAction.CallbackContext context)
    {
        boosting = context.performed;
    }
    #endregion

   /* void FixedUpdate() 
    {
        playerRb.AddForce(playerRb.transform.TransformDirection(Vector3.forward) * verticalMove * speedMult, ForceMode.VelocityChange);
        playerRb.AddForce(playerRb.transform.TransformDirection(Vector3.right) * horizontalMove * speedMult, ForceMode.VelocityChange);
        playerRb.AddTorque(playerRb.transform.right * speedMultAngle * mouseInputY * -1, ForceMode.VelocityChange);
        playerRb.AddTorque(playerRb.transform.up * speedMultAngle * mouseInputX, ForceMode.VelocityChange);

        playerRb.AddTorque(playerRb.transform.forward * speedRollMultAngle * rollInput, ForceMode.VelocityChange);


    }

    //Prevent the player from going out of bounds
    /*void ConstrainPlayerPosition()
    {
        if(transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }

        if(transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
    }*/
    
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("You have collided with the enemy");
        }
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Fuel"))
        {
            Destroy(other.gameObject);
        }
        
    }

}
