using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorPuzzleManager : MonoBehaviour
{
    public GameObject Access;
    public GameObject NoAccess;
    List<string> colorSolution = new List<string>();
    Transform parentTransform;
    int solution = 0;
    bool fourAttempts = false;

    // Start is called before the first frame update
    void Start()
    {
        solution = 0;
        NoAccess.SetActive(true);
        Access.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (colorSolution.Count >= 4 && !fourAttempts)
        {
            fourAttempts = true;
            Debug.Log("4 inactive");
            CheckSolution();
        }
    }

    public void CheckCubes(string color)
    {
        colorSolution.Add(color);
    }

    private void CheckSolution()
    {
        parentTransform = gameObject.transform;

        // Loop through each child object
        for (int i = 0; i < colorSolution.Count; i++)
        {
            if (colorSolution[i] == "Blue" || colorSolution[i] == "Green" || colorSolution[i] == "Red" || colorSolution[i] == "Pink")
            {
                Debug.Log("correct cube!" + solution);
                solution++; 
            }
        }

        //Check if the solution was found
        if (solution == 4)
        {
            Debug.Log("WELL DONE!");
            NoAccess.SetActive(false);
            Access.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("INCORRECT CODE" + solution);
            solution = 0;
            colorSolution.Clear();
            fourAttempts = false;

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                // Get a reference to the current child object's transform
                Transform childTransform = parentTransform.GetChild(i);
                childTransform.gameObject.SetActive(true);
            }
        }
    }
}
