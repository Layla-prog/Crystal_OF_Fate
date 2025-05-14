using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public string characterName = "Default";

    public PlayerHealthUI healthUI;

    public bool isDead = false;

    // Event triggered when damage is taken
    public System.Action<float> OnDamaged;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

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
        if (isDead) return;

        currentHealth -= damage;

        //clamp to 0
        currentHealth = Mathf.Max(currentHealth, 0);

        //Update the health bar
        healthUI.ShowHealthBar(currentHealth / maxHealth);

        OnDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0f && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log($"{gameObject.name} died.");

        // Game over scene
        GameOverManager.Instance?.ShowGameOver();

        // Disable player components
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 2f);
    }

    public void RestoreHealth(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthUI?.ShowHealthBar(currentHealth / maxHealth);
    }
}
