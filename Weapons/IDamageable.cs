using UnityEngine;

public interface IDamageable
{
    void TakeHit(Hit hit);

    void TakeDamage(float damage);
}