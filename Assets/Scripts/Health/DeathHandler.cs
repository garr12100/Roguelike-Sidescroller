using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
