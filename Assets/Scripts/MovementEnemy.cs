using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEnemy : MonoBehaviour
{
    public EnemyDetection _detection;
    public float _speed = 0f;
    public Transform[] _pivots;
    int _indexPivot = 0;
    float _timeStep = 0f;
    int _stepsZigZag = 0;
    int _zigZagDirection = 0;
    int _randomZigZagDirection =  0;
    float maxJumpHeight = 4.0f;
    Vector3 _lastPositionGround;
    float _jumpSpeed = 12.0f;
    float _fallSpeed = 12.0f;
    bool _inputJump = false;
    Transform _transPlayer;

    public enum TYPES_MOVEMENT
    {
        SIMPLE,
        JUMPING,
        ZIGZAG,
        TITAN
    }
    public TYPES_MOVEMENT _typeMovement;

    // MonoBehaviour -------------------------------------------------------
    void Start()
    {
        Init();
        _randomZigZagDirection = Random.Range(0, 2) * 2 - 1;
    }

    void Update()
    {
        _timeStep += Time.deltaTime;


        if (_timeStep >= _speed)
        {
            ChoiceMovement();
            _timeStep = 0f;
        }
    }

    // Private --------------------------------------------------------------
	void ChoiceMovement()
    {
        LookPlayer(_transPlayer);

        if(!_detection._canMove)
        {
            return;
        }

        switch (_typeMovement)
        {
            
            case TYPES_MOVEMENT.SIMPLE:


                RotateCube();
                break;
            case TYPES_MOVEMENT.JUMPING:

                StopAllCoroutines();

                int random = Random.Range(0, 10);
                // 40% possibilities jumping
                if (random <= 3)
                {
                    Jumping();
                }
                else
                {
                    RotateCube();
                }
                break;

            case TYPES_MOVEMENT.ZIGZAG:

                if(_detection._enemiesNear)
                {
                    _stepsZigZag = 0;
                }

                if(_stepsZigZag <= 5 )
                {
                    _zigZagDirection = 0;
                }
                else  if (_stepsZigZag > 5 && _stepsZigZag <= 10)
                {
                    _zigZagDirection = _randomZigZagDirection;
                }
                else  if (_stepsZigZag > 10 && _stepsZigZag <= 15)
                {
                    _zigZagDirection = 0;
                }
                else if (_stepsZigZag > 15 && _stepsZigZag <= 20)
                {
                    _zigZagDirection = -1  * _randomZigZagDirection ;
                }
                else  if (_stepsZigZag > 10 )
                {
                    _stepsZigZag = 0;
                }

                LookLeftright(_zigZagDirection);
                RotateCube();

                _stepsZigZag++;

                break;
            case TYPES_MOVEMENT.TITAN:


                RotateCube();
                break;
            default:
              
                break;
        }

    }

    void Jumping()
    {
        _lastPositionGround = transform.position;
        _inputJump = true;

        StartCoroutine(Jump());
    }

    /// <summary>
    /// if no left, is right, Selec direction walk enemy
    /// </summary>
    /// <param name="value">if no left, is right.</param>
    void LookLeftright(int value)
    {
        if(value == -1)
        {
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y - 90f, 0f);
        }
        else if (value == 1)
        {
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 90f , 0f); 
        }
    }

    void LookPlayer(Transform transPlayer)
    {
        transform.LookAt(transPlayer.position, Vector3.up);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y  , 0f);
        _indexPivot = 0;
    }

	void  RotateCube()
    {
        StartCoroutine(RotateEdge(90f, _pivots[_indexPivot], Vector3.up));
        _indexPivot++;

        if(_indexPivot >= _pivots.Length)
        {
            _indexPivot = 0;
        }
    }

    IEnumerator RotateEdge(float degrees, Transform pivot, Vector3 axis)
    {
        int frames = 10; 
                        
        float degreesPerFrame = degrees / (float)frames;
        for (int i = 1; i <= frames; i++)
        {
            transform.RotateAround(pivot.position, pivot.right, degreesPerFrame);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Jump()
    {
        while (true)
        {
            if (transform.position.y >= maxJumpHeight)
            {
                _inputJump = false;
            }
               
            if (_inputJump)
            {
                transform.Translate(Vector3.up * _jumpSpeed * Time.deltaTime);   
            }
            else if (!_inputJump)
            {
                transform.position = Vector3.Lerp(transform.position, _lastPositionGround, _fallSpeed * Time.deltaTime);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // Public ----------------------------------------------------------------

    public void Init()
    {
        _transPlayer = GameObject.FindWithTag("Player").transform;
        LookPlayer(_transPlayer);
    }
}
