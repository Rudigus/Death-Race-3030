using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectText : MonoBehaviour
{
    private GameObject player;
    private TextMeshProUGUI collectibleText;

    private void Start()
    {
        collectibleText = gameObject.GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player");
        collectibleText.text = "Colecionáveis (0/5)";
    }

    private void Update()
    {
        SetCollectiblesText();
    }

    private void SetCollectiblesText()
    {
        collectibleText.text = "Colecionáveis (" +
            player.GetComponent<CarInputController>().GetPickUpCollected() + "/5)";
    }
}
