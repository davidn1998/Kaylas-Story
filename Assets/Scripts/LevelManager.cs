using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //panel animator
    [Header("Animation")]
    Animator transitionAnim;

    //SFX
    [Header("SFX")]
    [SerializeField] AudioClip transitionSound = null;
    [SerializeField] AudioClip warpSound = null;

    static LevelManager instance;

    public static LevelManager Instance { get { return instance;  } }

    private void Awake()
    {
        Singleton();   
    }

    private void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


    //LOADING FUNCTIONS

    //reload level
    public void ReloadLevel()
    {
        StartCoroutine(ReloadScene());
    }

    //load level index
    public void LoadLevel(int scene)
    {
        StartCoroutine(LoadScene(scene));
    }


    //LOADING COROUTINES

    private IEnumerator LoadScene(int scene)
    {
        transitionAnim = GameObject.FindGameObjectWithTag("transition").GetComponent<Animator>();
        transitionAnim.SetTrigger("End");
        MusicManager.Instance.PlaySingle(warpSound);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator ReloadScene()
    {
        transitionAnim = GameObject.FindGameObjectWithTag("transition").GetComponent<Animator>();
        transitionAnim.SetTrigger("End");
        MusicManager.Instance.RandomiseSfx(transitionSound);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnLevelWasLoaded(int level)
    {
        MusicManager.Instance.RandomiseSfx(transitionSound);
    }

}
