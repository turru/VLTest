using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    SpawnEnemies _spawnerEnemies;

    public Text _texLife;
    public Text _texDeaths;

    public Slider _sliderProSimple;
    public Slider _sliderProJumping;
    public Slider _sliderProZigZag;
    public Slider _sliderNumSimple;
    public Slider _sliderNumJumping;
    public Slider _sliderNumZigZag;

    public Slider _sliderInterval;

    public GameObject _panelMenu;
    public GameObject _panelOptions;

    [HideInInspector]
    public static UIController _instanceRef;

    // MonoBehaviour -------------------------------------------------------
	void Awake()
	{
        _instanceRef = this;
	}

	void Start () {
        _spawnerEnemies = GameObject.FindObjectOfType<SpawnEnemies>();

        UpdateUI();

        _sliderProSimple.value = _spawnerEnemies._probabilites[0];
        _sliderProJumping.value = _spawnerEnemies._probabilites[1];
        _sliderProZigZag.value = _spawnerEnemies._probabilites[2];

        _sliderNumSimple.value = _spawnerEnemies._maxObjects[0];
        _sliderNumJumping.value = _spawnerEnemies._maxObjects[1];
        _sliderNumZigZag.value = _spawnerEnemies._maxObjects[2];

        _sliderInterval.value = _spawnerEnemies._timeSpawn;
	}
	
    void OnEnable()
    {
        EventManagerCustom.UpdateUI += UpdateUI;
    }

    void OnDisable()
    {
        EventManagerCustom.UpdateUI -= UpdateUI;
    }

    void OnDestroy()
    {
        _instanceRef = null;
    }

	void UpdateUI () 
    {
        GameController gameController = GameController._instanceRef;
        _texLife.text   = gameController.LifePlayer.ToString();
        _texDeaths.text = gameController.NumCubesDeaths.ToString();
	}

    // Publics --------------------------------------------------------------

    public void SetProSimple(Slider slider)
    {
        _spawnerEnemies._probabilites[0] = slider.value;
    }

    public void SetProJumping(Slider slider)
    {
        _spawnerEnemies._probabilites[1] = slider.value;
    }

    public void SetProZigZag(Slider slider)
    {
        _spawnerEnemies._probabilites[2] = slider.value;
    }

    public void SetNumSimple (Slider slider)
    {
        _spawnerEnemies._maxObjects[0] = (int)slider.value;
    }

    public void SetNumJumping(Slider slider)
    {
        _spawnerEnemies._maxObjects[1] = (int)slider.value;
    }

    public void SetNumZigZag (Slider slider)
    {
        _spawnerEnemies._maxObjects[2] = (int)slider.value;
    }

    public void SetIntervalSpawn(Slider slider)
    {
        _spawnerEnemies._timeSpawn = slider.value;
    }

    public void Paused(bool paused)
    {
        if(paused)
        {
            _panelOptions.SetActive(true);
            _panelMenu.SetActive(false);
        }else
        {
            _panelOptions.SetActive(false);
            _panelMenu.SetActive(false);
        }
    }

    public void Menu()
    {
        _panelMenu.SetActive(true);
        _panelOptions.SetActive(false);
        GameController._instanceRef.Menu();

    }

    public void Play()
    {
        _panelMenu.SetActive(false);
        _panelOptions.SetActive(false);
        GameController._instanceRef.Play();
    }

    public void Resume()
    {
        _panelMenu.SetActive(false);
        _panelOptions.SetActive(false);
        GameController._instanceRef.Paused();
    }



	
}
