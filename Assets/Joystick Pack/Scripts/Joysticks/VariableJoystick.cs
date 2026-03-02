using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return _moveThreshold; } set { _moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float _moveThreshold = 1;
    [SerializeField] private JoystickType _joystickType = JoystickType.Fixed;

    private Vector2 _fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        _joystickType = joystickType;
        if(joystickType == JoystickType.Fixed)
        {
            _background.anchoredPosition = _fixedPosition;
            _background.gameObject.SetActive(true);
        }
        else
            _background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        _fixedPosition = _background.anchoredPosition;
        SetMode(_joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(_joystickType != JoystickType.Fixed)
        {
            _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            _background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if(_joystickType != JoystickType.Fixed)
            _background.gameObject.SetActive(false);

        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (_joystickType == JoystickType.Dynamic && magnitude > _moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - _moveThreshold) * radius;
            _background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }