using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ContainerGameData", fileName = "ContainerGameData_")]
public class ContainerGameDataSO : ScriptableObject
{
    [SerializeField] List<ListableGameDataSO> entries;

    public List<ListableGameDataSO> Entries => entries;
}
