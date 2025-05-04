using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KateCombat : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Get the animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //check for stab input - R
        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerStab();
        }
    }

    void TriggerStab()
    {
        //trigger the stab animation
        animator.SetTrigger("IsStabbing");

        //stab range
        float stabRange = 1.5f;
        float stabRadius = 1f;
        Vector3 stabOrigin = transform.position + transform.forward * stabRange * 0.5f;

        //find all colliders in the stab range
        Collider[] hitEnemies = Physics.OverlapSphere(stabOrigin, stabRadius);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(15);
                }
            }
        }
    }
}
