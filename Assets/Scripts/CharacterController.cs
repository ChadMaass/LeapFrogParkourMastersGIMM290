using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public float rotationSpeed = 90;
    public float gravity = -20f;
    public float jumpSpeed = 15;
    public float raycastDistance = 5f;
    public static bool hasKilledPlayer2 = false;
    public static int player1Score = 0;
    public bool oneHasWon = false;
    public float pushForce = 3.0f;

    public Transform player;

    CharacterController characterController;
    UIManager uiManager;
    Vector3 moveVelocity;
    Vector3 turnVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        GetComponent<Renderer>().material.color = Color.blue;
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    void Update()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        if (characterController.isGrounded)
        {
            moveVelocity = transform.forward * speed * vInput;
            turnVelocity = transform.up * rotationSpeed * hInput;
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveVelocity.y = jumpSpeed;
            }
        }
        //Adding gravity
        moveVelocity.y += gravity * Time.deltaTime;
        //characterController.Move(moveVelocity * Time.deltaTime);
        if (characterController != null && characterController.enabled)
        {
            characterController.Move(moveVelocity * Time.deltaTime);
        }
        transform.Rotate(turnVelocity * Time.deltaTime);

        if (!hasKilledPlayer2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Player2"))
                {
                    Player2Controller player2Controller = hit.collider.GetComponent<Player2Controller>();
                    if (player2Controller != null)
                    {
                        Debug.Log("Player 2 has been killed");
                        player2Controller.KillPlayer2();
                        hasKilledPlayer2 = true;
                        player1Score++;
                        Debug.Log("Player 1 Score: " + player1Score);
                        uiManager.AddScore(player1Score);
                    }
                }
            }
        }

        if ((player1Score == 5) && !oneHasWon)
        {
            Debug.Log("Player 1 wins!");
            //player1Score = 0;
            //Player2Controller.player2Score = 0;
            uiManager.DisplayWinner("Player 1");
            //uiManager.AddScore(player1Score);
            //uiManager.AddScore2(Player2Controller.player2Score);
            GetComponent<CharacterController>().enabled = false;
            oneHasWon = true;
        }
        if (Player2Controller.player2Score == 5)
        {
            GetComponent<CharacterController>().enabled = false;
        }
    }

    private ControllerColliderHit contact;
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    public Vector3 respawnPosition;

    public void KillPlayer()
    {
        // Change the player color to red
        GetComponent<Renderer>().material.color = Color.red;

        // Disable the CharacterController
        GetComponent<CharacterController>().enabled = false;

        //GameObject player2Object = GameObject.Find("Player 2");

        // Player2Controller player2Controller = player2Object.GetComponent<Player2Controller>();

        // if(player2Controller.twoHasWon == false)
        // {
        //     StartCoroutine(RespawnP1());
        // }
        // Find the GameObject in the scene
        GameObject player2Object = GameObject.Find("Player 2");

        // Check if we found it
        if (player2Object != null)
        {
            // Get the Player2Controller component
            Player2Controller player2Controller = player2Object.GetComponent<Player2Controller>();

            // Check if we found the Player2Controller
            if (player2Controller != null)
            {
                if (player2Controller.twoHasWon == false)
                {
                    StartCoroutine(RespawnP1());
                }
            }
            else
            {
                Debug.LogError("Player2Controller not found on the player object.");
            }
        }
        else
        {
            // Log an error if we didn't find the player object
            Debug.LogError("Player2 object not found in the scene.");
        }
    }

    IEnumerator RespawnP1()
    {
        yield return new WaitForSeconds(2);

        // Generate a random position between
        float x = Random.Range(-25, 25);
        float z = Random.Range(-15, 9);

        // Use the current y position
        float y = transform.position.y;

        respawnPosition = new Vector3(x, y, z);

        // Teleport the player to the respawn position
        this.gameObject.transform.position = respawnPosition;

        // Change the player color to white
        GetComponent<Renderer>().material.color = Color.blue;

        // Enable the CharacterController
        GetComponent<CharacterController>().enabled = true;

        // Reset hasKilledPlayer to false
        Player2Controller.hasKilledPlayer = false;

        Debug.Log("Player 1 has respawned at " + this.gameObject.transform.position);
    }
}