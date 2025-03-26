using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shot1 : MonoBehaviour
{
    public GameObject _start;
    public GameObject _shell;
    public Slider _slider;
    public float _reload = 0;
    public float _timereload = 5;
    public ParticleSystem _firevfx;
    // Start is called before the first frame update
    void Start()
    {
        _slider.value = _timereload;
    }

    public void Fier()
    {
        if (Global.ind == 1)
        {
            if (_slider.value >= _slider.maxValue)
            {
                _firevfx.Play(true);
                _slider.value = 0;
                Vector3 spawn = _start.transform.position;
                Quaternion spawn1 = _start.transform.rotation;
                GameObject shell1 = Instantiate(_shell, spawn, spawn1);
                Rigidbody fly = shell1.GetComponent<Rigidbody>();
                fly.AddForce(shell1.transform.forward * 100, ForceMode.Impulse);
                Destroy(shell1, 5);
                _reload = _timereload;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value += Time.deltaTime;
        _reload -= Time.deltaTime;
    }
}
