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

    [Space(10)]
    [SerializeField] Color _colorHighlight;
    [SerializeField] Image _imageBorder;


    public RecipeSO RecipeSO => _recipe;

    public void Refresh()
    {
        // set sprite from product of recipe
        _imageRecipe.sprite = _recipe.Product.Sprite;

        // fill info if available
        if (PlayerManager.Instance.PlayerAlchemistData.IsRecipeAvailable(_recipe))
        {
            _imageRecipe.gameObject.SetActive(false);
            _imageRecipe.color = Color.white;
            _textName.text = _recipe.Product.ItemName;
            _textDesc.text = _recipe.Product.ItemDesc;
        }
        else
        {
            _imageRecipe.gameObject.SetActive(true);
            _imageRecipe.color = Color.black;
            _textName.text = UtilsText.AllText[UtilsText.text_job_alchemist_recipe_locked];
            _textDesc.text = UtilsText.AllText[UtilsText.text_job_alchemist_recipe_locked];
        }
    }

    public void OnClick()
    {
        tabAlchemist.OnSelectRecipe(this);
    }

    public void Select(bool select)
    {
        _imageBorder.color = select ? _colorHighlight : Color.white;
    }
}
