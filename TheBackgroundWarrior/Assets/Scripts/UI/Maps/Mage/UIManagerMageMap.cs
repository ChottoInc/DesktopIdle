using UnityEngine;

public class UIManagerMageMap : UIManager
{
    [Space(10)]
    [SerializeField] UIPlayerMageExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
