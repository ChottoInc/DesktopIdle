using UnityEngine;
using UnityEngine.UI;

public class UIPanelShrink : MonoBehaviour
{
    [SerializeField] Sprite spriteShrink;
    [SerializeField] Sprite spriteExpand;
    [SerializeField] Image imageButton;
    [SerializeField] GameObject[] objectsToHide;

    private bool isExpanded;

    private void Start()
    {
        isExpanded = true;

        UpdateButtonUI();
    }

    public void OnButtonShrink()
    {
        AudioManager.Instance.PlayClickUI();

        isExpanded = !isExpanded;

        foreach (var item in objectsToHide)
        {
            item.SetActive(isExpanded);
        }

        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        imageButton.sprite = isExpanded ? spriteShrink : spriteExpand;
    }
}
