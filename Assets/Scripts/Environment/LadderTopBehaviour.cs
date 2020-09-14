using UnityEngine;
using System.Collections;

public class LadderTopBehaviour : MonoBehaviour
{
    public GameObject floorObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(collision, floorObject.GetComponent<Collider2D>(), true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(collision, floorObject.GetComponent<Collider2D>(), false);
    }

}
