using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public Animator animator;

    public void StartHealing()
    {
        animator.SetBool("IsHealing", true);
        StartCoroutine(StopHealingAfter(3f)); // heals for 3 seconds
    }

    IEnumerator StopHealingAfter(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("IsHealing", false);
        animator.SetTrigger("FinishHeal");
    }
}
