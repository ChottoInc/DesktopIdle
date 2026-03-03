using UnityEngine;

public class UIManagerFisherMap : UIManager
{
    [SerializeField] UIPlayerFisherExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
