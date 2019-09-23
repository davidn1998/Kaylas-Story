using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //player respawn
    [Header("Player Respawn")]
    [SerializeField] float respawnDelay = 2f;

    //Player ability
    bool summonAbility = false;

    static GameManager instance;

    public static GameManager Instance { get { return instance; } }

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

    public void PlayerRespawn()
    {
        StartCoroutine(PlayerDie());
    }

    IEnumerator PlayerDie()
    {
        yield return new WaitForSeconds(respawnDelay);
        LevelManager.Instance.ReloadLevel();
    }

    public bool SummonAbility()
    {
        return summonAbility;
    }

    public void SetSummonAbility(bool active)
    {
        summonAbility = active;
    }

}
