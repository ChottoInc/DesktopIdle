using System.Collections.Generic;
using System.Linq;

public class PlayerBuffsSaveData
{
    public List<BuffSaveData> buffs;

    public PlayerBuffsSaveData() { }

    public PlayerBuffsSaveData(PlayerBuffsData data)
    {
        buffs = data.ActiveBuffs.Select(buff => new BuffSaveData(buff)).ToList();
    }
}
