﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float _lifeMax = 0f;
    public float _lifeCurrent = 0f;
    public int _damage = 0;
    public Color _colorInit;
    public float _h, _s, _v;
    public Renderer _renderer;
    public MovementEnemy.TYPES_MOVEMENT _typeEnemy;

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
        if(other.tag == "Player")
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
            case MovementEnemy.TYPES_MOVEMENT.SIMPLE:
                SpawnEnemies._instanceRef._objectsActiveSimple--;
                break;
            case MovementEnemy.TYPES_MOVEMENT.JUMPING:
                SpawnEnemies._instanceRef._objectsActiveJumping--;
                break;
            case MovementEnemy.TYPES_MOVEMENT.ZIGZAG:
                SpawnEnemies._instanceRef._objectsActiveZigZag--;
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