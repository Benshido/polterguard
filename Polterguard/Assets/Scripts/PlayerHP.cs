using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] float maxHitPoints = 100;
    [SerializeField] Slider hpSlider;

    private float hitPoints = 1;

    public bool IsAlive { get; private set; }

    void Start()
    {
        hitPoints = maxHitPoints;
        hpSlider.value = hitPoints;

        IsAlive = true;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        hpSlider.value = hitPoints;
        if (hitPoints <= 0)
        {
            IsAlive = false;
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player Hit");
            TakeDamage(20f);
            //collision.gameObject.GetComponent<PlayerHP>().TakeDamage(20f);
        }
    }
}
