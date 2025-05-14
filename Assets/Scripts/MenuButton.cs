using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
     [SerializeField] UnityEvent onPressAction; 
     [SerializeField] AudioSource buttonSound; 

         private bool keyDown = false;

  public MenuButtonController MenuButtonController 
    {
        get { return menuButtonController; }
        set { menuButtonController = value; }
    }

    public Animator Animator 
    {
        get { return animator; }
        set { animator = value; }
    }

    public AnimatorFunctions AnimatorFunctions 
    {
        get { return animatorFunctions; }
        set { animatorFunctions = value; }
    }

    public int ThisIndex 
    {
        get { return thisIndex; }
        set { thisIndex = value; }
    }


    // Update is called once per frame

    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);

            if (Input.GetAxisRaw("Submit") == 1)
            {
                if (!keyDown)
                {
                    keyDown = true;
                    animator.SetBool("pressed", true);
                    animatorFunctions.disableOnce = true;

                    if (buttonSound != null)
                        buttonSound.Play();

                    StartCoroutine(DelayedAction());
                }
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                keyDown = false;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }

    IEnumerator DelayedAction()
    {
        float delay = 0.3f; // fallback delay

        if (buttonSound != null && buttonSound.clip != null)
            delay = buttonSound.clip.length;

        yield return new WaitForSeconds(delay);
        onPressAction.Invoke();
    }
}
