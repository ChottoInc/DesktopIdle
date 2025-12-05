using UnityEngine;

public class UIManagerCombatMap : UIManager
{
    [SerializeField] UIPlayerFightExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
