using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreezerInteracted : MonoBehaviour
{
    public bool Interacted;
    public GameObject InteractText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Interacted = gameObject.GetComponent<Interaction>().Interacted;

        if (Interacted == true)
        {
            //InteractText.SetActive(false);

            if (gameObject.tag == "Enemy")
            {
                Debug.Log("Enemy!");
                gameObject.GetComponent<EnemyAI>().enabled = true;
                gameObject.GetComponent<EnemyHP>().enabled = true;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }

            switch (gameObject.name)
            {
                case "FreezerBlue":
                    transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().materials[1].color = Color.blue;
                    break;
                case "FreezerPink":
                    transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().materials[1].color = new Color(1f, 0.5f, 0.5f);
                    break;
                case "FreezerGreen":
                    transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().materials[1].color = Color.green;
                    break;
                case "FreezerRed":
                    transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().materials[1].color = Color.red;
                    break;
                default:
                    break;
            }
        }
    }
}
