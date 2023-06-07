using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;
    private bool isInvulnerable;
    //--ImpactStateLogic komentuj� do czas a� zaimplementujemy "HeavyAttack dla boss�w"
    public event Action OnTakeDamage;
    
    public event Action OnDie;



    public bool IsDead => health == 0;
    private void Start()
    {
        health = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }



    public void DealDamage(int damagevalue)
    {
        if (health == 0){    return; }

        if (isInvulnerable) { return; }

       health = Mathf.Max(health - damagevalue, 0);
        //--ImpactStateLogic komentuj� do czas a� zaimplementujemy "HeavyAttack dla boss�w"
        OnTakeDamage?.Invoke();
       
        if(health == 0)
        { OnDie?.Invoke(); }

        Debug.Log(health);
    }
}
