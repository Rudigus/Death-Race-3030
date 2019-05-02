using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(player.GetComponent<CarInputController>().GetPickUpCollected() == "5")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
