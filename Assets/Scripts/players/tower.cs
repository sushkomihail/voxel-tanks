using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject _tanktower;
    public float _rotate = 50.0f;
    public TouchField _touch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rot = (_rotate * Time.deltaTime * 5 * _touch._dist.x) / 20;
        _tanktower.transform.Rotate(0, rot, 0);

    }
    
}
