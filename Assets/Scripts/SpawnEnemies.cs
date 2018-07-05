using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {


    GameController _gameController;

    [HideInInspector]
    public static SpawnEnemies _instanceRef = null;

    public Transform[] _pointsSpawns;
    Collider[] _hitColliders;
    public GameObject _prefabSimple;
    public GameObject _prefabJumping;
    public GameObject _prefabZigZag;
    public GameObject _prefabTitan;

    List<GameObject> _enemiesSimple;
    List<GameObject> _enemiesJumping;
    List<GameObject> _enemiesZigZag;
    GameObject _enemiesTitan;

    public int _maxObjectsSimple = 0;
    public int _maxObjectsJumping = 0;
    public int _maxObjectsZigZag = 0;

    public int _objectsActiveSimple = 0;
    public int _objectsActiveJumping = 0;
    public int _objectsActiveZigZag = 0;

    public float _probabilitySimple = 0;
    public float _probabilityJumping = 0;
    public float _probabilityZigZag = 0;

    public float _timeSpawn = 0f;
    float _timeSpawnAux = 0f;


	void Awake()
	{
        if (_instanceRef == null)
        {
            _instanceRef = this;
        }

        _timeSpawn = 0.8f;

        _probabilitySimple = 0.3f;
        _probabilityJumping = 0.3f;
        _probabilityZigZag = 0.3f;

        _maxObjectsSimple = 15;
        _maxObjectsJumping = 15;
        _maxObjectsZigZag = 15;

        _enemiesSimple = new List<GameObject>();
        _enemiesJumping = new List<GameObject>();
        _enemiesZigZag = new List<GameObject>();
	
	}

	void Start()
	{
        _gameController = GameController._instanceRef;

        // Simple
        for (int i = 0; i < _maxObjectsSimple; i++)
        {
            GameObject eneSimple = (GameObject)Instantiate(_prefabSimple);
            eneSimple.SetActive(false);
            _enemiesSimple.Add(eneSimple);
        }

        // Jumping
        for (int i = 0; i < _maxObjectsJumping; i++)
        {
            GameObject eneSimple = (GameObject)Instantiate(_prefabJumping);
            eneSimple.SetActive(false);
            _enemiesJumping.Add(eneSimple);
        }

        // ZigZag
        for (int i = 0; i < _maxObjectsZigZag; i++)
        {
            GameObject eneSimple = (GameObject)Instantiate(_prefabZigZag);
            eneSimple.SetActive(false);
            _enemiesZigZag.Add(eneSimple);
        }

        // Titan
        _enemiesTitan = (GameObject)Instantiate(_prefabTitan);
        _enemiesTitan.SetActive(false);

	}

	void Update()
	{
        if(_gameController._state == GameController.GAME_STATE.GAMEPLAY)
        {
            _timeSpawnAux += Time.deltaTime;

            if (_timeSpawnAux >= _timeSpawn)
            {
                SpawnEnemie();
                _timeSpawnAux = 0f;
            }
        }

	}

	public void SpawnEnemie()
    {
        transform.RotateAround(transform.position, Vector3.up,15f);

        float probAuxSimple = Random.Range(0f,1f);
        float probAuxJumping = Random.Range(0f, 1f);
        float probAuxZigZag = Random.Range(0f, 1f);

        // Spawn Simple
        if(probAuxSimple <= _probabilitySimple && _maxObjectsSimple > _objectsActiveSimple)
        {
            for (int i = 0; i < _enemiesSimple.Count; i++)
            {
                if (!_enemiesSimple[i].activeInHierarchy)
                {

                    _hitColliders = Physics.OverlapSphere(_pointsSpawns[0].position, 2);
                    if (_hitColliders.Length >= 2) // 2 for , floor and object 
                    {
                        break;
                    }

                    _enemiesSimple[i].GetComponent<EnemyController>().Init();
                    _enemiesSimple[i].transform.position = new Vector3(_pointsSpawns[0].position.x, 0.5f, _pointsSpawns[0].position.z);
                    _enemiesSimple[i].transform.rotation = Quaternion.identity;
                    _objectsActiveSimple++;
                    break;
                }
            } 
        }

        // Spawn Jumping
        if (probAuxJumping <= _probabilityJumping && _maxObjectsJumping > _objectsActiveJumping)
        { 
            for (int i = 0; i < _enemiesJumping.Count; i++)
            {
                if (!_enemiesJumping[i].activeInHierarchy)
                {
                    _hitColliders = Physics.OverlapSphere(_pointsSpawns[1].position, 2);
                    if (_hitColliders.Length >= 2) // 2 for , floor and object 
                    {
                        break;
                    }

                    _enemiesJumping[i].GetComponent<EnemyController>().Init();
                    _enemiesJumping[i].transform.position = new Vector3(_pointsSpawns[1].position.x, 0.25f, _pointsSpawns[1].position.z);
                    _enemiesJumping[i].transform.rotation = Quaternion.identity;
                    _objectsActiveJumping++;
                    break;
                }
            }
        }

        // Spawn ZigZag
        if (probAuxZigZag <= _probabilityZigZag && _maxObjectsZigZag > _objectsActiveZigZag)
        {
            for (int i = 0; i < _enemiesZigZag.Count; i++)
            {
                if (!_enemiesZigZag[i].activeInHierarchy)
                {
                    _hitColliders = Physics.OverlapSphere(_pointsSpawns[2].position, 2);
                    if (_hitColliders.Length >= 2) // 2 for , floor and object 
                    {
                        break;
                    }

                    _enemiesZigZag[i].GetComponent<EnemyController>().Init();

                    _enemiesZigZag[i].transform.position = new Vector3(_pointsSpawns[2].position.x, 0.5f, _pointsSpawns[2].position.z);
                    _enemiesZigZag[i].transform.rotation = Quaternion.identity;

                    _objectsActiveZigZag++;
                    break;
                }
            }
        }
    }

    public void SpawnTitan()
    {
        if(_enemiesTitan.activeInHierarchy)
        {
            return;
        }

        _enemiesTitan.GetComponent<EnemyController>().Init();
        _enemiesTitan.transform.position = new Vector3(_pointsSpawns[3].position.x, 1.5f, _pointsSpawns[3].position.z);
        _enemiesTitan.transform.rotation = Quaternion.identity;
    }

    void InitGame()
    {
        // Simple
        foreach (GameObject ob in _enemiesSimple)
        {
            ob.SetActive(false);
        }

        // Jumping
        foreach (GameObject ob in _enemiesJumping)
        {
            ob.SetActive(false);
        }

        // ZigZag
        foreach (GameObject ob in _enemiesZigZag)
        {
            ob.SetActive(false);
        }


        // Titan
        if (_enemiesTitan != null)
        {
            _enemiesTitan.SetActive(false);
        }

        _objectsActiveSimple = 0;
        _objectsActiveJumping = 0;
        _objectsActiveZigZag = 0;
    }

    void OnEnable()
    {
        EventManagerCustom.InitGame += InitGame;
    }

    void OnDisable()
    {
        EventManagerCustom.InitGame -= InitGame;
    }


	void OnDestroy()
	{
        _instanceRef = null;
	}
}
