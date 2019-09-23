using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{

    [SerializeField] int connectedScene = 0;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            LevelManager.Instance.LoadLevel(connectedScene);
        }
    }

}
