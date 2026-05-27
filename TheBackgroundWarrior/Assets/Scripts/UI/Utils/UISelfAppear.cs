using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UISelfAppear : MonoBehaviour
{
    [SerializeField] bool autoStart = true;
    [SerializeField] float timeFade = 1f;

    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (autoStart)
        {
            Appear();
        }
    }

    public void Appear()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;

        group.DOFade(1, timeFade).SetEase(Ease.InOutSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        });
    }
}
