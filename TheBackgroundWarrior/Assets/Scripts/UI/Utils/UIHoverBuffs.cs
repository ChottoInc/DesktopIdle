using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBuffs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UITab _tabBuff;

    private bool _isOpen;
    private bool _isTabbed;

    private void Awake()
    {
        _tabBuff.OnSelected += Selected;
        _tabBuff.OnDeselected += Deselected;
    }

    private void OnDestroy()
    {
        _tabBuff.OnSelected -= Selected;
        _tabBuff.OnDeselected -= Deselected;
    }

    private void Selected()
    {
        _isTabbed = true;

        if(_isOpen)
        {
            OnExit();
        }
    }

    private void Deselected()
    {
        _isTabbed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isTabbed) return;

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_BUFFS;
        UITooltipManager.Instance.Show(tooltipData, transform.position, true);

        _isOpen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit();
    }

    private void OnExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_BUFFS, true);

        _isOpen = false;
    }
}
