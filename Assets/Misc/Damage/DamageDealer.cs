using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public DamageType type;
    public Transform centerOverride;

    /*
    private void OnCollisionStay(Collision collision)
    {
        Damageable receiver = collision.collider.GetComponent<Damageable>();

        if (receiver != null)
        {
            Damage damage = new Damage();
            damage.type = type;
            
            // Direction
            Vector3 v = collision.collider.transform.position;
            if (centerOverride == null)
                v -= transform.position;
            else
                v -= centerOverride.position;
            v = v.normalized;
            v.y = 0;
            damage.dir = v;

            // Send Damage Request
            receiver.TakeDamage(damage);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        Damageable receiver = collider.GetComponent<Damageable>();

        if (receiver != null)
        {
            Damage damage = new Damage();
            damage.type = type;

            // Direction
            Vector3 v = collider.transform.position;
            if (centerOverride == null)
                v -= transform.position;
            else
                v -= centerOverride.position;
            v = v.normalized;
            v.y = 0;
            damage.dir = v;

            // Send Damage Request
            receiver.TakeDamage(damage);
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        Damageable receiver = other.GetComponent<Damageable>();

        if (receiver != null)
        {
            Damage damage = new Damage();
            damage.type = type;

            // Direction
            Vector3 v = other.transform.position;
            if (centerOverride == null)
                v -= transform.position;
            else
                v -= centerOverride.position;
            v = v.normalized;
            v.y = 0;
            damage.dir = v;

            // Send Damage Request
            receiver.TakeDamage(damage);
        }
    }
}
