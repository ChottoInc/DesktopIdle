using UnityEngine;

public class BuffToSprite : ListableGameDataSO
{
    [SerializeField] UtilsBuffs.BuffType _buffType;
    [SerializeField] Sprite _sprite;

    public UtilsBuffs.BuffType BuffType => _buffType;
    public Sprite Sprite => _sprite;
}
