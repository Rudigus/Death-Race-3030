using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Transform bar;
    // The maximum health in the game (%)
    public float maxHealth;
    private float actualHealth;
    private float healthLevel;

    private void Start()
    {
        SetHealth(1.0f);
        actualHealth = maxHealth;
    }

    public void SetHealth(float healthNormalized)
    {
        if (healthNormalized > 1.0f)
        {
            bar.localScale = Vector2.one;
            actualHealth = maxHealth;
        }
        else if (healthNormalized > 0.0f)
        {
            bar.localScale = new Vector2(healthNormalized, 1.0f);
            actualHealth = GetHealth() * maxHealth;
        }
        else
        {
            bar.localScale = Vector2.up;
            actualHealth = 0;
            GameObject.FindWithTag("Player").GetComponent<CarInputController>().enabled = false;
            // Need to put death mechanisms later, like a death screen
        }
    }

    public void IncreaseHealth(float healthNormalized)
    {
        float updatedHealthLevel = GetHealth() + healthNormalized;
        SetHealth(updatedHealthLevel);
    }

    public void DecreaseHealth(float healthNormalized)
    {
        float updatedHealthLevel = GetHealth() - healthNormalized;
        SetHealth(updatedHealthLevel);
    }

    public float GetHealth()
    {
        healthLevel = bar.localScale.x;
        return healthLevel;
    }

    public float HealthNormalize(float healthUnnormalized)
    {
        float healthNormalized = healthUnnormalized / 100.0f;
        return healthNormalized;
    }
}
