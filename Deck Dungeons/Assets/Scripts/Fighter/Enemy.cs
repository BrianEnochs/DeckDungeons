using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace BK
{
    public class Enemy : MonoBehaviour
    {
        public List<EnemyAction> enemyActions;
        public List<EnemyAction> turns = new List<EnemyAction>();
        public int turnNumber;
        public bool shuffleActions;
        public Fighter thisEnemy;

        [Header("UI")]
        public Image intentIcon;
        public TMP_Text intentAmount;

        [Header("Specifics")]
        public bool bird;
        public bool elite;
        public bool nob;
        public bool wiggler;
        public GameObject wigglerBuff;
        public GameObject nobBuff;
        CombatManager cm;
        Fighter player;
        public bool midTurn;

        private void Start()
        {
            cm = FindObjectOfType<CombatManager>();
            player = cm.player;
            thisEnemy = GetComponent<Fighter>();
        }
        private void LoadEnemy()
        {
            cm = FindObjectOfType<CombatManager>();
            player = cm.player;
            thisEnemy = GetComponent<Fighter>();

            if (shuffleActions)
                GenerateTurns();
        }
        public void TakeTurn()
        {

            switch (turns[turnNumber].intentType)
            {
                case EnemyAction.IntentType.Attack:
                    StartCoroutine(AttackPlayer());
                    break;
                case EnemyAction.IntentType.Block:
                    PerformBlock();
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.StrategicBuff:
                    ApplyBuffToSelf(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.StrategicDebuff:
                    ApplyDebuffToPlayer(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.AttackDebuff:
                    ApplyDebuffToPlayer(turns[turnNumber].buffType);
                    StartCoroutine(AttackPlayer());
                    break;
                case EnemyAction.IntentType.AttackBlock:
                    PerformBlock();
                    StartCoroutine(AttackPlayer());
                    break;
                case EnemyAction.IntentType.BuffBlock:
                    PerformBlock();
                    ApplyBuffToSelf(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                default:
                    Debug.Log("Enemy TakeTurn broke");
                    break;
            }
        }
        public void GenerateTurns()
        {
            foreach (EnemyAction eA in enemyActions)
            {
                for (int i = 0; i < eA.chance; i++)
                {
                    turns.Add(eA);
                }
            }
            turns.Shuffle();
        }
        private IEnumerator AttackPlayer()
        {

            int totalDamage = turns[turnNumber].damage + thisEnemy.strength.buffValue;
            if (thisEnemy.weak.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 0.75f);
            }
            if (player.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }

            yield return new WaitForSeconds(0.5f);
            player.TakeDamage(totalDamage);
            yield return new WaitForSeconds(0.5f);
            WrapUpTurn();
        }
        private IEnumerator ApplyBuff()
        {
            yield return new WaitForSeconds(1f);
            WrapUpTurn();
        }
        private void WrapUpTurn()
        {
            turnNumber++;
            if (turnNumber == turns.Count)
                turnNumber = 0;

            if (bird)
                turnNumber = 1;

            if (nob && turnNumber == 0)
                turnNumber = 1;

            thisEnemy.EvaluateBuffsAtTurnEnd();
            midTurn = false;
        }
        private void ApplyBuffToSelf(Buff.Type t)
        {
            thisEnemy.AddBuff(t, turns[turnNumber].damage);
        }
        private void ApplyDebuffToPlayer(Buff.Type t)
        {
            if (player == null)
                LoadEnemy();

            player.AddBuff(t, turns[turnNumber].debuffAmount);
        }
        private void PerformBlock()
        {
            thisEnemy.AddBlock(turns[turnNumber].block);
        }
        public void DisplayIntent()
        {
            
            if (turns.Count == 0)
                LoadEnemy();

            intentIcon.sprite = turns[turnNumber].icon;

            if (turns[turnNumber].intentType == EnemyAction.IntentType.Attack || turns[turnNumber].intentType == EnemyAction.IntentType.AttackDebuff || turns[turnNumber].intentType == EnemyAction.IntentType.AttackBlock)
            {
                int totalDamage = turns[turnNumber].damage + thisEnemy.strength.buffValue;
                if (thisEnemy.weak.buffValue > 0)
                {
                    totalDamage = (int)(totalDamage * 0.75f);
                }
                if (player.vulnerable.buffValue > 0)
                {
                    totalDamage = (int)(totalDamage * 1.5f);
                }
                intentAmount.text = totalDamage.ToString();
            }
            else
                intentAmount.text = "";
        }
        public void CurlUP()
        {
            wigglerBuff.SetActive(false);
            thisEnemy.AddBlock(12);
        }
    }
}