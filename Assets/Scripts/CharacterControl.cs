using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float jogSpeed, runSpeed, jumpPower, turnSpeed;

    private float xInput, zInput;

    private float speed;

    private Vector3 movement;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = jogSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Get movement Input
        xInput = Input.GetAxis("Horizontal");

        zInput = Input.GetAxis("Vertical");

        //Check if the player is running
        if (Input.GetAxis("Run") > 0.1f)
        {
            speed = runSpeed;
        }
        else
        {
            speed = jogSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        //Set movement
        movement = new Vector3(xInput, 0, zInput) * 100 * speed * Time.deltaTime;

        if (movement.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
        }

        //Change the velocity
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
