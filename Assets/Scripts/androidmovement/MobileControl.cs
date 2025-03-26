using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileControl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image _joysticBg;
    public Image _joystic;
    public Vector2 _input;

    void Start()
    {
        _joysticBg = GetComponent<Image>();
        _joystic = transform.GetChild(0).GetComponent<Image>();
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        _input = Vector2.zero;
        _joystic.rectTransform.anchoredPosition = Vector2.zero;

    }
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joysticBg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / _joysticBg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / _joysticBg.rectTransform.sizeDelta.x);
            _input = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            _input = (_input.magnitude > 1.0f) ? _input.normalized : _input;
            _joystic.rectTransform.anchoredPosition = new Vector2(_input.x * (_joysticBg.rectTransform.sizeDelta.x / 2), _input.y * (_joysticBg.rectTransform.sizeDelta.y / 2));

        }
    }
    // public float Horizontal()
    // {
    //     if (input.x != 0) return input.x;
    //     else return Input.GetAxis("Horizontal");
    // }
    public bool Vertical1()
    {
        if (_input.y > 0 || _input.y < 0) return true;
        else return false;
    }
    public bool Vertical2()
    {
        if (_input.y < 0) return true;
        else return false;
    }
    public bool Horiz1()
    {
        if ((_input.x < 0 || _input.x > 0) && Mathf.Abs(_input.x) > Mathf.Abs(_input.y)) return true;
        else return false;
    }
}
