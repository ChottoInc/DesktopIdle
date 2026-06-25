using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBaitHandler : AbstractConsumableHandler<BaitSO>
{
    protected override bool UseItem(BaitSO item)
    {
        return true;
    }
}
