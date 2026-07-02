using System;

public interface IBasePlayerData
{
    public void AddLevel(int amount);
    public void AddStatPoints(int amount);

    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;
}
