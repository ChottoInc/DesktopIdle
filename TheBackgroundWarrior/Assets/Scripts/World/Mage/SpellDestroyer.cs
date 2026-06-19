using UnityEngine;

public class SpellDestroyer : MonoBehaviour
{
    [SerializeField] SpellMage spell;

    public void EndAnimation()
    {
        spell.EndAnimation();
    }
}
