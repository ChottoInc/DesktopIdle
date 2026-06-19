using UnityEngine;

public class UIBasePanelFloating : MonoBehaviour
{
    [SerializeField] protected float moveTime = 0.5f;
    [SerializeField] protected Transform startContentPos;
    [SerializeField] protected Transform endContentPos;


    public float MoveTime => moveTime;
    public Transform StartContentPos => startContentPos;
    public Transform EndContentPos => endContentPos;


    [Space(10)]
    [SerializeField] protected UIAnimatedPanelFloatingValue[] objectsToMove;


    protected virtual void OnDestroy()
    {
        //damageableEntity.OnTakeDamage -= ShowDamage;
    }

    protected virtual void Awake()
    {
        //damageableEntity.OnTakeDamage += ShowDamage;
    }


    protected virtual void ShowValue(int value)
    {
        if (!SettingsManager.Instance.IsDamageOn) return;

        // setup object
        var obj = GetFirstFreeObject();

        if (obj != null)
            obj.Animate(value);
    }

    protected virtual UIAnimatedPanelFloatingValue GetFirstFreeObject()
    {
        foreach (var obj in objectsToMove)
        {
            if (!obj.IsAnimating) return obj;
        }
        return null;
    }
}
