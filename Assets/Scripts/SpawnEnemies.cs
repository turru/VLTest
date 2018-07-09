using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    GameController _gameController;


    [HideInInspector] public static SpawnEnemies _instanceRef = null;

    [SerializeField] private Transform[] _pointsSpawns; // 4 pointsSpawn, 0 enemy Simple, 1 enemy Jumping, 2 enemy ZigZag, 3 Enemy Titan
    [SerializeField] private GameObject _prefabTitan;

    [HideInInspector] public float _timeSpawn = 0f;
    float _timeSpawnAux = 0f;

    GameObject _enemiyTitan;
    Collider[] _hitColliders;

    // index 0 = Simple, index 1 = Jumping, index = 1 ZigZag
    public GameObject[] _prefabsEnemies = new GameObject[3];
    [HideInInspector] public int[] _maxObjects = new int[3];
    [HideInInspector] public int[] _objectsActive = new int[3];
    [HideInInspector] public float[] _probabilites = new float[3];
    float[] _probabilityValueCurrent = new float[3];
    List<List<GameObject>> _enemiesList;
    float _radiusDetectionSphere = 2f;


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
        _enemiyTitan = (GameObject)Instantiate(_prefabTitan);
        _enemiyTitan.SetActive(false);
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
        if (_enemiyTitan != null)
        {
            _enemiyTitan.SetActive(false);
        }
    }

    // Public --------------------------------------------------------------

    /// <summary>
    /// Call spawn enemies, check if there are enemies near, if not enable enemy
    /// </summary>
    public void SpawnEnemie()
    {
        transform.RotateAround(transform.position, Vector3.up, 21f);

        for (int i = 0; i <= 2; i++)
        {
            _probabilityValueCurrent[i] = Random.Range(0f, 1f);
        }

        for (int i = 0; i <= 2; i++)
        {
            if (_probabilityValueCurrent[i] <= _probabilites[i] && _maxObjects[i] > _objectsActive[i])
            {
                for (int j = 0; j < _enemiesList[i].Count; j++)
                {
                    if (!_enemiesList[i][j].activeInHierarchy)
                    {

                        // If a enemy near no spawn
                        if(CheckEnemyNear(_pointsSpawns[i].position, _radiusDetectionSphere))
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

    /// <summary>
    /// Spawns the titan.
    /// </summary>
    public void SpawnTitan()
    {
        if (_enemiyTitan.activeInHierarchy)
        {
            return;
        }

        // If a enemy near no spawn
        if (CheckEnemyNear(_pointsSpawns[3].position, _radiusDetectionSphere))
        {
            return;
        }

        _enemiyTitan.GetComponent<EnemyController>().Init();
        _enemiyTitan.transform.position = new Vector3(_pointsSpawns[3].position.x, _enemiyTitan.transform.localScale.y * 0.5f, _pointsSpawns[3].position.z);
        _enemiyTitan.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Checks the enemy near.
    /// </summary>
    /// <returns><c>true</c>, if enemy near was checked, <c>false</c> otherwise.</returns>
    /// <param name="position">Position of Sphere.</param>
    /// <param name="radius">Radius Sphere.</param>
    bool CheckEnemyNear(Vector3 position, float radius)
    {
        bool result = false;

        // If a enemy near no spawn
        _hitColliders = Physics.OverlapSphere(position, radius); // detect a sphere raycast  if gameobject near
        if (_hitColliders.Length >= 2) // 2 for , floor and object 
        {
            result = true;
        }


        return result;
    }

}
