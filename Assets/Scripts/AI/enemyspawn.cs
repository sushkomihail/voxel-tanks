using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemyspawn : MonoBehaviour
{
    public GameObject[] _objects;
    public GameObject _tt;
    public GameObject _lt;
    public GameObject _pt;
    public GameObject[] _spawn;
    // Start is called before the first frame update
    void Start()
    {
        if (Global.ind == 0)
        {
            _lt.SetActive(true);
        }
        if (Global.ind == 1)
        {
            _pt.SetActive(true);
        }
        if (Global.ind == 2)
        {
            _tt.SetActive(true);
        }
        //Instantiate(players[global.ind], new Vector3(45, 0.55f, -40), Quaternion.identity);
        for (int i = 0; i < _objects.Length; i++)
        {
            Instantiate(_objects[i], _spawn[i].transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
