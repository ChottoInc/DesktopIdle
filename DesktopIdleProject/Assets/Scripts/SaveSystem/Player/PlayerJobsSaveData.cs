using System.Collections.Generic;

public class PlayerJobsSaveData
{
    // Stores obtained jobs (which one are available)
    public List<int> availableJobs;

    public PlayerJobsSaveData() { }

    public PlayerJobsSaveData(PlayerManager manager)
    {
        availableJobs = new List<int>();

        foreach (var job in manager.AvailableJobs)
        {
            availableJobs.Add((int)job);
        }
    }
}
