using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOn : MonoBehaviour
{
    public bool Interacted;
    public Animator Animator;
    public Animation Animation;
    public GameObject PowerSwitch;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        Animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Interacted = PowerSwitch.GetComponent<Interaction>().Interacted;
        Debug.Log(Interacted);

        if (Interacted == true)
        {
            Animator.SetBool("Interacted", true);
        }
    }
}
