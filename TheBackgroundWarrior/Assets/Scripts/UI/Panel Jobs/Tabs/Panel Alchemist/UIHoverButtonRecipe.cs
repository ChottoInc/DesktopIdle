using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverButtonRecipe : UIHoverButtonTab
{
    [SerializeField] UIRecipeInfoPrefab _recipe;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!_recipe.IsAvailable) return;

        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!_recipe.IsAvailable) return;

        base.OnPointerExit(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!_recipe.IsAvailable) return;

        base.OnPointerClick(eventData);
    }
}
