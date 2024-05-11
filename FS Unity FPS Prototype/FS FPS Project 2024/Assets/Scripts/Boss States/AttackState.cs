using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IBossState
{
    int sayThisOnce = 0;
    int sayThisOnceAgain = 0;

    private bool isPullingCard = false;
    private bool secondPhaseActive = false;

    public IBossState DoState(BossSearch boss)
    {
        while (sayThisOnce == 0)
        {
            Debug.Log("Melore has entered attack state");
            sayThisOnce++;
        }

        if (boss.agent == null)
        {
            boss.agent = boss.GetComponent<NavMeshAgent>();
        }

        switch (boss.hpValue)
        {
            case BossSearch.HPValue.highHP:
                while (sayThisOnceAgain == 0)
                {
                    Debug.Log("Melore is above 25% HP");
                    sayThisOnceAgain++;
                }
                if(!isPullingCard)
                    CardPull(boss);
                break;
            case BossSearch.HPValue.quarterOrBelow:
                secondPhaseActive = true;
                Debug.Log("Melore is above 25% HP");
                if (!isPullingCard)
                    AttackPatternTwo();
                break;
        }

        if (!boss.playerTarget.activeSelf)
        {
            return boss.idleState;
        }
        else
        {
            return boss.attackState;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void CardPull(BossSearch boss)
    {
        isPullingCard = true;
        Debug.Log("Melore is pulling a card.");
        if (!secondPhaseActive)
        {
            boss.cardDeck.Add(boss.Card_TheWorld);
            boss.cardDeck.Add(boss.Card_TheMagician);
            boss.cardDeck.Add(boss.Card_Justice);

            int randCard = Random.Range(0, boss.cardDeck.Count);

            GameObject cardSelected = boss.cardDeck[randCard];


            if (cardSelected != null)
            {
                if (cardSelected == boss.Card_TheWorld)
                {
                    Debug.Log("Selected The World");
                    TheWorld(boss);

                }
                else if (cardSelected == boss.Card_TheMagician)
                {
                    Debug.Log("Selected The Magician");
                    TheMagician(boss);

                }
                else
                {
                    Debug.Log("Selected Justice");
                    Justice(boss);
                }
            }
            //foreach(var card in boss.cardDeck)
            //{
            //    Debug.Log(card.name);
            //}
        }
        else
        {
            boss.cardDeck.Add(boss.Card_TheWorld);
            boss.cardDeck.Add(boss.Card_TheMagician);
            boss.cardDeck.Add(boss.Card_Justice);
            boss.cardDeck.Add(boss.Card_TheTower);

            int randCard = Random.Range(0, boss.cardDeck.Count);

            GameObject cardSelected = boss.cardDeck[randCard];
            cardSelected.SetActive(true);

            if (cardSelected != null)
            {
                if (cardSelected == boss.Card_TheWorld)
                {
                    Debug.Log("Selected The World");
                    TheWorld(boss);
                }
                else if (cardSelected == boss.Card_TheMagician)
                {
                    Debug.Log("Selected The Magician");
                }
                else if (cardSelected == boss.Card_Justice)
                {
                    Debug.Log("Selected Justice");
                }
                else
                {
                    Debug.Log("Selected The Tower");
                }
            }

            cardSelected.SetActive(false);
        }
    }

    private void TheWorld(BossSearch boss)
    {
        boss.StartCoroutine(ExecuteTheWorld(boss));
    }
    
    private void TheMagician(BossSearch boss)
    {
        boss.StartCoroutine(ExecuteTheMagician(boss));
    }
    
    private void Justice(BossSearch boss)
    {
        boss.StartCoroutine(ExecuteJustice(boss));
    }

    private void AttackPatternTwo()
    {
        Debug.Log("Boss is below half HP");
    }

    IEnumerator ExecuteTheWorld(BossSearch boss)
    {
        Debug.Log("Melore is executing: The World");

        boss.WorldLocationOne.SetActive(true);
        boss.WorldLocationTwo.SetActive(true);
        boss.WorldLocationThree.SetActive(true);
        boss.WorldLocationFour.SetActive(true);
        boss.WorldLocationFive.SetActive(true);

        //Make UI Elements appear on the ground to show danger spots
        //after a cooldown, deal damage in those triggers
        //turn off triggers
        //return to loop

        yield return new WaitForSeconds(3);

        isPullingCard = false;
        boss.WorldLocationOne.SetActive(false);
        boss.WorldLocationTwo.SetActive(false);
        boss.WorldLocationThree.SetActive(false);
        boss.WorldLocationFour.SetActive(false);
        boss.WorldLocationFive.SetActive(false);

    }

    IEnumerator ExecuteTheMagician(BossSearch boss)
    {
        yield return new WaitForSeconds(3);

        Debug.Log("Melore is executing: The Magician");

        isPullingCard = false;
    }

    IEnumerator ExecuteJustice(BossSearch boss)
    {
        yield return new WaitForSeconds(3);

        Debug.Log("Melore is executing: Justice");

        isPullingCard = false;
    }
}
