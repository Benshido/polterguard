using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called before the first frame update

    public float interactive = 0;
    public bool Interacted = false;
    public GameObject InteractText;
    public Animator animator;


    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == true)
        {
            if (other.gameObject.tag == "Player")
            {
                interactive = 1;
                InteractText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactive = 0;
            InteractText.SetActive(false);
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
                animator.enabled = true;
                Interacted = true;
                InteractText.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
