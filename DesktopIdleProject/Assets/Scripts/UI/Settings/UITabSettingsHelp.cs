using UnityEngine;

public class UITabSettingsHelp : UITabWindow
{
    [SerializeField] UIHelpJobFilter[] filters;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        foreach (var filter in filters)
        {
            filter.gameObject.SetActive(PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(filter.Job));
        }
    }
}
