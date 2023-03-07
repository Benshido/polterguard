using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCutscene : MonoBehaviour
{

    public GameObject Player;
    public GameObject PlayerCamera;
    public GameObject CutsceneCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == true)
        {
            if (other.gameObject.tag == "Player")
            {
                Player.GetComponent<FirstPersonController>().enabled = false;
                PlayerCamera.SetActive(false);
                CutsceneCamera.SetActive(true);
                StartCoroutine(EndCutscene());
            }
        }
    }

    IEnumerator EndCutscene()
    {
        yield return new WaitForSeconds(3);
        Player.GetComponent<FirstPersonController>().enabled = true;
        PlayerCamera.SetActive(true);
        CutsceneCamera.SetActive(false);
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
