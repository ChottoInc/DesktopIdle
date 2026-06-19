using UnityEngine;

public class MageLauncher : MonoBehaviour
{
    [SerializeField] PlayerMage _player;

    public void Cast()
    {
        _player.ExternalAttack();
    }
}
