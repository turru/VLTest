using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    GameController _gameController;

    [HideInInspector]
    public static SpawnEnemies _instanceRef = null;

    public Transform[] _pointsSpawns;

    public GameObject _prefabTitan;

    [HideInInspector] public float _timeSpawn = 0f;
    float _timeSpawnAux = 0f;

    GameObject _enemiesTitan;
    Collider[] _hitColliders;

    // index 0 = Simple, index 1 = Jumping, index = 1 ZigZag
    public GameObject[] _prefabsEnemies = new GameObject[3];
    [HideInInspector] public int[] _maxObjects = new int[3];
    [HideInInspector] public int[] _objectsActive = new int[3];
    [HideInInspector] public float[] _probabilites = new float[3];
    float[] probability = new float[3];
    List<List<GameObject>> _enemiesList;


    // MonoBehaviour -------------------------------------------------------

    void Awake()
    {
        if (_instanceRef == null)
        {
            _instanceRef = this;
        }

        _timeSpawn = 0.8f;

        _enemiesList = new List<List<GameObject>>();
        for (int i = 0; i < 3; i++)
        {
            // init lists
            List<GameObject> aux = new List<GameObject>();
            _enemiesList.Add(aux);

            // init array maxObjects
            _maxObjects[i] = 15;
            _probabilites[i] = 0.3f;
        }
    }

    void Start()
    {
        _gameController = GameController._instanceRef;

        for (int i = 0; i < _maxObjects.Length ; i++)
        {
            for (int j = 0; j < _maxObjects[i]; j++)
            {
                GameObject enemyGO = (GameObject)Instantiate(_prefabsEnemies[i]);
                enemyGO.SetActive(false);
                _enemiesList[i].Add(enemyGO);
            }
        }

        // Titan
        _enemiesTitan = (GameObject)Instantiate(_prefabTitan);
        _enemiesTitan.SetActive(false);
    }

    void Update()
    {
        if (_gameController._state == GameController.GAME_STATE.GAMEPLAY)
        {
            _timeSpawnAux += Time.deltaTime;

            if (_timeSpawnAux >= _timeSpawn)
            {
                SpawnEnemie();
                _timeSpawnAux = 0f;
            }
        }
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

    // Private --------------------------------------------------------------

    /// <summary>
    /// Call spawn enemies, check if there are enemies near, if not enable enemy
    /// </summary>
    public void SpawnEnemie()
    {
        transform.RotateAround(transform.position, Vector3.up, 15f);

        for (int i = 0; i <= 2; i++)
        {
            probability[i] = Random.Range(0f, 1f);
        }

        for (int i = 0; i <= 2; i++)
        {
            if (probability[i] <= _probabilites[i] && _maxObjects[i] > _objectsActive[i])
            {
                for (int j = 0; j < _enemiesList[i].Count; j++)
                {
                    if (!_enemiesList[i][j].activeInHierarchy)
                    {
                        _hitColliders = Physics.OverlapSphere(_pointsSpawns[i].position, 2f); // detect  if gameobject near
                        if (_hitColliders.Length >= 2) // 2 for , floor and object 
                        {
                            break;
                        }

                        _enemiesList[i][j].GetComponent<EnemyController>().Init();
                        _enemiesList[i][j].transform.position = new Vector3(_pointsSpawns[i].position.x, _enemiesList[i][j].transform.localScale.y * 0.5f, _pointsSpawns[i].position.z);
                        _enemiesList[i][j].transform.rotation = Quaternion.identity;

                        _objectsActive[i]++;
                        break;
                    }
                }
            }
        }

    }

    public void SpawnTitan()
    {
        if (_enemiesTitan.activeInHierarchy)
        {
            return;
        }

        _enemiesTitan.GetComponent<EnemyController>().Init();
        _enemiesTitan.transform.position = new Vector3(_pointsSpawns[3].position.x, 1.5f, _pointsSpawns[3].position.z);
        _enemiesTitan.transform.rotation = Quaternion.identity;
    }

    void InitGame()
    {
        for (int i = 0; i <= 2; i++)
        {
            foreach (GameObject ob in _enemiesList[i])
            {
                ob.SetActive(false);
            }

            _objectsActive[i] = 0;
        }

        // Titan
        if (_enemiesTitan != null)
        {
            _enemiesTitan.SetActive(false);
        }
    }

}
