using System;

public class SummonData
{
    public float CurrentAtkPerc { get; private set; }
    public float CurrentAtkSpd => 0.5f;
    public float MaxHp { get; private set; }
    public float CurrentHp { get; private set; }


    public bool IsDead => CurrentHp <= 0;


    public event Action OnDeath;

    public SummonData() { }


    public SummonData(float atkPerc, float maxHp)
    {
        CurrentAtkPerc = atkPerc;
        MaxHp = maxHp;
        CurrentHp = maxHp;
    }

    public void DecreaseHp(float val)
    {
        CurrentHp -= val;

        if (IsDead)
        {
            OnDeath?.Invoke();
        }
    }
}
