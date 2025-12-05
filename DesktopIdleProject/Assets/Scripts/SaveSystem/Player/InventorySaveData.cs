using System.Collections.Generic;

public class InventorySaveData
{
    public List<ItemGroupSaveData> groupSaves;

    public InventorySaveData() { }

    public InventorySaveData(Inventory inventory)
    {
        groupSaves = new List<ItemGroupSaveData>();

        foreach (var group in inventory.ItemGroups)
        {
            groupSaves.Add(new ItemGroupSaveData(group));
        }
    }
}
