using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerSwitch : MonoBehaviour
{
    public bool Interacted;
    public Animator Animator;
    public GameObject Switch;
    public GameObject InteractText;
    public GameObject ExitLevelTrigger;
    public GameObject SoundEffect;
    public GameObject LightsNoise;
    public TMP_Text CurrentObjective;
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
            ExitLevelTrigger.SetActive(true);
            SoundEffect.SetActive(true);
            LightsNoise.SetActive(false);
            CurrentObjective.GetComponent<TextMeshProUGUI>().text = "Go to Main Floor";
        }
    }
}
