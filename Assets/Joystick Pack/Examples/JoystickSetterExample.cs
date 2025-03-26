using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSetterExample : MonoBehaviour
{
    public VariableJoystick _variableJoystick;
    public Text _valueText;
    public Image _background;
    public Sprite[] _axisSprites;

    public void ModeChanged(int index)
    {
        switch(index)
        {
            case 0:
                _variableJoystick.SetMode(JoystickType.Fixed);
                break;
            case 1:
                _variableJoystick.SetMode(JoystickType.Floating);
                break;
            case 2:
                _variableJoystick.SetMode(JoystickType.Dynamic);
                break;
            default:
                break;
        }     
    }

    public void AxisChanged(int index)
    {
        switch (index)
        {
            case 0:
                _variableJoystick.AxisOptions = AxisOptions.Both;
                _background.sprite = _axisSprites[index];
                break;
            case 1:
                _variableJoystick.AxisOptions = AxisOptions.Horizontal;
                _background.sprite = _axisSprites[index];
                break;
            case 2:
                _variableJoystick.AxisOptions = AxisOptions.Vertical;
                _background.sprite = _axisSprites[index];
                break;
            default:
                break;
        }
    }

    public void SnapX(bool value)
    {
        _variableJoystick.SnapX = value;
    }

    public void SnapY(bool value)
    {
        _variableJoystick.SnapY = value;
    }

    private void Update()
    {
        _valueText.text = "Current Value: " + _variableJoystick.Direction;
    }
}