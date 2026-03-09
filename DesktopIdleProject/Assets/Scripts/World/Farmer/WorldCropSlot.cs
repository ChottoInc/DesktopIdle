using UnityEngine;

public class WorldCropSlot : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] cropRenderers;

    public Transform[] CropTransforms { get; private set; }

    private void Start()
    {
        CropTransforms = new Transform[cropRenderers.Length];
        for (int i = 0; i < cropRenderers.Length; i++)
        {
            CropTransforms[i] = cropRenderers[i].transform;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        foreach(var renderer in cropRenderers)
        {
            renderer.sprite = sprite;
        }
    }
}
