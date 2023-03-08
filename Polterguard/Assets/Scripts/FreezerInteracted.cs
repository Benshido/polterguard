using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerInteracted : MonoBehaviour
{
    public bool Interacted;
    public Animator Animator;
    public GameObject Switch;
    public GameObject InteractText;
    public GameObject ExitLevelTrigger;
    public GameObject SoundEffect;
    public GameObject LightsNoise;
    // Start is called before the first frame update
    void Start()
    {
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
            SoundEffect.SetActive(true);
        }
    }
}
