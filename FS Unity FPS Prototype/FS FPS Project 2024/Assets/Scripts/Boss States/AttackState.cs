using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IBossState
{
    int sayThisOnce = 0;
    int sayThisOnceAgain = 0;

    private bool isPullingCard = false;
    private bool secondPhaseActive = false;

    private int worldCastTime = 3;
    //private int magicianCastTime = 2;
    private int justiceCastTime;
    private int towerCastTime;

    
    private float flightSpeed = 1.5f;

    public IBossState DoState(BossSearch boss)
    {
        //while (sayThisOnce == 0)
        //{
        //    Debug.Log("Melore has entered attack state");
        //    sayThisOnce++;
        //}

        if (boss.agent == null)
        {
            boss.agent = boss.GetComponent<NavMeshAgent>();
        }

        switch (boss.hpValue)
        {
            case BossSearch.HPValue.highHP:
                //while (sayThisOnceAgain == 0)
                //{
                //    Debug.Log("Melore is above 25% HP");
                //    sayThisOnceAgain++;
                //}
                if(!isPullingCard)
                    CardPull(boss);
                break;
            case BossSearch.HPValue.quarterOrBelow:
                secondPhaseActive = true;
                Debug.Log("Melore is below 25% HP");
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
            boss.cardDeck.Clear();
            for (int i = 0; i < 10; i++)
            {
                boss.cardDeck.Add(boss.Card_TheWorld);
                boss.cardDeck.Add(boss.Card_TheMagician); 
            }
            boss.cardDeck.Add(boss.Card_Justice);
            boss.cardDeck.Add(boss.Card_Justice);

            int randCard = Random.Range(0, boss.cardDeck.Count);

            GameObject cardSelected = boss.cardDeck[randCard];
            cardSelected.SetActive(true);

            if (cardSelected != null)
            {
                if (cardSelected == boss.Card_TheWorld)
                {
                    Debug.Log("Selected The World");
                    Shuffle(boss);
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
                //clear deck
                boss.cardDeck.Clear();
            }
            //foreach(var card in boss.cardDeck)
            //{
            //    Debug.Log(card.name);
            //}
            cardSelected.SetActive(false);
        }
        else
        {
            boss.cardDeck.Clear();

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
            boss.cardDeck.Clear();
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

    private void Shuffle(BossSearch boss)
    {
        boss.StartCoroutine(CycleCards(boss));
    }

    IEnumerator ExecuteTheWorld(BossSearch boss)
    {
        Debug.Log("Melore is executing: The World");
        
        //boss.ExplosionUIOne.SetActive(true);
        //boss.ExplosionUITwo.SetActive(true);
        //boss.ExplosionUIThree.SetActive(true);
        //boss.ExplosionUIFour.SetActive(true);
        //boss.ExplosionUIFive.SetActive(true);

        boss.BossCenterPOS.SetActive(true);
        boss.BossRightPOS.SetActive(true);
        boss.BossLeftPOS.SetActive(true);
        boss.FarRightPOS.SetActive(true);
        boss.FarLeftPOS.SetActive(true);

        boss.UICenter.SetActive(true);
        boss.UIBossRight.SetActive(true);
        boss.UIBossLeft.SetActive(true);
        boss.UIFarRight.SetActive(true);
        boss.UIFarLeft.SetActive(true);

        yield return new WaitForSeconds(worldCastTime);

        boss.UICenter.SetActive(false);
        boss.UIBossRight.SetActive(false);
        boss.UIBossLeft.SetActive(false);
        boss.UIFarRight.SetActive(false);
        boss.UIFarLeft.SetActive(false);

        yield return new WaitForSeconds(.5f);

        boss.ExplosionCenter.SetActive(true);
        boss.ExplosionBossRight.SetActive(true);
        boss.ExplosionBossLeft.SetActive(true);
        boss.ExplosionFarRight.SetActive(true);
        boss.ExplosionFarLeft.SetActive(true);        

        yield return new WaitForSeconds(1);

        boss.CenterTrigger.enabled = true;
        boss.BossRightTrigger.enabled = true;
        boss.BossLeftTrigger.enabled = true;
        boss.FarRightTrigger.enabled = true;
        boss.FarLeftTrigger.enabled = true;

        yield return new WaitForSeconds(.5f);

        boss.ExplosionCenter.SetActive(false);
        boss.ExplosionBossRight.SetActive(false);
        boss.ExplosionBossLeft.SetActive(false);
        boss.ExplosionFarRight.SetActive(false);
        boss.ExplosionFarLeft.SetActive(false);

        boss.CenterTrigger.enabled = false;
        boss.BossRightTrigger.enabled = false;
        boss.BossLeftTrigger.enabled = false;
        boss.FarRightTrigger.enabled = false;
        boss.FarLeftTrigger.enabled = false;

        boss.BossCenterPOS.SetActive(false);
        boss.BossRightPOS.SetActive(false);
        boss.BossLeftPOS.SetActive(false);
        boss.FarRightPOS.SetActive(false);
        boss.FarLeftPOS.SetActive(false);

        yield return new WaitForSeconds(5);

        isPullingCard = false;
    }

    IEnumerator ExecuteTheMagician(BossSearch boss)
    {
        Debug.Log("Melore is executing: The Magician");

        boss.auraUnderBoss.SetActive(true);
        

        boss.auraList.Add(boss.aura1);
        boss.auraList.Add(boss.aura2);
        boss.auraList.Add(boss.aura3);
        boss.auraList.Add(boss.aura4);
        boss.auraList.Add(boss.aura5);

        for (int i = 0; i < boss.auraList.Count; i++)
        {
            boss.auraList[i].SetActive(true);
            yield return new WaitForSeconds(1);
        }

        

        yield return new WaitForSeconds(3);

        

        boss.auraUnderBoss.SetActive(false);
        isPullingCard = false;
    }

    

    IEnumerator ExecuteJustice(BossSearch boss)
    {
        yield return new WaitForSeconds(2);

        boss.LArmHitBox.enabled = false;
        boss.RArmHitBox.enabled = false;
        boss.bodyHitBox.enabled = false;

        boss.justiceObj.SetActive(true);
        boss.justiceTrigger.resetSpawner();
        yield return new WaitForSeconds(1f);
        boss.justiceObj.SetActive(false);

        float HPtoHeal = boss.maxHP * .02f;

        if(boss.HP + HPtoHeal <= boss.maxHP) 
        {
            boss.HP += HPtoHeal;

            boss.body.material.color = Color.green;
            boss.LArm.material.color = Color.green;
            boss.RArm.material.color = Color.green;

            yield return new WaitForSeconds(0.1f);

            boss.body.material.color = Color.white;
            boss.LArm.material.color = Color.white;
            boss.RArm.material.color = Color.white;

            yield return new WaitForSeconds(2.9f);
        }
        else
        {
            yield return new WaitForSeconds(2.9f);
        }
        if(boss.HP + HPtoHeal <= boss.maxHP) 
        {
            boss.HP += HPtoHeal;

            boss.body.material.color = Color.green;
            boss.LArm.material.color = Color.green;
            boss.RArm.material.color = Color.green;

            yield return new WaitForSeconds(0.1f);

            boss.body.material.color = Color.white;
            boss.LArm.material.color = Color.white;
            boss.RArm.material.color = Color.white;

            yield return new WaitForSeconds(2.9f);
        }
        else
        {
            yield return new WaitForSeconds(2.9f);
        }
        if (boss.HP + HPtoHeal <= boss.maxHP) 
        {
            boss.HP += HPtoHeal;

            boss.body.material.color = Color.green;
            boss.LArm.material.color = Color.green;
            boss.RArm.material.color = Color.green;

            yield return new WaitForSeconds(0.1f);

            boss.body.material.color = Color.white;
            boss.LArm.material.color = Color.white;
            boss.RArm.material.color = Color.white;

            yield return new WaitForSeconds(2.9f);
        }
        else
        {
            yield return new WaitForSeconds(2.9f);
        }
        if (boss.HP + HPtoHeal <= boss.maxHP) 
        {
            boss.HP += HPtoHeal;

            boss.body.material.color = Color.green;
            boss.LArm.material.color = Color.green;
            boss.RArm.material.color = Color.green;

            yield return new WaitForSeconds(0.1f);

            boss.body.material.color = Color.white;
            boss.LArm.material.color = Color.white;
            boss.RArm.material.color = Color.white;

            yield return new WaitForSeconds(2.9f);
        }
        else
        {
            yield return new WaitForSeconds(2.9f);
        }
        if (boss.HP + HPtoHeal <= boss.maxHP) 
        {
            boss.HP += HPtoHeal;

            boss.body.material.color = Color.green;
            boss.LArm.material.color = Color.green;
            boss.RArm.material.color = Color.green;

            yield return new WaitForSeconds(0.1f);

            boss.body.material.color = Color.white;
            boss.LArm.material.color = Color.white;
            boss.RArm.material.color = Color.white;

            yield return new WaitForSeconds(2.9f);
        }
        else
        {
            yield return new WaitForSeconds(2.9f);
        }

        

        boss.LArmHitBox.enabled = true;
        boss.RArmHitBox.enabled = true;
        boss.bodyHitBox.enabled = true;

        isPullingCard = false;
    }



    IEnumerator CycleCards(BossSearch boss)
    {
        boss.cardDeck.Add(boss.Card_TheWorld);
        boss.cardDeck.Add(boss.Card_TheMagician);
        boss.cardDeck.Add(boss.Card_Justice);
        boss.cardDeck.Add(boss.Card_TheTower);

        for (int i = 0; i < 5; i++)
        {
            int randCard = Random.Range(0, boss.cardDeck.Count);

            boss.cardDeck[randCard].SetActive(true);
            yield return new WaitForSeconds(.75f);
            boss.cardDeck[randCard].SetActive(false); 
        }

        boss.cardDeck.Clear();

        yield return new WaitForSeconds(2);
    }

}
