using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 _dist;
    public Vector2 _old;
    public int _id;
    public bool _pressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        _id = eventData.pointerId;
        _old = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (pressed)
        // {
        //     if (id >= 0 && id < Input.touches.Length)
        //     {
        //         dist = Input.touches[id].position - old;
        //         old = Input.touches[id].position;
        //     }
        //     else
        //     {
        //         dist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - old;
        //         old = Input.mousePosition;
        //     }
        // }
        // else 
        // {
        //     dist = Vector2.zero;
        //
        // }
    }
}
