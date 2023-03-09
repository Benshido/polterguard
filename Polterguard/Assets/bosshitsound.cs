using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosshitsound : MonoBehaviour
{
    [SerializeField] public AudioSource playerlistener;
    [SerializeField] public AudioClip hitsound;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        playerlistener.PlayOneShot(hitsound);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
