using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody bulletRb;
    private void Start()
    {
        bulletRb.velocity = transform.forward * 20f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Monster"))
        {
            //bulletRb.velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Monster"))
        {
            //bulletRb.velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
