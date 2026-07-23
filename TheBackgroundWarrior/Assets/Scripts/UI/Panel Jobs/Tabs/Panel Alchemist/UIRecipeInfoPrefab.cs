using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeInfoPrefab : MonoBehaviour
{
    [SerializeField] UITabJobAlchemist tabAlchemist;

    [Space(10)]
    [SerializeField] RecipeSO _recipe;

    [Space(10)]
    [SerializeField] Image _imageRecipe;
    [SerializeField] TMP_Text _textName;
    [SerializeField] TMP_Text _textDesc;


    public bool IsAvailable { get; private set; }


    public RecipeSO RecipeSO => _recipe;

    public void Refresh()
    {
        // set sprite from product of recipe
        _imageRecipe.sprite = _recipe.Product.Sprite;

        // fill info if available
        IsAvailable = PlayerManager.Instance.PlayerAlchemistData.IsRecipeAvailable(_recipe);

        if (IsAvailable)
        {
            _imageRecipe.color = Color.white;
            _textName.text = _recipe.Product.ItemName;
            _textDesc.text = _recipe.Product.ItemDesc;
        }
        else
        {
            _imageRecipe.color = Color.black;
            _textName.text = UtilsText.AllText[UtilsText.text_job_alchemist_recipe_locked];
            _textDesc.text = UtilsText.AllText[UtilsText.text_job_alchemist_recipe_locked];
        }
    }

    public void OnClick()
    {
        if (!IsAvailable) return;

        AudioManager.Instance.PlayClickUI();

        tabAlchemist.OnSelectRecipe(this);
    }
}
