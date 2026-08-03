using UnityEngine;

[RequireComponent(typeof(UITabWindow))]
public class UIAutoClose : MonoBehaviour
{
    private UITabWindow _tabWindow;

    private bool _isCounting;
    private float _timerClose;

    private void Awake()
    {
        _tabWindow = GetComponent<UITabWindow>();
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!SettingsManager.Instance.IsAutocloseHudOn) return;

        if(!_isCounting && _tabWindow.IsOpen)
        {
            _isCounting = true;
        }

        if(_isCounting && !_tabWindow.IsOpen)
        {
            ResetTimer();
        }

        if (_isCounting)
        {
            _timerClose -= Time.unscaledDeltaTime;

            if(_timerClose <= 0)
            {
                // close and reset timer
                if (_tabWindow.CanClose())
                {
                    _tabWindow.Close();
                    ResetTimer();
                }
            }
        }
    }

    private void ResetTimer()
    {
        _isCounting = false;
        _timerClose = UtilsGeneral.GetAutocloseTimerByType(SettingsManager.Instance.CurrentAutocloseTimerType);
    }
}
