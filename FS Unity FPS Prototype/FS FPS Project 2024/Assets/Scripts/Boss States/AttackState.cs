using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IBossState
{
    private bool isPullingCard = false;
    private bool secondPhaseActive = false;

    private int worldCastTime = 3;

    public IBossState DoState(BossSearch boss)
    {
        if (boss.agent == null)
        {
            boss.agent = boss.GetComponent<NavMeshAgent>();
        }

        switch (boss.hpValue)
        {
            case BossSearch.HPValue.highHP:
                if (!isPullingCard)
                    CardPull(boss);
                break;
            case BossSearch.HPValue.quarterOrBelow:
                Debug.Log("Melore is below 25% HP");
                if (!isPullingCard)
                    CardPull(boss);
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

    private void CardPull(BossSearch boss)
    {
        isPullingCard = true;

        boss.cardDeck.Clear();
        for (int i = 0; i < 14; i++)
        {
            boss.cardDeck.Add(boss.Card_TheWorld);
            boss.cardDeck.Add(boss.Card_TheMagician);
        }
        for (int i = 0; i < 6; i++)
        {
            boss.cardDeck.Add(boss.Card_Justice);
        }

        int randCard = Random.Range(0, boss.cardDeck.Count);

        GameObject cardSelected = boss.cardDeck[randCard];
        cardSelected.SetActive(true);

        if (cardSelected != null)
        {
            if (cardSelected == boss.Card_TheWorld)
            {
                TheWorld(boss);
            }
            else if (cardSelected == boss.Card_TheMagician)
            {
                TheMagician(boss);
            }
            else
            {
                Justice(boss);
            }
            boss.cardDeck.Clear();
        }
        cardSelected.SetActive(false);
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

    IEnumerator ExecuteTheWorld(BossSearch boss)
    {
        boss.anim.SetTrigger("TheWorld");

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
        boss.anim.SetTrigger("TheMagician");

        boss.auraUnderBoss.SetActive(true);

        boss.auraList.Clear();
        boss.auraList.Add(boss.aura1);
        boss.auraList.Add(boss.aura2);
        boss.auraList.Add(boss.aura3);
        boss.auraList.Add(boss.aura4);
        boss.auraList.Add(boss.aura5);

        boss.shootPosList.Clear();
        boss.shootPosList.Add(boss.magiShootPOS1);
        boss.shootPosList.Add(boss.magiShootPOS2);
        boss.shootPosList.Add(boss.magiShootPOS3);
        boss.shootPosList.Add(boss.magiShootPOS4);
        boss.shootPosList.Add(boss.magiShootPOS5);

        for (int i = 0; i < boss.auraList.Count; i++)
        {
            boss.auraList[i].SetActive(true);
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < boss.auraList.Count; i++)
        {
            boss.shootPos = boss.shootPosList[i];
            FaceTarget(boss, boss.shootPosList[i]);
            boss.InstantiateBullet(boss.shootPos.transform.position);
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < boss.auraList.Count; i++)
        {
            boss.auraList[i].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2);

        boss.auraUnderBoss.SetActive(false);
        isPullingCard = false;
    }

    bool canSeePlayer(BossSearch boss, GameObject shootPos)
    {
        boss.playerDir = gameManager.instance.player.transform.position - shootPos.transform.position;

        RaycastHit hit;

        if (Physics.Raycast(shootPos.transform.position, boss.playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                FaceTarget(boss, shootPos);
            }
            return true;
        }
        return false;
    }

    void FaceTarget(BossSearch boss, GameObject shootPos)
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(boss.playerDir.x, shootPos.transform.position.y, boss.playerDir.x));
        shootPos.transform.rotation = Quaternion.Lerp(shootPos.transform.rotation, rot, Time.deltaTime);
    }

    IEnumerator ExecuteJustice(BossSearch boss)
    {
        boss.anim.SetTrigger("Justice");

        //boss.healAura.SetActive(true);
        boss.justiceObj.SetActive(true);
        boss.justiceTrigger.resetSpawner();
        yield return new WaitForSeconds(1f);
        boss.justiceObj.SetActive(false);

        float HPtoHeal = boss.maxHP * .02f;
        for (int i = 0; i < 5; i++)
        {
            if (boss.HP + HPtoHeal <= boss.maxHP)
            {
            
                boss.HP += HPtoHeal;

                boss.UpdateEnemyUI();

                boss.healAura.SetActive(true);
                boss.hitbox.enabled = false;
                
                yield return new WaitForSeconds(1f);

                boss.healAura.SetActive(false);
                boss.hitbox.enabled = true;

                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
        }
        
        //boss.healAura.SetActive(false);
        isPullingCard = false;
    }
}
