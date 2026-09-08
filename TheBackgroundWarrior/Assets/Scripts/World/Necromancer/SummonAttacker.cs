using UnityEngine;

public class SummonAttacker : MonoBehaviour
{
    [SerializeField] Summon _summon;

    public void ExternalAttack()
    {
        _summon.ExternalAttack();
    }
}
