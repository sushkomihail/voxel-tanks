using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public float _hpbrick = 3.0f;
    public GameObject _block;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "shellltal" || other.tag == "shellptal" || other.tag == "shellttal")
        {
            _hpbrick--;
            if (_hpbrick <= 0)
            {
                Destroy(_block);
            }
        }
        if (other.tag == "shelllten" || other.tag == "shellpten" || other.tag == "shelltten")
        {
            _hpbrick--;
            if (_hpbrick <= 0)
            {
                Destroy(_block);
            }
        }
    }
}
