using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI levelTitle = null;
    [SerializeField] TextMeshProUGUI startButton = null;

    private void Start()
    {
        StartCoroutine(DisplayTitle());
    }

    IEnumerator DisplayTitle()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 7) {
            yield return new WaitForSeconds(0.5f);
            levelTitle.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
        }
        else {
            yield return new WaitForSeconds(0.5f);
            levelTitle.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            levelTitle.gameObject.SetActive(false);
        }
    }

    public void hideMenu() {
        levelTitle.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }
}
