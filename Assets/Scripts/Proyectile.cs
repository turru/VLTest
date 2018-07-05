using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour {

    public ParticleSystem _particleInit;
    public ParticleSystem _particleImpact;
    public ParticleSystem _particleBullet;
    public SphereCollider _collider;
    public Rigidbody _rigidbody;
    public float _damage = 0f;

    public void Run()
    {
        _collider.enabled = true;
        _particleBullet.gameObject.GetComponent<AudioSource>().Play();
        _particleBullet.Play();
        _particleInit.Play();
    }

    void OnCollisionEnter(Collision hit)
    {
        foreach (ContactPoint contact in hit.contacts)
        {
            if (contact.otherCollider.tag == "Enemy")
            {
                EnemyController _ene = hit.gameObject.GetComponent<EnemyController>();

                if (_ene != null)
                {
                    _ene.Damage(_damage);
                }
            }
        }

        _collider.enabled = false;

        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        _rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);

        _particleImpact.Play();
        _particleImpact.gameObject.GetComponent<AudioSource>().Play();
        _particleBullet.Stop();
        _particleInit.Play();

        Invoke("Desactivate",1f);
    }

	void Desactivate()
    {
        gameObject.SetActive(false);
    }
}
