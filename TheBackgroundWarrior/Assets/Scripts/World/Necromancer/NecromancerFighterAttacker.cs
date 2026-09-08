using UnityEngine;

public class NecromancerFighterAttacker : MonoBehaviour
{
    [SerializeField] NecromancerFighter _fighter;

    public void ExternalAttack()
    {
        _fighter.ExternalAttack();
    }
}
