using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called before the first frame update

    public float interactive = 0;
    public bool Interacted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("touch");
            interactive = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactive = 0;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactive == 1)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("interacted");
                Interacted = true;
            }
        }
    }
}
