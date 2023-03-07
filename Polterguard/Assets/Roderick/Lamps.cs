using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lamps : MonoBehaviour
{
    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            gameObject.GetComponent<Light>().intensity = Random.Range(5, 20);
        }
        else
        {
            gameObject.GetComponent<Light>().intensity = 20;
        }
    }
}
