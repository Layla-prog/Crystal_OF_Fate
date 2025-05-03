using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 15f;

    private Animator animator;
    private bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasExploded)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
        Explode();
    }

    void Explode()
    {
        hasExploded = true;
        animator.SetTrigger("Explode");
        Destroy(gameObject, 1f); 
    }
}
