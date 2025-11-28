using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAnimatedButton : Button
{
    [SerializeField] float timeSingleFrame = 0.2f;
    [SerializeField] Sprite[] spriteList;

    private int animationDirection = 1;
    private int spriteIndex = 0;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(CoAnimation(timeSingleFrame));
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        ResetButton();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        ResetButton();

        base.OnPointerClick(eventData);
    }

    private void ResetButton()
    {
        StopAllCoroutines();
        animationDirection = 1;
        spriteIndex = 0;

        image.sprite = spriteList[0];
    }

    private IEnumerator CoAnimation(float timeBtwFrames)
    {
        while (true)
        {
            image.sprite = GetNextSprite();
            yield return new WaitForSecondsRealtime(timeBtwFrames);
        }
    }

    private Sprite GetNextSprite()
    {
        if (spriteList == null || spriteList.Length == 0)
            return null;

        // return current frame
        Sprite result = spriteList[spriteIndex];

        // move index
        spriteIndex += animationDirection;

        // handle bouncing
        if (spriteIndex == spriteList.Length - 1)
            animationDirection = -1;
        else if (spriteIndex == 0)
            animationDirection = 1;

        return result;
    }
}
