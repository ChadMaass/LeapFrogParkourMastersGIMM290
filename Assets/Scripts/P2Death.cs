using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player2Controller character2 = other.GetComponent<Player2Controller>();
        if (character2 != null)
        {
            character2.KillPlayer2();
        }
    }
}
