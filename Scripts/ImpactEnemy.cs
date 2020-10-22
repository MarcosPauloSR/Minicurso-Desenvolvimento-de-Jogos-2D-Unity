using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEnemy : MonoBehaviour
{

    [SerializeField]
    private GameObject bomb;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
