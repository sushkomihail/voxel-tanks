using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shot2 : MonoBehaviour
{
    public GameObject _start;
    public GameObject _shell;
    public float _hp = 1300f;
    public Slider _slider;
    public float _reload = 0;
    public float _timereload = 1;
    public ParticleSystem _firevfx;
    public Button _fireButton;
    // Start is called before the first frame update
    void Start()
    {
        _slider.maxValue = _timereload;
        _slider.value = _timereload;
    }

    public void Fier()
    {
        if (Global.ind == 0)
        {
            if (_slider.value >= _slider.maxValue)
            {
                _firevfx.Play(true);
                _slider.value = 0;
                Vector3 spawn = _start.transform.position;
                Quaternion spawn1 = _start.transform.rotation;
                GameObject shell1 = Instantiate(_shell, spawn, spawn1);
                Rigidbody fly = shell1.GetComponent<Rigidbody>();
                fly.AddForce(shell1.transform.forward * 200, ForceMode.Impulse);
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
