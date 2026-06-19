using UnityEngine;

public class WarriorSpellPerformer : MonoBehaviour
{
    [SerializeField] SpellWarrior spell;

    public void Perform()
    {
        spell.ExternalMakeEffect();
    }
}
