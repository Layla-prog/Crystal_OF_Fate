using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
