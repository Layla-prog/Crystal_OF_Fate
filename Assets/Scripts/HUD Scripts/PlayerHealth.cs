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
        if (isDead) return;

        currentHealth -= damage;

        //clamp to 0
        currentHealth = Mathf.Max(currentHealth, 0);

        //Update the health bar
        healthUI.ShowHealthBar(currentHealth / maxHealth);

        OnDamaged?.Invoke(currentHealth);

        /*currentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ". Current health: " + currentHealth);

        currentHealth = Mathf.Max(0, currentHealth);

        //TopHealthBarManager.Instance.ShowHealthBar(gameObject.name, currentHealth / maxHealth);

        if (healthUI != null)
        {
            healthUI.ShowHealthBar(currentHealth / maxHealth);
        }*/

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

        /*GameOverManager gameOver = FindObjectOfType<GameOverManager>();
        if (gameOver != null)
        {
            gameOver.ShowGameOver();
        }*/

        // Game over scene
        GameOverManager.Instance?.ShowGameOver();
        //Invoke(nameof(TriggerGameOver), 1.5f);

        // Disable player components
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 2f);
    }

    /*private void TriggerGameOver()
    {
        GameOverManager.Instance?.ShowGameOver();
    }*/

    public void RestoreHealth(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthUI?.ShowHealthBar(currentHealth / maxHealth);
    }
}
