using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    
	private void OnEnable()
	{
        Invoke("Desactivate",2f);
	}

    void Desactivate()
    {
        gameObject.SetActive(false);
    }
}
