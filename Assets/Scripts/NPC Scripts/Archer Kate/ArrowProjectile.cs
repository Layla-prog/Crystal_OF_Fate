using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 5f;

    private Vector3 direction;

    public void Initialize(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime); // optional cleanup
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            EnemyHealthUI ui = other.GetComponentInChildren<EnemyHealthUI>();

            if (eh != null)
            {
                eh.TakeDamage(damage);
                Debug.Log("Arrow hit enemy: " + other.name);
            }

            if (ui != null)
            {
                float healthPercent = eh.currentHealth / eh.maxHealth;
                ui.Show(healthPercent); 
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Ally") && !other.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy arrow if it hits wall etc.
        }
    }
}
