using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target; //player
    public Vector3 offset = new Vector3(0, 50, 0); //height above the player

    void LateUpdate()
    {
        if (target != null)
        {
            //follow the player's position
            transform.position = target.position + offset;

            //rotate to match the player's facing direction
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        }
    }

}
