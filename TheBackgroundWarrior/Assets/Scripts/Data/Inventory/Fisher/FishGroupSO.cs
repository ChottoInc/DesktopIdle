using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Fisher/Fish Group Data", fileName = "FishGroupData_")]
public class FishGroupSO : ListableGameDataSO
{
    [SerializeField] UtilsGather.FishGroupType groupType;
    [SerializeField] string groupNameTextId;
    [SerializeField] string groupName;

    [Space(10)]
    [SerializeField] string groupDescTextId;
    [TextArea]
    [SerializeField] string groupDesc;

    [Space(10)]
    [SerializeField] FishSO[] fishes;

    public override int Id => (int)groupType;


    public UtilsGather.FishGroupType GroupType => groupType;
    public string GroupName
    {
        get
        {
            string res = UtilsText.AllText[groupNameTextId];
            if (res != null) return res; else return groupName;
        }
    }
    public string GroupDesc
    {
        get
        {
            string res = UtilsText.AllText[groupDescTextId];
            if (res != null) return res; else return groupDesc;
        }
    }


    public FishSO[] Fishes => fishes;
}
