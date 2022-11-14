using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private float healthModifier;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool isMine;
    
    private GameObject _myTank;
    private bool _tankTrigger;

    private void Start()
    {
        Destroy (gameObject, lifeTime);
    }
    private void OnTriggerEnter (Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (!_tankTrigger)
        {
            _myTank = other.gameObject;
            _tankTrigger = true;
        }
   
        Debug.Log("Changing player health");
        var targetHealth = other.GetComponent<Complete.TankHealth>();
        if (isMine)
        {
            if (_myTank == other.gameObject) return;
            targetHealth.TakeDamage(healthModifier);
            Destroy(this.gameObject);
        }
        else
        {
            targetHealth.TakeDamage(healthModifier);
            Destroy(this.gameObject);
        }
    }
}
