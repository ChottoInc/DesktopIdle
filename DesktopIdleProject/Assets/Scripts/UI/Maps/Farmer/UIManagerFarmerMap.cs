using UnityEngine;

public class UIManagerFarmerMap : UIManager
{
    [SerializeField] UIPlayerFarmerExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
