using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 2f;
    [Header("Ground")]
    [SerializeField] Transform groundCheck = null;
    [SerializeField] float groundDistance = .4f;
    [SerializeField] LayerMask groundLayer;

    CharacterController playerController;
    Vector3 movement;
    Vector3 velocity;
    float moveX;
    float moveZ;
    float runningSpeed = 2;
    bool isRunning = false;
    bool isGrounded = true;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        //--------------------------Movement------------------------
        //Basic Movement
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        
        movement = transform.right * moveX + transform.forward * moveZ;
        
        //Running 
        if(Input.GetKey(KeyCode.LeftShift) && !isRunning)
        {
            speed *= runningSpeed;
            isRunning = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && isRunning)
        {
            speed /= runningSpeed;
            isRunning = false;
        }

        playerController.Move(movement * speed * Time.unscaledDeltaTime);

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.unscaledDeltaTime;
        playerController.Move(velocity * Time.unscaledDeltaTime);
        //------------------------------------------------------------

        //Restart Level
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
