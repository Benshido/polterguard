using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public GameObject menuManager;
    public AudioSource source;
    public AudioClip clip;
    public AudioClip deathClip;
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
            source.PlayOneShot(deathClip);
            IsAlive = false;
            StartCoroutine(WaitForSound(deathClip));
        }
        else
            source.PlayOneShot(clip);
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

    public void CallDeathScreen()
    {
        menuManager.GetComponent<PauseMenu>().DeathScreen(IsAlive);
    }

    public IEnumerator WaitForSound(AudioClip playedClip)
    {
        yield return new WaitUntil(() => source.isPlaying == false);
        CallDeathScreen();
    }
}