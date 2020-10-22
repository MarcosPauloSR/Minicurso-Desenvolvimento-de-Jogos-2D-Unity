using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteTudo : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Morte());
    }

    public IEnumerator Morte()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
