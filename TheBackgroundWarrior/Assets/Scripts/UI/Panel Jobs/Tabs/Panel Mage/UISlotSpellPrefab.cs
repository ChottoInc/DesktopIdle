using UnityEngine;
using UnityEngine.UI;

public class UISlotSpellPrefab : MonoBehaviour
{
    [SerializeField] UITabJobMage _tabMage;

    [Space(10)]
    [SerializeField] int idSlot;
    [SerializeField] Image iconSpell;
    [SerializeField] Sprite spriteLocked;

    public bool IsUnlocked { get; private set; }

    private SpellData _spellData;

    public void Setup(UtilsMage.MageSpellType spellType)
    {
        _spellData = PlayerManager.Instance.PlayerMageData.GetSpellByType(spellType);

        IsUnlocked = PlayerManager.Instance.PlayerMageData.IsSlotUnlocked(idSlot);

        if (IsUnlocked)
        {
            if (_spellData == null)
            {
                iconSpell.gameObject.SetActive(false);
            }
            else
            {
                iconSpell.sprite = _spellData.SpellSO.Sprite;
                iconSpell.gameObject.SetActive(true);
            }
        }
        else
        {
            iconSpell.sprite = spriteLocked;
            iconSpell.gameObject.SetActive(true);
        }
    }

    public void OnClick()
    {
        if (_spellData == null) return;

        PlayerManager.Instance.PlayerMageData.UnequipFromSlot(idSlot);
        _tabMage.Open();
        PlayerManager.Instance.SaveMageData();
    }
}
