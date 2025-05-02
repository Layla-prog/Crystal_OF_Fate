using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private PlayerHealthUI healthUI;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        healthUI = GetComponent<PlayerHealthUI>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //clamp to 0
        currentHealth = Mathf.Max(currentHealth, 0);

        //Update the health bar
        healthUI.ShowHealthBar(currentHealth / maxHealth);
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthUI.ShowHealthBar(currentHealth / maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TakeDamage(10f);
        }
    }
}
