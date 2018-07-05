using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour {

    public GameObject _shotPrefab;
    public Transform _positionSpawn = null;
    RaycastHit hit;
    public float _timeFire = 0f;
    public float _timeCadence = 0f;
    public float _rate = 0f;
    public float _damage = 0f;
    public float _dispersion = 0f;
    public int _projPershot = 0;
    public float _cadence = 2f;
    public int _numShots = 0;
    public int _numShotsTemp = 0;
    int _maxObjects = 20;
    List<GameObject> _poolBullets = new List<GameObject>();

    // MonoBehaviour -------------------------------------------------------
	void Awake()
	{
        _maxObjects = 20;

        GunConfiguration gunn = Resources.Load("gunConfiguration/" + name) as GunConfiguration;
        _rate = gunn._rate;
        _damage = gunn._damage;
        _dispersion = gunn._dispersion;
        _projPershot = gunn._projPershot;
        _cadence = gunn._cadence;
        _numShots = gunn._numShots;
	}

	void Start()
	{
        for (int i = 0; i < _maxObjects; i++)
        {
            GameObject bullet = (GameObject)Instantiate(_shotPrefab);
            bullet.SetActive(false);
            bullet.GetComponent<Proyectile>()._damage = _damage;
            _poolBullets.Add(bullet);
        }
	}

	void Update()
	{
        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        _timeFire += Time.deltaTime;
        _timeCadence += Time.deltaTime;
	}

    void OnEnable()
    {
        _timeFire = 2f;
        _timeCadence = 2f;

        EventManagerCustom.InitGame += InitGame;
    }

    void OnDisable()
    {
        EventManagerCustom.InitGame -= InitGame;
    }

    // Private --------------------------------------------------------------

    void InitGame()
    {
        foreach(GameObject ob in _poolBullets)
        {
            ob.SetActive(false);
        }
    }

    // Public ----------------------------------------------------------------

	public void Shot()
    {
        if(_timeFire >= _rate && _timeCadence >= _cadence)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f))
                {
                    _numShotsTemp++;

                    // dispersion
                    Vector3 pos = hit.point + Random.insideUnitSphere * _dispersion ; 

                    for (int i = 0; i < _poolBullets.Count; i++)
                    {
                        if(!_poolBullets[i].activeInHierarchy )
                        {
                            _poolBullets[i].SetActive(true);

                            _poolBullets[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                            _poolBullets[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            _poolBullets[i].GetComponent<SphereCollider>().enabled = true;
                            _poolBullets[i].GetComponent<Proyectile>().Run();
                            _poolBullets[i].transform.position = _positionSpawn.position;
                            _poolBullets[i].transform.rotation = Quaternion.identity;
                            _poolBullets[i].transform.LookAt(pos);
                            _poolBullets[i].GetComponent<Rigidbody>().AddForce(_poolBullets[i].transform.forward * 5000f);
                            break;
                        }
                    }

                    _timeFire = 0f;

                    if(_numShotsTemp >= _numShots )
                    {
                        _numShotsTemp = 0;
                        _timeCadence = 0f;
                    }
                }
            }
        }
    }
}
