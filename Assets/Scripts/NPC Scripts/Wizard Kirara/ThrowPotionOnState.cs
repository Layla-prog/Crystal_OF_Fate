using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPotionOnState : StateMachineBehaviour
{
    public float throwDelay = 0.3f; // Delay to sync with animation motion

    private float timer = 0f;
    private bool hasThrown = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        hasThrown = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (!hasThrown && timer >= throwDelay)
        {
            WizardEnemyAI combat = animator.GetComponent<WizardEnemyAI>();
            if (combat != null)
            {
                combat.ThrowPotionAtPlayer(); // Your method to throw the actual potion
                hasThrown = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
