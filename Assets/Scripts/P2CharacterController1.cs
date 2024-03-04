using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player2Controller : MonoBehaviour
{
    public float speed = 3;
    public float rotationSpeed = 90;
    public float gravity = -20f;
    public float jumpSpeed = 15;
    public float raycastDistance = 5f;
    public static bool hasKilledPlayer = false;
    public static int player2Score = 0;
    public bool twoHasWon = false;

    public Transform player2;

    CharacterController characterController;
    UIManager uiManager;
    Vector3 moveVelocity;
    Vector3 turnVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        GetComponent<Renderer>().material.color = Color.green;
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    void Update()
    {
        var hInput = Input.GetAxis("Horizontal2");
        var vInput = Input.GetAxis("Vertical2");

        if (characterController.isGrounded)
        {
            moveVelocity = transform.forward * speed * vInput;
            turnVelocity = transform.up * rotationSpeed * hInput;
            if (Input.GetKeyDown(KeyCode.K))
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

        if (!hasKilledPlayer)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerController playerController = hit.collider.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        Debug.Log("Player 1 has been killed");
                        playerController.KillPlayer();
                        hasKilledPlayer = true;
                        player2Score++;
                        Debug.Log("Player 2 Score: " + player2Score);
                        uiManager.AddScore2(player2Score);
                    }
                }
            }
        }

        if ((player2Score == 5) && !twoHasWon)
        {
            Debug.Log("Player 2 wins!");
            //player2Score = 0;
            //PlayerController.player1Score = 0;
            uiManager.DisplayWinner("Player 2");
            //uiManager.AddScore2(player2Score);
            //uiManager.AddScore(PlayerController.player1Score);
            GetComponent<CharacterController>().enabled = false;
            twoHasWon = true;
        }
        if(PlayerController.player1Score == 5)
        {
            GetComponent<CharacterController>().enabled = false;
        }
    }

    // public void KillPlayer2()
    // {
    //     // Implement your logic here to kill the player
    //     Debug.Log("Player 2 has been killed");
    // }
    public Vector3 respawn2Position;

    public void KillPlayer2()
    {
        // Change the player color to red
        GetComponent<Renderer>().material.color = Color.red;

        // Disable the CharacterController
        GetComponent<CharacterController>().enabled = false;

        GameObject playerObject = GameObject.Find("Player 1");

        PlayerController playerController = playerObject.GetComponent<PlayerController>();

        if(playerController.oneHasWon == false)
        {
            StartCoroutine(RespawnP2());
        }
    }

    IEnumerator RespawnP2()
    {
        yield return new WaitForSeconds(2);

        // Generate a random x and z position
        float x = Random.Range(-25, 25);
        float z = Random.Range(-15, 9);

        // Use the current y position
        float y = transform.position.y;

        respawn2Position = new Vector3(x, y, z);

        // Teleport the player to the respawn position
        this.gameObject.transform.position = respawn2Position;

        // Change the player color to white
        GetComponent<Renderer>().material.color = Color.green;

        // Enable the CharacterController
        GetComponent<CharacterController>().enabled = true;

        // Reset hasKilledPlayer to false
        PlayerController.hasKilledPlayer2 = false;

        Debug.Log("Player 2 has respawned at " + this.gameObject.transform.position);
    }
}