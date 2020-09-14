using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler_MovePosition : DeathHandler
{
    public Transform respawnPosition;

    public override void Die()
    {
        if (respawnPosition != null)
        {
            gameObject.transform.position = respawnPosition.position;
            var health = GetComponent<Health>();
            health.FullRestore();
        }
    }
}
