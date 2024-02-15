using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public bool isColliding;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("assignment2Objects"))
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("assignment2Objects"))
        {
            isColliding = false;
        }
    }
}
