using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube : MonoBehaviour
{
    public GameObject puzzle;

    public void CallCheckCubes()
    {
        puzzle.GetComponent<SecondFloorPuzzleManager>().CheckCubes(gameObject.name);
    }
}
