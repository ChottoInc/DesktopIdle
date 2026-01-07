using DG.Tweening;
using TMPro;
using UnityEngine;


public class UITooltipName : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [Space(10)]
    [SerializeField] float timeToFade = 1f;

    [Space(10)]
    [SerializeField] RectTransform root;
    [SerializeField] TMP_Text textName;
    [SerializeField] float minWidth = 80f;
    [SerializeField] float maxWidth = 600f;

    private float startHeight;

    private Tween tweenFade;

    private void OnDestroy()
    {
        tweenFade?.Kill();
    }

    private void Start()
    {
        startHeight = root.rect.height;
    }

    private void Resize()
    {
        // Force TMP to update its geometry
        textName.ForceMeshUpdate();

        float textWidth = textName.preferredWidth;

        textWidth = Mathf.Clamp(textWidth, minWidth, maxWidth);

        root.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            textWidth
        );

        /*
        float finalHeight = startHeight;

        if(textName.text.Length > 50)
        {
            finalHeight *= 1.3f;
        }

        root.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            finalHeight
        );*/
    }

    public void Show(string text, Vector2 position, bool fade = false)
    {
        if (!SettingsManager.Instance.AreTooltipsOn) return;

        // set pos
        transform.position = position;

        // set text
        textName.text = text;

        gameObject.SetActive(true);

        if (!fade)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            // handles fade
            if(tweenFade == null)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(1f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true);
        }

        Resize();
        FixTMPAnchors();
    }

    private void FixTMPAnchors()
    {
        RectTransform rt = textName.rectTransform;

        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void Hide(bool fade = false)
    {
        if (!fade)
        {
            canvasGroup.alpha = 0f;

            gameObject.SetActive(false);
        }
        else
        {
            // handles fade
            if (tweenFade == null)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(0f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
