using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    public float gravityScale;
    public float bounceForce = 11;

    private Vector3 moveDirection;
    private Camera theCam;
    public GameObject playerModel;
    public float rotateSpeed;

    public Animator anim;

    public bool levelEnded = false;
    public bool isKnocking;
    private bool hasBeenKnocked = false;
    public float knockBackLength = .6f;
    private float knockbackCounter;
    public Vector2 knockbackPower;

    public GameObject[] playerPieces;

    private const int MAX_JUMP = 2;
    private int jumpCounter;

    private GameObject lastwallhit;
    private GameObject currentwall;

    //DEBUG Purposes
    public GameObject wallJumpEffect;
    public Text wallJumpText;

    public float slopeForce;
    public float slopeForceRayLength;
    

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        wallJumpText.enabled = false;
        controller = GetComponent<CharacterController>(); //Getting the CharacterController on parent object
        theCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isKnocking && !levelEnded)
        {
            //Initialize moveDirection, a vector which will control which way, and how fast the character is moving.
            float yStore = moveDirection.y; //store gravity
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal")); //work out movement
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1) * moveSpeed; //same speed in diagonal direction
            moveDirection.y = yStore; //restore gravity after normalization

            if (controller.isGrounded) //if player is grounded
            {
                anim.SetBool("IsJumping", false);
                hasBeenKnocked = false; //reset hasbeenknocked to false if hits the ground
                moveDirection.y = 0f; //y axis movement is 0
                jumpCounter = 0;

                if (Input.GetButtonDown("Jump")) //if player presses jump button
                {
                    anim.SetBool("IsJumping", true);
                    AudioManager.instance.PlaySFX(9);
                    moveDirection.y = jumpForce; //apply jumpforce to our y position
                    jumpCounter++;
                }
            } else if (!controller.isGrounded && jumpCounter < MAX_JUMP)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    anim.SetBool("IsJumping", true);
                    AudioManager.instance.PlaySFX(9);
                    moveDirection.y = jumpForce + 4;
                    jumpCounter++;
                }
            }

            if(!controller.isGrounded || controller.isGrounded)
            {
                wallJumpText.enabled = false;
            }


            moveDirection.y = moveDirection.y + ((Physics.gravity.y) * gravityScale * Time.deltaTime); //applying gravity
            controller.Move(moveDirection * Time.deltaTime); //applying movedirection to the character


            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                //transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f); //make sure we face whatever direction the camera is pointing
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }

        if(levelEnded)
        {
            moveDirection = Vector3.zero;
            controller.Move(moveDirection);
            playerModel.transform.rotation = GameManager.instance.rotation;
        }

        if(isKnocking)
        {
            wallJumpText.enabled = false;
            knockbackCounter -= Time.deltaTime;

   
            float yStore = moveDirection.y; 
            moveDirection = playerModel.transform.forward * -knockbackPower.x;
            moveDirection.y = yStore;

            if (controller.isGrounded) //if player is grounded
            {
                moveDirection.y = 0f; //y axis movement is 0
            }


            moveDirection.y = moveDirection.y + ((Physics.gravity.y) * gravityScale * Time.deltaTime); //applying gravity

            controller.Move(moveDirection * Time.deltaTime);
           
            if (knockbackCounter <= 0)
            {
                isKnocking = false;
            }
        }


        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", controller.isGrounded);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) // Walljump Code
    {
        if (!controller.isGrounded && hit.normal.y < 0.1f) // If Player is in the air and is against a jumpable wall
        {
            Debug.Log("Hit Wall!");
            currentwall = hit.gameObject;

            if(lastwallhit != currentwall) //if last wall hit isnt the current wall hit
            {
                wallJumpText.enabled = true;
            }

            if (Input.GetButtonDown("Jump") && lastwallhit != currentwall && !hasBeenKnocked)  //if player presses jump, last wall isnt current wall and player hasnt been knocked
            {
                anim.SetBool("IsJumping", true);
                AudioManager.instance.PlaySFX(10);

                moveDirection.y = jumpForce + 5;

                Instantiate(wallJumpEffect, transform.position, transform.rotation);
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                jumpCounter--;
                lastwallhit = currentwall;
            } else if (hasBeenKnocked) //if player has been knocked
            {
                wallJumpText.enabled = false; //disable walljump text as they can no longer walljump
            }
        } 
        else
        {
            currentwall = null;
            lastwallhit = null;
        }
    }

    public void knockBack()
    {
        hasBeenKnocked = true;
        isKnocking = true;
        knockbackCounter = knockBackLength;
        Debug.Log("Knocked Back");
        moveDirection.y = knockbackPower.y;
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
