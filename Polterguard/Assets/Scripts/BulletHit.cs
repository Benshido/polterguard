using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            other.GetComponent<EnemyHP>().TakeAttack(20);

        if (other.gameObject.tag == "Puzzle")
        {
            other.GetComponent<PuzzleCube>().CallCheckCubes();
            other.gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
