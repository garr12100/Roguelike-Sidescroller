using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter between " + gameObject.name + " and " + collision.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger Stay between " + gameObject.name + " and " + collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger Exit between " + gameObject.name + " and " + collision.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Enter between " + gameObject.name + " and " + collision.gameObject.name);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Collision Stay between " + gameObject.name + " and " + collision.gameObject.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collision Exit between " + gameObject.name + " and " + collision.gameObject.name);
    }


}
