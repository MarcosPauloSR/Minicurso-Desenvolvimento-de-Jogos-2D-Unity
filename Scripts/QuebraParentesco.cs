using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuebraParentesco : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            transform.DetachChildren();
        }
    }
}
