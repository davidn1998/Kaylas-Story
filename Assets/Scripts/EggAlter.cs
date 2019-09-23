using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAlter : MonoBehaviour
{

    [SerializeField] GameObject alterExplosion = null;
    [SerializeField] AudioClip eggCollected = null;
    private void Start()
    {
        if (GameManager.Instance.SummonAbility())
        {
            GetComponentInChildren<Animator>().gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Instance.SummonAbility())
        {
            MusicManager.Instance.GetComponent<AudioSource>().PlayOneShot(eggCollected);
            Instantiate(alterExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject.GetComponentInChildren<Animator>().gameObject);
            Destroy(GetComponent<Collider2D>());
            GameManager.Instance.SetSummonAbility(true);
        }
    }

}
