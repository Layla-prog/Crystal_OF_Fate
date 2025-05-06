using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public PlayerHealthUI healthUI;

    // Event triggered when damage is taken
    public System.Action<float> OnDamaged;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        //healthUI = GetComponent<PlayerHealthUI>();

        if (healthUI == null)
        {
            healthUI = GetComponentInChildren<PlayerHealthUI>();

            if (healthUI == null)
            {
                Debug.LogWarning("PlayerHealthUI not assigned and not found in children!");
            }
        }

        healthUI?.ShowHealthBar(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //clamp to 0
        currentHealth = Mathf.Max(currentHealth, 0);

        //Update the health bar
        healthUI.ShowHealthBar(currentHealth / maxHealth);

        OnDamaged?.Invoke(currentHealth);
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthUI?.ShowHealthBar(currentHealth / maxHealth);
    }
}
