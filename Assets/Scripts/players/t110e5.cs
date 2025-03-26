using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class T110E5 : MonoBehaviour
{
    public float _speed = 5.0f;
    public float _rotate = 50.0f;
    public bool _moving;
    public bool _rotation;
    public Rigidbody _tank;
    public float _hp = 2500f;
    public Slider _slider;
    public Joystick _joystick;
    public RectTransform _handle;
    // Start is called before the first frame update
    void Start()
    {
        _tank.GetComponent<Rigidbody>();
        _slider.maxValue = _hp;
        _slider.value = _hp;
        _handle = _handle.GetComponent<RectTransform>();
    }

    void Move()
    {
        // moving = Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("down") || Input.GetKey("s");
        // rotation = Input.GetKey("left") || Input.GetKey("a") || Input.GetKey("right") || Input.GetKey("d");
        // if (handle.localPosition.y < -50)
        // {
        //     transform.Rotate(Vector3.up * (-joystick.Horizontal) * rotate * Time.deltaTime);
        // }
        // else
        // {
        //     transform.Rotate(Vector3.up * joystick.Horizontal * rotate * Time.deltaTime);
        // }
        // tank.transform.Translate(Vector3.forward * speed * joystick.Vertical * Time.deltaTime);
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        _slider.value = _hp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "shelllev")
        {
            _hp = _hp - 1000f;
            _slider.value = _hp;
            if (_hp <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (other.tag == "shellguard")
        {
            _hp = _hp - 200f;
            _slider.value = _hp;
            if (_hp <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

