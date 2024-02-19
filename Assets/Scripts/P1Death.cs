using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController character = other.GetComponent<PlayerController>();
        if (character != null)
        {
            character.KillPlayer();
        }
    }
}

