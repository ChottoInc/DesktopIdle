using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Job Data", fileName = "JobData_")]
public class PlayerJobSO : ScriptableObject
{
    [SerializeField] UtilsPlayer.PlayerJob job;
    [SerializeField] UtilsPlayer.PlayerJob[] requiredJobs;

    [Space(10)]
    [SerializeField] string jobName;
    [SerializeField] string unlockConditionsTextId;
    [SerializeField] string unlockConditions;


    public UtilsPlayer.PlayerJob Job => job;
    public UtilsPlayer.PlayerJob[] RequiredJobs => requiredJobs;

    public string JobName => jobName;
    public string JobUnlockConditions
    {
        get
        {
            string res = UtilsText.AllTextDictionary[unlockConditionsTextId];
            if (res != null) return res; else return unlockConditions;
        }
    }
}
