using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public string characterName = "Default";

    public EnemyHealthUI healthUI;

    // Event triggered when damage is taken
    public System.Action<float> OnDamaged;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        //TopHealthBarManager.Instance.ShowHealthBar(gameObject.name, currentHealth / maxHealth);

        if (healthUI != null)
        {
            healthUI.Show(currentHealth / maxHealth);
        }

        OnDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        GoblinAI goblin = GetComponent<GoblinAI>();
        if (goblin != null)
        {
            goblin.Die(); // This drops the potion and disables the goblin AI
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
