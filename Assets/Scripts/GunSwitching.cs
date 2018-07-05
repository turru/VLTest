using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitching : MonoBehaviour {

    public int _selectedGun = 0;
    int prevSelectGun = 0;
    public Gun _gunActive = null;

    // MonoBehaviour -------------------------------------------------------

    void Start()
    {
        SelectGun();
    }
	
	void Update () {

        prevSelectGun = _selectedGun;


        if(Input.GetKeyDown("1"))
        {
            _selectedGun = 0; 
        }

        if (Input.GetKeyDown("2"))
        {
            _selectedGun = 1;
        }

        if (Input.GetKeyDown("3"))
        {
            _selectedGun = 2;
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_selectedGun >= transform.childCount - 1)
            {
                _selectedGun = 0;   
            }else 
            {
                _selectedGun++;  
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_selectedGun <= 0)
            {
                _selectedGun = transform.childCount - 1;
            }
            else
            {
                _selectedGun--;
            }
        }

        if(prevSelectGun != _selectedGun)
        {
            SelectGun();
        }
	}

    // Private --------------------------------------------------------------

    void SelectGun()
    {
        int i = 0;

        foreach(Transform gun in transform )
        {
            if(i == _selectedGun)
            {
                gun.gameObject.SetActive(true);
                _gunActive = gun.GetComponent<Gun>();
            }
            else
            {
                gun.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
