using UnityEngine;

public abstract class ListableGameDataSO : ScriptableObject
{
    [SerializeField] protected int id;

    public virtual int Id => id;
}
