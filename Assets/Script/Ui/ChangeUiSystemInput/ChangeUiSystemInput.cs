using UnityEngine;
using UnityEngine.UI;

public class ChangeUiSystemInput : MonoBehaviour
{
    [SerializeField] private GameObject _blackPanel;
    [SerializeField] private GameObject _panelKeyboard;
    [SerializeField] private GameObject _panelGamepad;
    [SerializeField] private Slider _slider;

    public void ShowChangeSystemInput()
    {
        switch (_slider.value)
        {
            case 0:
                _panelGamepad.SetActive(false);
                _panelKeyboard.SetActive(true);
                break;
            case 1:
                _panelGamepad.SetActive(true);
                _panelKeyboard.SetActive(false);
                break;
        }
        _blackPanel.SetActive(false);
    }
}
