using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 8f;
    public float maxLifetime = 5f;
    
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            // Rotate to face the player horizontally only
            Vector3 dir = (target.position - transform.position).normalized;
            dir.y = 0f;
            transform.forward = dir;
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.collider.GetComponent<PlayerHealth>();
            if (health != null)
            {
                Debug.Log("PlayerHealth script found!");
                health.TakeDamage(damage); 
            }
        }

        // Play explosion animation.
        Explode();
    }


    void OnTriggerEnter(Collider other)
    {
        //if (hasExploded) return;

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
        // You can optionally disable the mesh or collider
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        Destroy(gameObject, 1f); 
    }
}
