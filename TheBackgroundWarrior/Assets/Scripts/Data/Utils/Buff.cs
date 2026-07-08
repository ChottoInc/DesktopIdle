
using System;
using static UtilsBuffs;

[System.Serializable]
public class Buff
{
    private BuffType _buffType;
    private float _remainingTime;

    public float StartDuration { get; private set; }

    public event Action<BuffType> OnBuffExpired;

    public BuffType BuffType => _buffType;
    public float RemainingTime => _remainingTime;

    public bool IsExpired => _remainingTime <= 0 ? true : false;

    public Buff(BuffType buffType, float remainingTime)
    {
        _buffType = buffType;
        _remainingTime = remainingTime;

        StartDuration = remainingTime;
    }

    public Buff(BuffSaveData saveData)
    {
        _buffType = (BuffType)saveData.buffType;
        _remainingTime = saveData.remainingTime;
    }

    public void AddTimer(float val)
    {
        _remainingTime += val;
    }

    public void DecreaseTimer(float val)
    {
        _remainingTime -= val;
        if (IsExpired) OnBuffExpired?.Invoke(_buffType);
    }
}
