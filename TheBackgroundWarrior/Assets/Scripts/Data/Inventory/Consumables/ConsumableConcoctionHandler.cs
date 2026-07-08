

public class ConsumableConcoctionHandler : AbstractConsumableHandler<ConcoctionSO>
{
    protected override bool UseItem(ConcoctionSO item)
    {
        if (item.Permanent)
        {
            // add perm stats to player at start
            PlayerManager.Instance.PlayerAlchemistData.AddPermaStatCounter(item.PermaStat);
            PlayerManager.Instance.SaveAlchemistData();
        }
        else
        {
            // active buff for duration
            Buff buff = new Buff(item.Buff, item.Duration);
            PlayerManager.Instance.PlayerBuffsData.AddBuff(buff);
            PlayerManager.Instance.SaveBuffsData();
        }


        return true;
    }
}
