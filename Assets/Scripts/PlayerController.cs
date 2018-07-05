using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public Transform _cameraTR;
    public Transform _gunTR;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    float yaw = 0.0f;
    float pitch = 0.0f;
    GunSwitching _gunSwitching = null;

    GameController _gameController;

    public Transform  _posCamView_1;
    public Transform  _posCamView_2;

    bool _camFPS = false;
    public GameObject _capsulePlayer;


    // Behaviour ---------------------------------------------------------------

    void Start()
    {
        _gameController = GameController._instanceRef;

        _gunSwitching = GetComponentInChildren<GunSwitching>();
    }

    void Update()
    {
        if(!_gameController._paused)
        {
            // shot
            if (Input.GetMouseButton(0))
            {
                SpawnShot();
            }

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");


            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
            _cameraTR.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        // shot
        if (Input.GetKeyDown("e"))
        {
            ChangeView();
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

    // Private ---------------------------------------------------------------

    void InitGame()
    {
        transform.rotation = Quaternion.identity;
        _cameraTR.rotation = Quaternion.identity;
        yaw = 0f;
        pitch = 0f; ;
    }

    void SpawnShot()
    {
        _gunSwitching._gunActive.Shot();

    }

    void ChangeView()
    {
        _camFPS = !_camFPS;

        if(_camFPS)
        {
            _capsulePlayer.SetActive(false);
            _cameraTR.position = _posCamView_1.position;
        }
        else
        {
            _capsulePlayer.SetActive(true);
            _cameraTR.position = _posCamView_2.position; 
        }
    }

}
