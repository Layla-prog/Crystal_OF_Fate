using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public string characterName = "Default";

    public EnemyHealthUI healthUI;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        //TopHealthBarManager.Instance.ShowHealthBar(gameObject.name, currentHealth / maxHealth);

        if (healthUI != null)
        {
            healthUI.Show(currentHealth / maxHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
