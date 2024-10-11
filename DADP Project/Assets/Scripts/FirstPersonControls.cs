using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    public Transform holdPosition1; // Position where the picked-up object will be held
    public Transform holdPosition2;

    private GameObject heldObject1;
    private GameObject heldObject2;
    
    public float pickUpRange = 3f;

    private Vector3 orbScale;
    private Vector3 heldObjectScale;

    private bool crouched;

    public GameObject[] objectsToChangeColor;
    public Material switchMaterial;

    public bool swimState = true;

    public Transform lastCheckpoint;
    private Vector3 checkpointDistance;
    
    public TextMeshProUGUI pickUpText;

    public PauseMenu PauseMenu;

    public GameObject[] orbArr = new GameObject[4];
    
    
    
    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();

        for (int i = 0; i < orbArr.Length; i++)
        {
            orbArr[i].SetActive(false);
        }
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
        // playerInput.Player.Crouch.performed += ctx => Crouch();
        // playerInput.Player.Crouch.canceled += ctx => unCrouch();

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
        if (!PauseMenu.PausedGame)
        {
            LookAround();
            CheckForPickUp();
        }
        
        if (!swimState)
        {
            Move();
            ApplyGravity();
            //Debug.Log("NOT SWIMMING");
        }

        if (swimState)
        {
            //Debug.Log("SWIMMING - " + playerCamera.localEulerAngles.x);
            SwimMovement();
        }
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

            if (heldObject1 != null && heldObject2 != null)
            {
                heldObject1.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
                heldObject1.transform.parent = null;
                heldObject1 = null;
            }
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);
            
            if (Physics.Raycast(ray, out hit, pickUpRange) && hit.collider.gameObject.CompareTag("Interactable"))
            {
                // Pick up the object
                // Disable physics

                if (hit.collider.gameObject.name.Contains("Orb"))
                {
                    Debug.Log("ORB!!!");
                    switch (hit.collider.gameObject.name)
                    {
                        case "Egg Orb":
                            orbArr[0].SetActive(true);
                            hit.collider.gameObject.SetActive(false);
                            break;
                        case "Mushroom Orb":
                            orbArr[1].SetActive(true);
                            hit.collider.gameObject.SetActive(false);
                            break;
                        case "Hand Orb":
                            orbArr[2].SetActive(true);
                            hit.collider.gameObject.SetActive(false);
                            break;
                        case "Geode Orb":
                            orbArr[3].SetActive(true);
                            hit.collider.gameObject.SetActive(false);
                            break;
                    }
                }
                else
                {
                    if (heldObject1 == null || heldObject2 == null)
                    {
                        if (heldObject1 == null)
                        {
                            heldObject1 = hit.collider.gameObject;
                            hit.collider.gameObject.transform.parent = holdPosition1;
                            heldObject1.GetComponent<Rigidbody>().isKinematic = true;
                            heldObject1.transform.position = holdPosition1.position;
                            if (hit.collider.gameObject.name == "Tablet")
                            {
                                heldObject1.transform.position += new Vector3(-0.35f, -0.45f, 1.85f);
                            }
                        }
                        else
                        {
                            heldObject2 = hit.collider.gameObject;
                            hit.collider.gameObject.transform.parent = holdPosition2;
                            heldObject2.GetComponent<Rigidbody>().isKinematic = true;
                            heldObject2.transform.position = holdPosition2.position;
                            if (hit.collider.gameObject.name == "Tablet")
                            {
                                heldObject1.transform.position += new Vector3(-0.35f, -0.45f, 1.85f);
                            }
                        }
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit, pickUpRange) && hit.collider.CompareTag("Door")) // Check if the object is a door
            {
                // Start moving the door upwards
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }
        }
        else
        {
            
        }
    }

    /*public void Crouch()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, crouchHeight,
            gameObject.transform.localScale.z);
        crouched = true;

        if (heldObject1 != null || heldObject2 != null)
        {
            heldObject1.transform.localScale = new Vector3(heldObjectScale.x, heldObjectScale.y * (1 / crouchHeight), heldObjectScale.z);
            heldObject2.transform.localScale = new Vector3(heldObjectScale.x, heldObjectScale.y * (1 / crouchHeight), heldObjectScale.z);
        }
    }

    public void unCrouch()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1,
            gameObject.transform.localScale.z);
        crouched = false;
        
        if (heldObject1 != null || heldObject2 != null)
        {
            heldObject1.transform.localScale = heldObjectScale;
            heldObject2.transform.localScale = heldObjectScale;
        }
    }*/

    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
            {
                // Change the material color of the objects in the array
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; //Set the color to match the switch material color
                    }
                }
            }

        }
    }
    private IEnumerator RaiseDoor(GameObject door)
    {
        float raiseAmount = 5f; // The total distance the door will be raised
        float raiseSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising
                                                                        // Continue raising the door until it reaches the target height
        while (door.transform.position.y < endPosition.y)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position =
                Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed *
                                                                          Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }
    }
    

    public void SwimMovement()
    {
        Vector3 move = new Vector3();
        if(AngleToSpectrum(playerCamera.localEulerAngles.x) <= 0)
        {
            
            move = new Vector3(moveInput.x,  (AngleToSpectrum(playerCamera.localEulerAngles.x)/2) + 0.2f, moveInput.y);
        }
        else
        {
            move = new Vector3(moveInput.x,  (AngleToSpectrum(playerCamera.localEulerAngles.x)/6) + 0.2f, moveInput.y);
        }
        move = transform.TransformDirection(move);
        characterController.Move(move * (moveSpeed * Time.deltaTime));
    }

    private float AngleToSpectrum(float angleInput)
    {
        float result = (angleInput - 180) / 90;
        return result;
    }
    
    private void CheckForPickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
// Perform raycast to detect objects
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
// Check if the object has the "PickUp" tag
            if (hit.collider.CompareTag("Interactable"))
            {
// Display the pick-up text
                pickUpText.gameObject.SetActive(true);
                pickUpText.text = hit.collider.gameObject.name + "\nPickup with 'E'";
            }
            else
            {
// Hide the pick-up text if not looking at a "PickUp" object
                pickUpText.gameObject.SetActive(false);
            }
        }
        else
        {
// Hide the text if not looking at any object
            pickUpText.gameObject.SetActive(false);
        }
    }
}