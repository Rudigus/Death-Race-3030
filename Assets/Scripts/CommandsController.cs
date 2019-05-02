using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsController : MonoBehaviour
{

    private void Update()
    {
        ChecarComandos();
    }

    private void ChecarComandos()
    {
        // "F1" doesn't work, really. Needs to be "f1"
        if (Input.GetKeyDown("f1"))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
    }
}
