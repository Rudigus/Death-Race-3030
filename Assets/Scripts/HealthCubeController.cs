using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCubeController : MonoBehaviour
{
    private Renderer renderer;
    private GameObject enemy;
    private float currentHealth;
    private float maxHealth;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        enemy = transform.parent.gameObject;
    }

    
    void Update()
    {
        renderer.material.color = GetCurrentColor();
    }

    private Color32 GetCurrentColor()
    {
        if (enemy != null)
        {
            currentHealth = enemy.GetComponent<EnemyInputController>().GetCurrentHealth();
            maxHealth = enemy.GetComponent<EnemyInputController>().GetMaxHealth();
        }
        Vector3 colorValues;
        if (currentHealth > (maxHealth * 3) / 4)
        {
            colorValues = Vector3.Lerp(new Vector3(255, 255, 0), new Vector3(0, 255, 0),
                ((currentHealth - (maxHealth * 3) / 4)) / (maxHealth / 4));
        }
        else if (currentHealth > (maxHealth * 2) / 4)
        {
            colorValues = Vector3.Lerp(new Vector3(255, 0, 0), new Vector3(255, 255, 0),
                ((currentHealth - (maxHealth * 2) / 4)) / (maxHealth / 4));
        }
        else
        {
            colorValues = Vector3.Lerp(new Vector3(255, 0, 0), new Vector3(0, 0, 0),
                (((maxHealth * 2) / 4 - currentHealth)) / ((maxHealth * 2) / 4));
        }
        Color32 currentColor = new Color32((byte)colorValues.x, (byte)colorValues.y, 
            (byte)colorValues.z, 255);
        return currentColor;
    }
}
