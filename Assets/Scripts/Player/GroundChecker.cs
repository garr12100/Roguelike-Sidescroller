using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour
{
    public event System.Action OnNotGrounded;
    public enum Direction { up, down, forward, back }
    public Direction direction;
    public int numRays;
    public float spacing;
    public float rayDistance = .2f;
    public LayerMask groundLayers;

    public bool grounded;
    //{
    //    get
    //    {
    //        Vector2 dir = Vector2.zero;
    //        switch (direction)
    //        {
    //            case Direction.up:
    //                dir = Vector2.up;
    //                break;
    //            case Direction.down:
    //                dir = Vector2.down;
    //                break;
    //            case Direction.forward:
    //                dir = Vector2.right;
    //                break;
    //            case Direction.back:
    //                dir = Vector2.left;
    //                break;
    //        }
    //        int min = (int)Mathf.Ceil(-(float)numRays / 2);
    //        int max = numRays / 2;
    //        for (int i = min; i < max; i++)
    //        {
    //            Vector2 pos;
    //            if (direction == Direction.back || direction == Direction.forward)
    //                pos = new Vector2(transform.position.x, transform.position.y + (i * spacing));
    //            else
    //                pos = new Vector2(transform.position.x + (i * spacing), transform.position.y);
    //            if (Physics2D.Raycast(pos, dir, rayDistance, groundLayers))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}

    private void Update()
    {
        Vector2 dir = Vector2.zero;
        switch (direction)
        {
            case Direction.up:
                dir = Vector2.up;
                break;
            case Direction.down:
                dir = Vector2.down;
                break;
            case Direction.forward:
                dir = Vector2.right;
                break;
            case Direction.back:
                dir = Vector2.left;
                break;
        }
        int min = (int)Mathf.Ceil(-(float)numRays / 2);
        int max = numRays / 2;
        for (int i = min; i <= max; i++)
        {
            Vector2 pos;
            if (direction == Direction.back || direction == Direction.forward)
                pos = new Vector2(transform.position.x, transform.position.y + (i * spacing));
            else
                pos = new Vector2(transform.position.x + (i * spacing), transform.position.y);
            Debug.DrawRay(pos, dir * rayDistance, Color.blue);
            if (Physics2D.Raycast(pos, dir, rayDistance, groundLayers))
            {
                grounded = true;
                return;
            }
        }
        if (grounded && OnNotGrounded != null)
            OnNotGrounded();
        grounded = false;
    }

    //   Collider2D col;
    //// Use this for initialization
    //void Start ()
    //   {
    //       col = GetComponent<Collider2D>();
    //}

    //   // Update is called once per frame
    //   void OnTriggerStay2D(Collider2D otherCol)
    //   {
    //       if (otherCol.CompareTag(UtilityStrings.Tags.Level))
    //       {
    //           grounded = true;
    //       }
    //   }

    //   void OnTriggerExit2D(Collider2D otherCol)
    //   {
    //       if (otherCol.CompareTag(UtilityStrings.Tags.Level))
    //       {
    //           if (grounded && OnNotGrounded != null)
    //               OnNotGrounded();
    //           grounded = false;

    //       }
    //   }
}
