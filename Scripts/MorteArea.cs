using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteArea : MonoBehaviour
{

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("pass"))
        {
            Destroy(collision.gameObject);
        }
    }

}
