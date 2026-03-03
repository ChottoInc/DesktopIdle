using UnityEngine;

public class UIButtonTabQuests : MonoBehaviour
{
    [SerializeField] UITab tabButton;
    [SerializeField] GameObject notificationObj;


    private void OnDestroy()
    {
        QuestManager.Instance.OnNeedNotication -= EnableNotification;
        tabButton.OnDeselected -= DisableNotification;
    }

    private void Awake()
    {
        QuestManager.Instance.OnNeedNotication += EnableNotification;
        tabButton.OnDeselected += DisableNotification;
    }

    public void EnableNotification() 
    {
        notificationObj.SetActive(true);
    }

    public void DisableNotification()
    {
        notificationObj.SetActive(false);
    }
}
