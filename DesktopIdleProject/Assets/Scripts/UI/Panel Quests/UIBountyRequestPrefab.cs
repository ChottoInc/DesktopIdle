using UnityEngine;

public class UIBountyRequestPrefab : MonoBehaviour
{
    [SerializeField] int slot;
    [SerializeField] UITabQuestsBounties tabBounties;

    public void OnButtonChoose()
    {
        tabBounties.OpenBountiesList(slot);
    }
}
