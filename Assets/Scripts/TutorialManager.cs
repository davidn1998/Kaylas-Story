using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] Animator popUp = null;
    [SerializeField] string tutorial = "JumpTutorial";
    [SerializeField] GameObject previousTutorial = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DisplayTutorial();
        }
    }

    void DisplayTutorial()
    {

        popUp.gameObject.transform.position = new Vector2(transform.position.x, popUp.gameObject.transform.position.y);

            if (previousTutorial)
            {
                previousTutorial.SetActive(false);
            }

            if (tutorial == "JumpTutorial")
            {
                popUp.gameObject.SetActive(true);
            }
            else
            {
                popUp.gameObject.SetActive(true);
                popUp.SetBool(tutorial, true);
            }
    }

}
