using UnityEngine;

public class UIManagerMinerMap : UIManager
{
    [SerializeField] UIPlayerMinerExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
