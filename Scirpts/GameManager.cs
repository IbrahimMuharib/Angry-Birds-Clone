using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private float EndGameCheckWaitTimer = 5f;
    [SerializeField] private GameObject restartScreen;
    [SerializeField] private SlingShotHandler slingShotHandler;
    public int MaxNumberOfShots = 3;
    private int usedNumberOfShots = 0;
    private IconHandler iconHandler;
    private List<Piggie> piggies = new List<Piggie>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        iconHandler = FindObjectOfType<IconHandler>();
        Piggie[] pigs = FindObjectsOfType<Piggie>();
        for (int i = 0; i < pigs.Length; i++)
        {
            piggies.Add(pigs[i]);
        }
    }
    public void UseShot()
    {
        usedNumberOfShots++;
        iconHandler.UseShot(usedNumberOfShots);

        CheckForLastShot();
    }

    public bool canShoot()
    {
        return usedNumberOfShots < MaxNumberOfShots;
    }

    private void CheckForLastShot()
    {
        if (usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterTime());
        }
    }

    private IEnumerator CheckAfterTime()
    {
        yield return new WaitForSeconds(EndGameCheckWaitTimer);

        if (piggies.Count == 0)
        {
            Win();
        }
        else
        {
            Restart();
        }
    }

    public void RemovePiggie(Piggie piggie)
    {
        piggies.Remove(piggie);
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (piggies.Count == 0)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("WIN");
        restartScreen.SetActive(true);
        slingShotHandler.enabled = false;
    }
    public void Restart()
    {
        Debug.Log("RESTARTING");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
