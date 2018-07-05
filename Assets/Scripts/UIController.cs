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

	void Awake()
	{
        _instanceRef = this;
	}


	// Use this for initialization
	void Start () {
        _spawnerEnemies = GameObject.FindObjectOfType<SpawnEnemies>();

        UpdateUI();

        _sliderProSimple.value = _spawnerEnemies._probabilitySimple;
        _sliderProJumping.value = _spawnerEnemies._probabilityJumping;
        _sliderProZigZag.value = _spawnerEnemies._probabilityZigZag;

        _sliderNumSimple.value = _spawnerEnemies._maxObjectsSimple;
        _sliderNumJumping.value = _spawnerEnemies._maxObjectsJumping;
        _sliderNumZigZag.value = _spawnerEnemies._maxObjectsZigZag;

        _sliderInterval.value = _spawnerEnemies._timeSpawn;
	}
	

	void UpdateUI () 
    {
        GameController gameController = GameController._instanceRef;
        _texLife.text   = gameController.LifePlayer.ToString();
        _texDeaths.text = gameController.NumCubesDeaths.ToString();
	}

    public void SetProSimple(Slider slider)
    {
        _spawnerEnemies._probabilitySimple = slider.value;
    }

    public void SetProJumping(Slider slider)
    {
        _spawnerEnemies._probabilityJumping = slider.value;
    }

    public void SetProZigZag(Slider slider)
    {
        _spawnerEnemies._probabilityZigZag = slider.value;
    }

    public void SetNumSimple (Slider slider)
    {
        _spawnerEnemies._maxObjectsSimple = (int)slider.value;
    }

    public void SetNumJumping(Slider slider)
    {
        _spawnerEnemies._maxObjectsJumping = (int)slider.value;
    }

    public void SetNumZigZag (Slider slider)
    {
        _spawnerEnemies._maxObjectsZigZag = (int)slider.value;
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
}
