using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chosetank : MonoBehaviour
{
    public Button _play;
    public GameObject[] _players;
    public int _touches = 0;
    public GameObject _onscene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ussr()
    {
        _play.interactable = true;
        _touches += 1;
        if (_touches == 1)
        {
            Global.ind = 0;
            Instantiate(_players[0], new Vector3(669, 280, -849), Quaternion.identity);
        }
        else
        {
            _onscene = GameObject.FindGameObjectWithTag("Finish");
            Destroy(_onscene);
            Instantiate(_players[0], new Vector3(669, 280, -849), Quaternion.identity);
            Global.ind = 0;
        }
    }
    public void Franch()
    {
        _play.interactable = true;
        _touches += 1;
        if (_touches == 1)
        {
            Global.ind = 1;
            Instantiate(_players[1], new Vector3(669, 280, -849), Quaternion.identity);
        }
        else
        {
            _onscene = GameObject.FindGameObjectWithTag("Finish");
            Destroy(_onscene);
            Instantiate(_players[1], new Vector3(669, 280, -849), Quaternion.identity);
            Global.ind = 1;
        }
    }
    public void Usa()
    {
        _play.interactable = true;
        _touches += 1;
        if (_touches == 1)
        {
            Global.ind = 2;
            Instantiate(_players[2], new Vector3(669, 280, -849), Quaternion.identity);
        }
        else
        {
            _onscene = GameObject.FindGameObjectWithTag("Finish");
            Destroy(_onscene);
            Instantiate(_players[2], new Vector3(669, 280, -849), Quaternion.identity);
            Global.ind = 2;
        }
    }
}
