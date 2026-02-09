using UnityEngine;
using UnityEngine.UI;

public class UIButtonJobTab : MonoBehaviour
{
    [SerializeField] UtilsPlayer.PlayerJob job;

    [Space(10)]
    [SerializeField] GameObject[] links;

    [Space(10)]
    [SerializeField] GameObject barrier;
    [SerializeField] Button button;

    private PlayerJobSO jobSO;
    
    public void Refresh()
    {
        // Initialize
        if(jobSO == null)
        {
            jobSO = UtilsPlayer.GetJobByType(job);
        }

        // Check if all required jobs are available, if so show the job
        bool show = true;

        foreach (var requiredJob in jobSO.RequiredJobs)
        {
            if (!PlayerManager.Instance.AvailableJobs.Contains(requiredJob))
            {
                show = false;
            }
        }

        gameObject.SetActive(show);

        foreach (var link in links)
        {
            link.SetActive(show);
        }

        // Check if the player can actually do the job, if so activate the button
        bool active = PlayerManager.Instance.AvailableJobs.Contains(job);

        button.interactable = active;
        barrier.SetActive(!active);
    }
}
