using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField]
    private TMP_Text damageText;
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.1f, 1.5f);
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
