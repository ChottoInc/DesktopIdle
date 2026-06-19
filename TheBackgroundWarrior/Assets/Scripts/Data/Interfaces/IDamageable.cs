using System;

public interface IDamageable
{
    public event Action<int> OnTakeDamage;
}
