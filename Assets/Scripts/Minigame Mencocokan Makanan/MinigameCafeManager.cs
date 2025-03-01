using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Handle layers in minigame scene, handle timer, handle scoring & target
public class MinigameCafeManager : Minigame
{
    [SerializeField]
    private GameObject targetPanel;
    [SerializeField] private TimerScript timer;
    [SerializeField] private int coinMultiplier;
    [SerializeField] private List<CafeCustomerSpawner> spawners;

    public override void checkScore()
    {
        if (currentScore >= targetScore)
        {
            isWin = true;
            targetPanel.GetComponent<Animator>().SetBool("targetAccomplished", true);
        }
    }

    public override int calculateCoinGained()
    {
        return currentScore * 10 * coinMultiplier;
    }

    public override void SetLevel1()
    {
        timer.SetTimerMaxValue(35f);
        targetScore = 8;
        coinMultiplier = 1;
        foreach (var spawner in spawners)
        {
            spawner.SetCustomerWaitTime(4, 6);
        }
        Init();
        SoundManager.Instance.StopMusic();
    }

    public override void SetLevel2()
    {
        timer.SetTimerMaxValue(30f);
        targetScore = 15;
        coinMultiplier = 2;
        foreach (var spawner in spawners)
        {
            spawner.SetCustomerWaitTime(3, 5);
        }
        Init();
        SoundManager.Instance.StopMusic();
    }
    public override void SetLevel3()
    {
        timer.SetTimerMaxValue(25f);
        targetScore = 15;
        coinMultiplier = 3;
        foreach (var spawner in spawners)
        {
            spawner.SetCustomerWaitTime(2, 4);
        }
        Init();
        SoundManager.Instance.StopMusic();
    }
}
