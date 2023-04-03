using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<Collider> alreadyCollideWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollideWith.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == myCollider) { return; }

        if(alreadyCollideWith.Contains(other)) { return; }

        alreadyCollideWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(10);
        
        }
    }
}
