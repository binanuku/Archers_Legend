using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinBolt : MonoBehaviour
{
    Rigidbody rb;
    Vector3 NewDir;
    int bounceCnt = 3;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        NewDir = transform.up;
        rb.velocity = NewDir * -10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            bounceCnt--;
            if (bounceCnt > 0)
            {
                NewDir = Vector3.Reflect(NewDir, collision.contacts[0].normal);
                rb.velocity = NewDir * -10f;
            }
            else
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
