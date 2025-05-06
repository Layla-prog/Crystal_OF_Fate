using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public EnemyHealthUI healthUI;

    public Canvas worldSpaceCanvas;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (healthUI != null)
        {
            healthUI.Show(currentHealth / maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
       // if (currentHealth <= 0f) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0, maxHealth);

        //Debug.Log($"{gameObject.name} took {damage} damage. Current: {currentHealth}");

        if (healthUI != null)
        {
            healthUI.Show(currentHealth / maxHealth);
        }

        if (animator != null)
        {
            animator.SetTrigger("Hit-B");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DealDamage(GameObject target)
    {
        if (target == null) return;

        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(15); 
        }
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        Debug.Log($"{gameObject.name} healed by {amount}. Current: {currentHealth}");

        healthUI?.Show(currentHealth / maxHealth);
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");

        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
