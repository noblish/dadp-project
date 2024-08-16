using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonControls : MonoBehaviour
{
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float crouchSpeed;
    public float crouchHeight;
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera

    // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player

    private CharacterController characterController; // Reference to the CharacterController component
    
    public Transform holdPosition; // Position where the picked-up object will be held
    public Transform orbPosition;

    private GameObject heldObject;
    private GameObject orbGameObject;// Reference to the currently held object
    public float pickUpRange = 3f;

    private Vector3 orbScale;
    private Vector3 heldObjectScale;

    private bool crouched;
    
    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed +=
            ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled +=
            ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled
        playerInput.Player.Crouch.performed += ctx => Crouch();
        playerInput.Player.Crouch.canceled += ctx => unCrouch();

        // Subscribe to the look input events
        playerInput.Player.Look.performed +=
            ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.Look.canceled +=
            ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed
        
        // Subscribe to the pick-up input event
        playerInput.Player.Interaction.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed
    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        if (!crouched)
        {
            characterController.Move(move * (moveSpeed * Time.deltaTime));
        }
        else
        {
            characterController.Move(move * (crouchSpeed * Time.deltaTime));
        }
        
    }

    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void PickUpObject()
    {
        if (!crouched)
        {
            
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (heldObject != null)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
                heldObject.transform.parent = null;
            }
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

            if (Physics.Raycast(ray, out hit, pickUpRange) && hit.collider.gameObject.CompareTag("Interactable"))
            {
                // Pick up the object
                // Disable physics

                if (hit.collider.gameObject.name.Equals("Orb"))
                {
                    orbGameObject = hit.collider.gameObject;
                    orbGameObject.GetComponent<Rigidbody>().isKinematic = true;
                    orbGameObject.transform.position = orbPosition.position;
                    orbGameObject.transform.rotation = orbPosition.rotation;
                    orbGameObject.transform.parent = orbPosition;

                    orbScale = orbGameObject.transform.localScale;
                } 
                else{
                    heldObject = hit.collider.gameObject;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    // Attach the object to the hold position
                    heldObject.transform.position = holdPosition.position;
                    heldObject.transform.rotation = holdPosition.rotation;
                    heldObject.transform.parent = holdPosition;

                    heldObjectScale = heldObject.transform.localScale;
                }
            }
        }
    }

    public void Crouch()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,crouchHeight, 
            gameObject.transform.localScale.z);
        crouched = true;

        if (orbGameObject != null)
        {
            orbGameObject.transform.localScale = new Vector3(orbScale.x, orbScale.y * (1/crouchHeight), orbScale.z);
        }
        if (heldObject != null)
        {
            heldObject.transform.localScale = new Vector3(heldObjectScale.x, heldObjectScale.y * (1/crouchHeight), heldObjectScale.z);
        }
    }
    
    public void unCrouch()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,1, 
            gameObject.transform.localScale.z);
        crouched = false;
        if (orbGameObject != null)
        {
            orbGameObject.transform.localScale = orbScale;
        }
        if (heldObject != null)
        {
            heldObject.transform.localScale = heldObjectScale;
        }
    }
}