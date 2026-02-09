using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBountiesList : MonoBehaviour
{
    [SerializeField] UITabQuestsBounties panelQuest;

    [Space(10)]
    [SerializeField] GameObject bountyPrefab;
    [SerializeField] Transform container;

    private List<GameObject> questObjs;

    public void Open()
    {
        gameObject.SetActive(true);

        FillBounties();
    }

    private void FillBounties()
    {
        questObjs = ClearList(questObjs);

        var bountySOs = UtilsQuest.GetAllBountyQuests();

        for (int i = 0; i < bountySOs.Length; i++)
        {
            bool canShow = true;

            string currentId = bountySOs[i].UniqueId;

            if (!bountySOs[i].AvailableFor.SharesAnyValueWith(PlayerManager.Instance.AvailableJobs))
                canShow = false;

            if(QuestManager.Instance.IsBountyActiveById(currentId))
                canShow = false;

            if (canShow)
            {
                GameObject prefab = Instantiate(bountyPrefab, transform.position, Quaternion.identity);
                prefab.transform.SetParent(container);

                prefab.transform.localScale = new Vector3(1, 1, 1);

                if (prefab.TryGetComponent(out UIBountyPrefab obj))
                {
                    obj.Setup(this, currentId, bountySOs[i].QuestData);
                }
                questObjs.Add(prefab);
            }
        }
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
        {
            list = new List<GameObject>();
            return list;
        }

        foreach (var item in list)
        {
            Destroy(item);
        }
        list.Clear();
        return list;
    }

    public void OnBountyAccepted(string id)
    {
        panelQuest.OnBountyAccepted(id);

        // hide bounty list
        gameObject.SetActive(false);
    }

    public void OnButtonClose()
    {
        // back to previous menu
        panelQuest.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
