using DG.Tweening;
using UnityEngine;

public class UIScrollTriangle : MonoBehaviour
{
    [SerializeField] Vector2 _offset;
    [SerializeField] float _duration;

    private Tween _tweenMovement;

    private bool _isActive;

    private bool _isInit;

    private void Update()
    {
        InitializeIfNeeded();
    }

    private void InitializeIfNeeded()
    {
        if (_isInit) return;

        _tweenMovement = transform.DOMove((Vector2)transform.position + _offset, _duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject, LinkBehaviour.KillOnDestroy).SetUpdate(true);

        _isActive = true;

        _isInit = true;

        Pause();
    }

    public void Resume()
    {
        if (!_isInit) return;

        if (_isActive) return;

        //Debug.Log("resumed");

        gameObject.SetActive(true);
        _isActive = true;

        if (_tweenMovement == null) return;

        _tweenMovement.Play();
    }

    public void Pause()
    {
        if (!_isInit) return;

        if (!_isActive) return;

        //Debug.Log("paused");

        if (_tweenMovement == null) return;

        _tweenMovement.Pause();

        gameObject.SetActive(false);
        _isActive = false;
    }

    public void Show()
    {
        if (!_isInit) return;

        if (_isActive) return;

        gameObject.SetActive(true);
        _isActive = true;
    }

    public void Hide()
    {
        if (!_isInit) return;

        if (!_isActive) return;

        gameObject.SetActive(false);
        _isActive = false;
    }
}
