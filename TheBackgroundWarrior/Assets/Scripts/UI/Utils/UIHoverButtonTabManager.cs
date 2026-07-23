using UnityEngine;

public class UIHoverButtonTabManager : MonoBehaviour
{
    [SerializeField] TabManager _tabManager;

    private UIHoverButtonTab _currentHoveredButton;
    private UIHoverButtonTab _currentSelectedButton;

    private void Awake()
    {
        if( _tabManager != null)
        {
            _tabManager.OnSelectFirstTab += ResetCurrentButton;
        }
    }

    private void OnDestroy()
    {
        if (_tabManager != null)
        {
            _tabManager.OnSelectFirstTab -= ResetCurrentButton;
        }
    }

    public void ChangeHovered(UIHoverButtonTab button)
    {
        if (_currentHoveredButton != null && !_currentHoveredButton.IsSelected)
            _currentHoveredButton.Unhover();

        _currentHoveredButton = button;

        if (_currentHoveredButton != null)
            _currentHoveredButton.Hover();
    }

    public void ChangeSelected(UIHoverButtonTab button)
    {
        if (_currentSelectedButton != null)
        {
            _currentSelectedButton.Deselect();
            _currentSelectedButton.Unhover();
        }

        _currentSelectedButton = button;
    }

    private void ResetCurrentButton()
    {
        if(_currentHoveredButton != null)
            _currentHoveredButton.Unhover();

        if(_currentSelectedButton != null)
            _currentSelectedButton.Deselect();
    }
}
