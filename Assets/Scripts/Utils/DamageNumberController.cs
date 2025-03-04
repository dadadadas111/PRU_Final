using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController instance;
    public DamageNumber damageNumberPrefab;
 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    public void CreateNumber(Vector3 position, int damage)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab, position, Quaternion.identity, transform);
        damageNumber.SetDamage(damage);
    }
}
