using UnityEngine.EventSystems;

public interface ITooltipNameable : IPointerEnterHandler, IPointerExitHandler
{
    public string GetText();
}
