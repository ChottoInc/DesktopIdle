
[System.Serializable]
public class BuffSaveData
{
    public int buffType;
    public float remainingTime;

    public BuffSaveData() { }

    public BuffSaveData(Buff buff)
    {
        buffType = (int)buff.BuffType;
        remainingTime = buff.RemainingTime;
    }
}
