using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelItems : MonoBehaviour
{

    [SerializeField] GameObject invItemPrefab;
    [SerializeField] Transform container;

    private List<GameObject> itemObjs;

    private List<ItemGroup> itemGroups;

    private int currentInvFilter;

    [SerializeField] UIInventoryPanelInfo panelInfo;

    public void Setup(int filter)
    {
        currentInvFilter = filter;

        itemObjs = ClearList(itemObjs);

        FillWindow();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillWindow()
    {
        // clear previuos list
        if (itemGroups != null) itemGroups.Clear();

        // get updated list
        itemGroups = new List<ItemGroup>(PlayerManager.Instance.Inventory.ItemGroups);

        for (int i = 0; i < itemGroups.Count; i++)
        {
            switch (currentInvFilter)
            {
                case UITabInventory.ID_INVENTORY_FILTER_ALL: CreateSinglePrefab(itemGroups[i]); break;
            }
        }
    }

    private void CreateSinglePrefab(ItemGroup group)
    {
        GameObject prefab = Instantiate(invItemPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIInventoryItem obj))
        {
            obj.Setup(this, group);
        }
        itemObjs.Add(prefab);
    }


    public void ShowPanelInfo(bool show)
    {
        panelInfo.Show(show);
    }

    public void ShowDetails(ItemGroup group)
    {
        panelInfo.Show(true);
        panelInfo.Setup(group);
    }
}
