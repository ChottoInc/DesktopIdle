using UnityEngine;

public class NecromancerSummoner : MonoBehaviour
{
    [SerializeField] PlayerNecromancer _player;

    public void ExternalSummon()
    {
        _player.ExternalEndAnimation();
    }
}
