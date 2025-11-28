using UnityEngine;

public class UIManagerCombatMap : UIManager
{
    [SerializeField] UIPlayerExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
