using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    // all variables are marked as "[SerializeField] private" to debug in the inspector
    // but its scope is private
    [Header("Private variables")]
    [SerializeField] private float _lifeMax = 0f;
    [SerializeField] private float _lifeCurrent = 0f;
    [SerializeField] private int _damage = 0;
    [SerializeField] private Color _colorInit;
    [SerializeField] private float _h, _s, _v;
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private MovementEnemy.TYPES_ENEMY _typeEnemy ;

    // MonoBehaviour -------------------------------------------------------

	void Awake()
	{
        _lifeCurrent = _lifeMax;

	}

	void Start()
	{
        _colorInit = _renderer.material.color;
        Color.RGBToHSV(_colorInit, out _h, out _s, out _v);
        Init();
	}

	void OnTriggerEnter(Collider other)
	{
        if(other.tag == Constants.SRTING_TAG_PLAYER)
        {
            GameController._instanceRef.LifePlayer -= _damage;
            Death();
        }
	}

	void Update()
	{
        _renderer.material.color = Color.HSVToRGB(_h, _s, _v);
	}

    // Private -------------------------------------------------------

	public void Init()
	{
        gameObject.SetActive(true);
        _lifeCurrent = _lifeMax;
        _s = _lifeCurrent / _lifeMax;
        Color.RGBToHSV(_colorInit, out _h, out _s, out _v);
        _renderer.material.color = Color.HSVToRGB(_h, _s, _v);
	}

    void Death()
    {
        GameController gameController = GameController._instanceRef;

        gameController.RunParticleDamage(transform.position);
        gameController.NumCubesDeaths++;

        UpdateEnemiesActive();

        gameObject.SetActive(false);
        transform.position = new Vector3(1000f, 0f, 0f);
    }

    void UpdateEnemiesActive()
    {
        switch (_typeEnemy)
        {
            case MovementEnemy.TYPES_ENEMY.SIMPLE:
                SpawnEnemies._instanceRef._objectsActive[0]--;
                break;
            case MovementEnemy.TYPES_ENEMY.JUMPING:
                SpawnEnemies._instanceRef._objectsActive[1]--;
                break;
            case MovementEnemy.TYPES_ENEMY.ZIGZAG:
                SpawnEnemies._instanceRef._objectsActive[2]--;
                break;
            default:
                break;
        }
    }

    // Public -------------------------------------------------------

    public void Damage(float damage)
    {
        _lifeCurrent -= damage;
        if(_lifeCurrent <= 0f)
        {
            _lifeCurrent = 0;
        }

        _s = _lifeCurrent / _lifeMax;
        _renderer.material.color = Color.HSVToRGB(_h, _s, _v);

        if (_lifeCurrent <= 0f)
        {
            Death();
        }
    }
}
