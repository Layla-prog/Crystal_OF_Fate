using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public Animator animator;
    public float healAmount = 25f;
    public float healThreshold = 70f;
    private PlayerHealth playerHealth;

    public ParticleSystem healingParticles;

    void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnDamaged += HandlePlayerDamaged;

        if (healingParticles != null)
        {
            healingParticles.Stop();
        }
    }

    void HandlePlayerDamaged(float currentHealth)
    {
        if (currentHealth < healThreshold)
        {
            StartHealing();
        }
    }

    public void StartHealing()
    {
        animator.SetBool("IsHealing", true);
        if (healingParticles != null)
        {
            healingParticles.Play();
        }
        StartCoroutine(StopHealingAfter(3f)); // heals for 3 seconds
    }

    IEnumerator StopHealingAfter(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("IsHealing", false);
        animator.SetTrigger("FinishHeal");

        if (healingParticles != null)
        {
            healingParticles.Stop();
        }

        playerHealth.RestoreHealth(healAmount);
    }
}
