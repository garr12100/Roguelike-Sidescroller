using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(DeathHandler))]
public class Health : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public float invulnerabilityTime = 1f;
    [SerializeField]
    private bool invulnerable = false;

    public event Action<bool> OnInvulnerability;
    public event Action OnTakeDamage;
    public event Action<Vector2> OnTakeDamageDirectional;


    private DeathHandler deathHandler;

    private void Awake()
    {
        deathHandler = GetComponent<DeathHandler>();
    }

    protected virtual void Start()
    {
        FullRestore();
    }

    public void TakeDamage(float amount, Vector2 direction = new Vector2())
    {
        if (!invulnerable)
        {
            currentHealth -= amount;
            if (!DeathCheck())
            {
                StartCoroutine(InvulnerabilityCountdown());
                if (OnTakeDamage != null)
                    OnTakeDamage();
                if (OnTakeDamageDirectional != null)
                    OnTakeDamageDirectional(direction);
            }
        }
    }



    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void FullRestore()
    {
        currentHealth = maxHealth;
    }

    private bool DeathCheck()
    {
        if (currentHealth <= 0)
        {
            Die();
            deathHandler.Die();
            return true;
        }
        return false;
    }

    private void Die()
    {
        SetInvulnerable(false);
        StopAllCoroutines();
    }

    IEnumerator InvulnerabilityCountdown()
    {
        SetInvulnerable(true);
        yield return new WaitForSeconds(invulnerabilityTime);
        SetInvulnerable(false);
    }

    public void SetInvulnerable(bool val)
    {
        invulnerable = val;
        if (OnInvulnerability != null)
            OnInvulnerability(invulnerable);
    }

}
