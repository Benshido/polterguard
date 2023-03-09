using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : MonoBehaviour
{
    // Start is called before the first frame update

    public float interactive = 0;
    public bool Interacted = false;
    public GameObject InteractText;
    public GameObject Keycard1;
    public GameObject Keycard2;
    public GameObject Keycard3;
    public float KeycardsCollected1;
    public float KeycardsCollected2;
    public float KeycardsCollected3;
    public Animator Animator;


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
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        KeycardsCollected1 = Keycard1.GetComponent<InteractKeycardsFloor1>().KeycardsCollected;
        KeycardsCollected2 = Keycard2.GetComponent<InteractKeycardsFloor1>().KeycardsCollected;
        KeycardsCollected3 = Keycard3.GetComponent<InteractKeycardsFloor1>().KeycardsCollected;
        if (interactive == 1)
        {
            if (Input.GetKeyDown(KeyCode.F) && KeycardsCollected1 == 1 && KeycardsCollected2 == 1 && KeycardsCollected3 == 1)
            {
                Interacted = true;
                InteractText.SetActive(false);
                Animator.enabled = true;

            }
        }
    }
}
