using UnityEngine;

public class UIManagerBlacksmithMap : UIManager
{
    [SerializeField] UIPlayerBlacksmithExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
