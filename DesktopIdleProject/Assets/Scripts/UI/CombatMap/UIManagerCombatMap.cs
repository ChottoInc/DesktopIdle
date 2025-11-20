using UnityEngine;

public class UIManagerCombatMap : MonoBehaviour
{
    [SerializeField] UIPlayerExpBar playerExpBar;

    public void Setup()
    {
        playerExpBar.Setup();
    }
}
