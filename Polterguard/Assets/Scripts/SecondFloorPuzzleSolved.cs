using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorPuzzleSolved : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Load Next Level!");
        }
    }
}
