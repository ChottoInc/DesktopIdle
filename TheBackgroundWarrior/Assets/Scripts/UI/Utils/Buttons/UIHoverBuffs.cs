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

    private void Start()
    {
        gameObject.SetActive(PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(UtilsPlayer.PlayerJob.Alchemist));
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
        if (PlayerManager.Instance.PlayerBuffsData.ActiveBuffs.Count < 1) return;

        if (_isTabbed) return;

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_BUFFS;
        UITooltipManager.Instance.Show(tooltipData, transform.position, true);

        _isOpen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PlayerManager.Instance.PlayerBuffsData.ActiveBuffs.Count < 1) return;

        OnExit();
    }

    private void OnExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_BUFFS, true);

        _isOpen = false;
    }
}
