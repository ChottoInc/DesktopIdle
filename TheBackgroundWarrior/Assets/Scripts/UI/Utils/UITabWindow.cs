using System;
using UnityEngine;

public class UITabWindow : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    public event Action OnTabClose;

    public virtual void Open()
    {
        IsOpen = true;

        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        IsOpen = false;

        gameObject.SetActive(false);

        OnTabClose?.Invoke();
    }

    public virtual bool CanClose()
    {
        return true;
    }
}
