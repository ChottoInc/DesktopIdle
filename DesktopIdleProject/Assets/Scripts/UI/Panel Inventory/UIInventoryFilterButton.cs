using UnityEngine;

public class UIInventoryFilterButton : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;
    [SerializeField] int filterId;
    [SerializeField] UtilsPlayer.PlayerJob[] showIfAvailableJobs;

    public void Refresh()
    {
        bool canShow = true;

        foreach (var job in showIfAvailableJobs)
        {
            if(!PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(job))
            {
                canShow = false;
                break;
            }
        }

        gameObject.SetActive(canShow);
    }

    public void OnButtonClick()
    {
        tabInventory.OpenInventory(filterId);
    }
}
