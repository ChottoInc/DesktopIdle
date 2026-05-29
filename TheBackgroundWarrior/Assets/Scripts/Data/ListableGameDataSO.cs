using UnityEngine;

public abstract class ListableGameDataSO : ScriptableObject
{
    [SerializeField] int id;

    public virtual int Id => id;
}
