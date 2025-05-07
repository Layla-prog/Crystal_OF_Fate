using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float jogSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpPower = 8f;
    public float gravity = 20f;
    public float turnSpeed = 10f;

    private float xInput, zInput;

    private float speed;

    private Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;
    private Animator animator;

    //Boost values
    private float baseJogSpeed;
    private float baseRunSpeed;

    private string storedPotionType = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        speed = jogSpeed;
        baseJogSpeed = jogSpeed;
        baseRunSpeed = runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Get movement Input
        xInput = Input.GetAxis("Horizontal");

        zInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(xInput, 0, zInput).normalized;

        //Check if the player is running
        if (Input.GetAxis("Run") > 0.1f)
        {
            speed = runSpeed;
        }
        else
        {
            speed = jogSpeed;
        }


        if (controller.isGrounded)
        {
			moveDirection = inputDirection * speed;

			if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpPower;
            }

            // only rotate if moving
            if (inputDirection.magnitude > 0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(inputDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }

            float movementSpeed = new Vector2(xInput, zInput).magnitude;
            animator.SetFloat("Speed", movementSpeed);
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the player
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Transform potion = transform.Find("hips/handslot.l/PotionAnchor/Potion(Clone)");
            if (potion != null && !string.IsNullOrEmpty(storedPotionType))
            {
                animator.SetTrigger("UseItem");
                if (storedPotionType == "Strength")
                {
                    StartCoroutine(ApplyStrengthBoost());
                }
                else if (storedPotionType == "Stamina")
                {
                    StartCoroutine (ApplyStaminaBoost());
                }

                Destroy(potion.gameObject); // Remove the potion after use
                storedPotionType = null;
            }
        }
    }

    public void SetCurrentPotionType(string type)
    {
        storedPotionType = type;
        Debug.Log($"Received {type} potion. Press E to use it.");
    }

    IEnumerator ApplyStrengthBoost()
    {
        Debug.Log("Strength Boost Activated!");
        runSpeed *= 1.5f;
        yield return new WaitForSeconds(10f);
        runSpeed = baseRunSpeed;
    }

    IEnumerator ApplyStaminaBoost()
    {
        Debug.Log("Stamina Boost Activated!");
        jogSpeed *= 1.5f;
        yield return new WaitForSeconds(10f);
        jogSpeed = baseJogSpeed;
    }
}
