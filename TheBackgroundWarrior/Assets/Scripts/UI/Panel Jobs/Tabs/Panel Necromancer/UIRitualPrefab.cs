using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRitualPrefab : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UtilsNecromancer.RitualType _ritualType;

    [Space(10)]
    [SerializeField] TMP_Text _textName;
    [SerializeField] TMP_Text _textDesc;

    [Space(10)]
    [SerializeField] Animator _ritualAnimator;
    [SerializeField] RuntimeAnimatorController _ritualAnimatorController;

    private bool _isUnlocked;


    public void Refresh()
    {
        PlayerNecromancerData data = PlayerManager.Instance.PlayerNecromancerData;

        switch (_ritualType)
        {
            case UtilsNecromancer.RitualType.Summon:
                _isUnlocked = true;
                _textName.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_summon_name];
                _textDesc.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_summon_desc]; 
                break;

            case UtilsNecromancer.RitualType.Arise:

                if(data.IsAriseRitualUnlocked)
                {
                    _isUnlocked = true;
                    _textName.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_arise_name];
                    _textDesc.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_arise_desc];
                }
                else
                {
                    _textName.text = UtilsText.AllText[UtilsText.text_job_necromancer_ritual_locked];
                    _textDesc.text = UtilsText.AllText[UtilsText.text_job_necromancer_arise_unlockconditions];
                }
                break;

            case UtilsNecromancer.RitualType.Afterlife:

                if (data.IsAfterlifeRitualUnlocked)
                {
                    _isUnlocked = true;
                    _textName.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_afterlife_name];
                    _textDesc.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_afterlife_desc];
                }
                else
                {
                    _textName.text = UtilsText.AllText[UtilsText.text_job_necromancer_ritual_locked];
                    _textDesc.text = UtilsText.AllText[UtilsText.text_job_necromancer_afterlife_unlockconditions];
                }
                break;

            case UtilsNecromancer.RitualType.Ade:

                if (data.IsAdeRitualUnlocked)
                {
                    _isUnlocked = true;
                    _textName.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_ade_name];
                    _textDesc.text = UtilsText.AllText[UtilsText.text_necromancer_ritual_ade_desc];
                }
                else
                {
                    _textName.text = UtilsText.AllText[UtilsText.text_job_necromancer_ritual_locked];
                    _textDesc.text = UtilsText.AllText[UtilsText.text_job_necromancer_ade_unlockconditions];
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isUnlocked)
        {
            _ritualAnimator.Play(_ritualAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 0.0f);
            _ritualAnimator.runtimeAnimatorController = _ritualAnimatorController;
        }
    }
}
