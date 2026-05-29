using UnityEngine;

public abstract class ListableGameDataSO : ScriptableObject
{
    [SerializeField] int id;

    public int Id => id;
}
