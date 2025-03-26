using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float _speed = 2;
    public float _rotate = 10;
    public float _rotatetower = 40;
    public Rigidbody _rb;
    public float _reload = 0;
    public float _timereload = 3;
    public float _timerotate = 18;
    public float _timemove = 10;
    public float _maxdist = 30;
    public int _vec = -1;
    public bool _canmove = true;
    public bool _canshoot = false;
    public bool _invis = true;
    public GameObject _tower;
    public GameObject _target;
    public GameObject _start;
    public GameObject _shell;
    public float _hp = 5000;
    public Slider _slider;
    public float _speedBullet;
    public ParticleSystem _firevfx;
    public ParticleSystem _expl;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _rb.GetComponent<Rigidbody>();
        _slider.value = _hp;
    }
    void Rotation()
    {
        transform.Rotate(Vector3.up * _rotate * Time.deltaTime * _vec);
        _timerotate -= Time.deltaTime;
    }
    void Move()
    {
        _rb.transform.Translate(Vector3.forward * _speed * (Time.deltaTime));
    }

    void Rotetetoplayer()
    {
        Vector3 rotvec = _target.transform.position - _tower.transform.position;
        rotvec.y = 0;
        if (rotvec == Vector3.zero) return;
        _tower.transform.rotation = Quaternion.RotateTowards(_tower.transform.rotation, Quaternion.LookRotation(rotvec, Vector3.up), _rotatetower * Time.deltaTime);
    }

    void Fier()
    {
        _firevfx.Play(true);
        Vector3 spawn = _start.transform.position;
        Quaternion spawn1 = _start.transform.rotation;
        GameObject shell1 = Instantiate(_shell, spawn, spawn1);
        Rigidbody fly = shell1.GetComponent<Rigidbody>();
        fly.AddForce(-shell1.transform.forward * _speedBullet, ForceMode.Impulse);
        Destroy(shell1, 5);
    }

    void Update()
    {
        RaycastHit hit;
        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (Physics.Raycast(_rb.transform.position, _target.transform.position - _rb.transform.position, out hit))
        { 
            if (hit.transform == _target.transform || dist <= _maxdist)
            {
                _canshoot = true;
                _invis = false;
                Rotetetoplayer();
            }
            else
            {
                _canshoot = false;
                _invis = true;
            }
        }    
            

        if (_canshoot && _reload <= 0)
        {
            _speedBullet = dist / (Mathf.Sqrt((2 * 0.25f) / 9.81f));
            Fier();
            _reload = _timereload; 
        }
        _reload -= Time.deltaTime;   

        if (_timemove > 0 && _canmove && _invis)
        {
            Move();
            _timemove -= Time.deltaTime;
            if (_timemove <= 0)
            {
                _timemove = 10 * 1.5f;
                _canmove = false;
                _vec *= -1;
            }
        }

        if (!_canmove && _timerotate > 0 && _invis)
        {
            Rotation();
            _timerotate -= Time.deltaTime;
            if (_timerotate <= 0)
            {
                _canmove = true;
                _timerotate = 18;
            }
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "shellttal")
        {
            _hp = _hp - 300f;
            _slider.value = _hp;
            if (_hp <= 0)
            {
                _expl.Play(true);
                Destroy(gameObject, 0.5f);
                Destroy(_slider);
            }
        }
        if (other.tag == "shellltal")
        {
            _hp = _hp - 200f;
            _slider.value = _hp;
            if (_hp <= 0)
            {
                _expl.Play(true);
                Destroy(gameObject, 0.5f);
                Destroy(_slider);
            }
        }
        if (other.tag == "shellptal")
        {
            _hp = _hp - 650f;
            _slider.value = _hp;
            if (_hp <= 0)
            {
                _expl.Play(true);
                Destroy(gameObject, 0.5f);
                Destroy(_slider);
            }
        }
    }
}
