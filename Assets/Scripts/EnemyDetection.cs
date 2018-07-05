using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

    Transform _myTransform = null;
    public float _radius = 0f;
    public LayerMask targetMask;
    public bool _canMove = false;
    public bool _enemiesNear = false;
    public Transform _playertransform;
    float _distMin;
    float _disThisToPlauyer;
    Collider[] _hitColliders;

    // MonoBehaviour -------------------------------------------------------

	void Start () {

        _playertransform = GameObject.FindWithTag("Player").transform;

       _myTransform = GetComponent<Transform>();;

        InvokeRepeating("Detection", 0f,0.25f);
	}
	
    // Private --------------------------------------------------------------

    void Detection()
    {
        _distMin = 1000f;
        _disThisToPlauyer = Vector3.Distance(_myTransform.position, _playertransform.position);
        _hitColliders = Physics.OverlapSphere(_myTransform.position, _radius,targetMask );

        int i = 0;
        while (i < _hitColliders.Length)
        {
            if (_hitColliders[i].transform != _myTransform)
            {
                _enemiesNear = true;

                float _disAux = Vector3.Distance(_hitColliders[i].transform.position, _playertransform.position);

                if(_disAux < _distMin)
                {
                    _distMin = _disAux;
                }
            }
            i++;
        }

        if (_disThisToPlauyer < _distMin)
        {
            _canMove = true;
        }
        else
        {
            _canMove = false;
        }

        if(_hitColliders.Length <= 1)
        {
            _canMove = true;
            _enemiesNear = false;
        }

    }

    // Debug ----------------------------------------------------------------

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, _radius);
    //}
}
