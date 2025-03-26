using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    public float _speed = 5.0f;
    public NavMeshAgent _enemy;
    public GameObject _target;
    public float _mindist = 15;
    public float _maxdist = 50;
    public float _speedBullet = 100;
    public float _rotatetower = 40;
    public float _rotatebody = 30;
    public float _reload = 0;
    public float _hp = 4400;
    public float _timereload = 3;
    public GameObject _tower1;
    public GameObject _tower2;
    public GameObject _start1;
    public GameObject _start2;
    public GameObject _shell;
    public Slider _slider;
    public bool _canshoot = false;
    public ParticleSystem _expl;

    void Movetoplayer()
    {
        _enemy.SetDestination(_target.transform.position);
    }

    void Fier()
    {
        Vector3 spawn = _start1.transform.position;
        Quaternion spawn1 = _start1.transform.rotation;
        GameObject shell1 = Instantiate(_shell, spawn, spawn1);
        Rigidbody fly = shell1.GetComponent<Rigidbody>();
        fly.AddForce(-shell1.transform.forward * _speedBullet, ForceMode.Impulse);
        Destroy(shell1, 5);

        Vector3 spawn3 = _start2.transform.position;
        Quaternion spawn2 = _start2.transform.rotation;
        GameObject shell2 = Instantiate(_shell, spawn3, spawn2);
        Rigidbody fly1 = shell2.GetComponent<Rigidbody>();
        fly1.AddForce(-shell2.transform.forward * _speedBullet, ForceMode.Impulse);
        Destroy(shell2, 5);
    }
    void Rotatetotarget()
    {
        Vector3 rotvec = _target.transform.position - _tower1.transform.position;
        rotvec.y = 0;
        if (rotvec == Vector3.zero) return;
        _tower1.transform.rotation = Quaternion.RotateTowards(_tower1.transform.rotation, Quaternion.LookRotation(rotvec, Vector3.up), _rotatetower * Time.deltaTime);
        Vector3 rotvec1 = _target.transform.position - _tower2.transform.position;
        rotvec1.y = 0;
        if (rotvec1 == Vector3.zero) return;
        _tower2.transform.rotation = Quaternion.RotateTowards(_tower2.transform.rotation, Quaternion.LookRotation(rotvec1, Vector3.up), _rotatetower * Time.deltaTime);
    }

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _enemy = GetComponent<NavMeshAgent>();
        _slider.value = _hp;
    }

    void Update()
    {
        RaycastHit hit;
        float dist = Vector3.Distance(_enemy.transform.position, _target.transform.position);
        if (Physics.Raycast(_enemy.transform.position, _target.transform.position - _enemy.transform.position, out hit, _maxdist))
        {
            if (dist >= _mindist && dist <= _maxdist && hit.transform == _target.transform)
            {
                Movetoplayer();
                

            }

            if (dist <= _maxdist)
            {
                _canshoot = true;
            }
            else
            {
                _canshoot = false;
            }

            Rotatetotarget();
            if (_reload <= 0 && _canshoot)
                {
                    Fier();
                    _reload = _timereload;
                }
                _reload -= Time.deltaTime;
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
