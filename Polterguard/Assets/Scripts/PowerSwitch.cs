using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : MonoBehaviour
{
    public bool Interacted;
    public Animator Animator;
    public GameObject Switch;
    public GameObject InteractText;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Interacted);
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Interacted = gameObject.GetComponent<Interaction>().Interacted;

        if (Interacted == true)
        {
            Animator.enabled = true;
            Switch.GetComponent<Interaction>().enabled = false;
            InteractText.SetActive(false);
        }
    }
}
