using UnityEngine;

public class UIInventoryFilterButton : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;
    [SerializeField] int filterId;

    public void OnButtonClick()
    {
        tabInventory.OpenInventory(filterId);
    }
}
