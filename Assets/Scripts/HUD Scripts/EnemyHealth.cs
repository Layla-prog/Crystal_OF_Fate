using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHelath = 100f;
    public float currentHelath;

    public EnemyHealthUI healthUI;

    // Start is called before the first frame update
    void Start()
    {
        currentHelath = maxHelath;
    }

    public void TakeDamage(float damage)
    {
        currentHelath -= damage;
        currentHelath = Mathf.Max(0, currentHelath);

        healthUI.Show(currentHelath / maxHelath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
