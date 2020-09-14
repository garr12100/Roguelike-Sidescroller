using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactHazard : MonoBehaviour
{
    public float damage = 1f;
    public bool directional = true;
    public Transform origin;
    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag(UtilityStrings.Tags.Player))
        {
            var health = otherCollider.GetComponentInChildren<Health>();
            if (health != null)
            {
                if (directional)
                {
                    Vector3 pos = origin == null ? transform.position : origin.position;
                    Vector2 dir = otherCollider.transform.position - pos;
                    health.TakeDamage(damage, dir);
                }
                else
                    health.TakeDamage(damage);
            }
        }
    }
}
