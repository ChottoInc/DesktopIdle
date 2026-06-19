using System;

public interface IHealable
{
    public event Action<int> OnHeal;
}
