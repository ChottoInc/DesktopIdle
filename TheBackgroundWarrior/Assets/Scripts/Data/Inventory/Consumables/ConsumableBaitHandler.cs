using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBaitHandler : AbstractConsumableHandler<BaitSO>
{
    protected override bool UseItem(BaitSO bait)
    {
        PlayerFisherData fisherData = PlayerManager.Instance.PlayerFisherData;

        // for now returns false, can handle overriding bait in the future, or accumulate if use more than one
        if (fisherData.IsBaitActive) return false;

        fisherData.SetActiveBait(bait);
        PlayerManager.Instance.SaveFisherData();

        return true;
    }
}
