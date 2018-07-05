﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum GAME_STATE
    {
        MENU,
        GAMEPLAY,
        PAUSE
    }

    public GAME_STATE _state;
    [HideInInspector]
    public static GameController _instanceRef = null;
    public  UIController _uiController;
    public GameObject _prefabParticleDamage;
    List<GameObject> _listParticleDamage;
    int _lifePlayer = 20;
    public bool _paused = false;

    public int LifePlayer
    {
        get { return _lifePlayer; }
        set 
        {
            if(value <= 0)
            {
                value = 0;
                Gameover();
            }

            _lifePlayer = value; 

            EventManagerCustom.UpdateUIMehod();
        }
    }

    int _numCubesDeaths = 0;
    public int NumCubesDeaths
    {
        get { return _numCubesDeaths; }
        set
        {

            _numCubesDeaths = value;

            if(_numCubesDeaths % 5 == 0 && _numCubesDeaths!= 0)
            {
                SpawnEnemies._instanceRef.SpawnTitan();
            }

            EventManagerCustom.UpdateUIMehod();
        }
    }

    // MonoBehaviour -------------------------------------------------------
    void Awake()
    {
        if(_instanceRef == null)
        {
            _instanceRef = this;
            DontDestroyOnLoad(gameObject); 
            Init ();
            SetFrameRate (60);
        }
        else
        {
            DestroyImmediate(gameObject); 
        }
    }
        
	void Start () 
    {
        _state = GAME_STATE.MENU;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        _uiController.Menu();
        Menu();
	}

    public void Menu()
    {
        _state = GAME_STATE.MENU;
        _paused = true;
        ActionMouse(true);
        EventManagerCustom.InitGameMehod();
    }

    void ActionMouse(bool stateMouse)
    {
        if (stateMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

	public void Play()
	{
        LifePlayer = 20;
        NumCubesDeaths = 0;

        ActionMouse(true);
        _state = GAME_STATE.GAMEPLAY;
        //_paused = false;
        Paused();
	}

    public void Paused()
    {
        _paused = !_paused;

        if (_paused)
        {
            _state = GAME_STATE.PAUSE;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _uiController.Paused(true);
        }
        else
        {
            _state = GAME_STATE.GAMEPLAY;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _uiController.Paused(false);
        }
    }

	private void Update()
	{
        if(_state == GAME_STATE.GAMEPLAY)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused();
            }    
        }
	}

    // Private --------------------------------------------------------------

    void Init()
    {
        _listParticleDamage = new List<GameObject>();

        // populate particles
        for (int i = 0; i < 20; i++)
        {
            GameObject particle = (GameObject)Instantiate(_prefabParticleDamage);
            particle.SetActive(false);
            _listParticleDamage.Add(particle);
        }
    }

    void SetFrameRate(int rate)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Gameover()
    {
        _uiController.Menu();
        Menu();
    }

    // Public ----------------------------------------------------------------
	
    public void RunParticleDamage( Vector3 position)
    {
        for (int i = 0; i < _listParticleDamage.Count; i++)
        {
            if (!_listParticleDamage[i].activeInHierarchy)
            {
                _listParticleDamage[i].SetActive(true);
                _listParticleDamage[i].transform.position = position;
                _listParticleDamage[i].transform.rotation = Quaternion.identity;
                _listParticleDamage[i].GetComponent<ParticleSystem>().Play();
                break;
            }
        }
    }

   
}
