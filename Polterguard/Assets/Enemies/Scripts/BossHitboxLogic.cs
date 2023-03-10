using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitboxLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage = 30;
    [SerializeField] private int lifetime = 5; // in frames
    private int currentLife = 0;
    [SerializeField] private float speed = 3; //units/s
    [SerializeField] private Transform parentpos;
    void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(other.gameObject.tag == "Player")
            {
                other.GetComponentInChildren<PlayerHP>().TakeDamage(damage);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currentLife++;
        parentpos.Translate(Vector3.forward * speed, parentpos);
        if(currentLife == lifetime)
        {
            Destroy(gameObject);
        }
    }
}
