using UnityEngine;

public class UITabWindow : MonoBehaviour
{
    protected bool isOpen;

    public virtual void Open()
    {
        isOpen = true;

        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        isOpen = false;

        gameObject.SetActive(false);
    }
}
