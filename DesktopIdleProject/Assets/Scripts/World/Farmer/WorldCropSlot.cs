using UnityEngine;

public class WorldCropSlot : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] cropRenderers;

    public void SetSprite(Sprite sprite)
    {
        foreach(var renderer in cropRenderers)
        {
            renderer.sprite = sprite;
        }
    }
}
