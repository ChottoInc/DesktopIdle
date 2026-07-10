using System;

public interface IBasePlayerData
{
    public void AddLevel(int amount, int maxLevel);
    public void AddStatPoints(int amount);
    public void RemoveStatPoints(int amount);

    public void AddExp(long amount, Func<int, bool> isMaxLevel, Func<long> expToNextLevel);

    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;
}
