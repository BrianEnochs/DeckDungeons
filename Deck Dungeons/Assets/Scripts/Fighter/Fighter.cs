using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    public class Fighter : MonoBehaviour
    {
        public int currentHealth;
        public int maxHealth;
        public int currentBlock = 0;
        public FighterHealthBar fighterHealthBar;

        [Header("Buffs")]
        public Buff vulnerable;
        public Buff weak;
        public Buff strength;
        public Buff ritual;
        public Buff enrage;
        public GameObject buffPrefab;
        public Transform buffParent;
        public bool isPlayer;
        Enemy enemy;
        public CombatManager cm;
        GameManager gameManager;
        public GameObject damageIndicator;

        public void Awake()
        {
            enemy = GetComponent<Enemy>();
            cm = FindObjectOfType<CombatManager>();
            gameManager = FindObjectOfType<GameManager>();

            currentHealth = maxHealth;
            fighterHealthBar.healthSlider.maxValue = maxHealth;
            fighterHealthBar.DisplayHealth(currentHealth);

        }
        public void TakeDamage(int amount)
        {
            if (currentBlock > 0)
                amount = BlockDamage(amount);

            if (enemy != null && enemy.wiggler && currentHealth == maxHealth)
                enemy.CurlUP();

            currentHealth -= amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UpdateHealthUI(currentHealth);


            if (currentHealth <= 0)
            {
                if (enemy != null)
                    cm.EndFight(true);
                else
                    cm.EndFight(false);
                if (isPlayer) { gameObject.SetActive(false); }
                else { Destroy(gameObject); }
            }
        }
        public void UpdateHealthUI(int newAmount)
        {
            currentHealth = newAmount;
            fighterHealthBar.DisplayHealth(newAmount);
        }
        public void AddBlock(int amount)
        {
            currentBlock += amount;
            fighterHealthBar.DisplayBlock(currentBlock);
        }
        private int BlockDamage(int amount)
        {
            if (currentBlock >= amount)
            {
                currentBlock -= amount;
                amount = 0;
            }
            else
            {
                amount -= currentBlock;
                currentBlock = 0;
            }

            fighterHealthBar.DisplayBlock(currentBlock);
            return amount;
        }

        public void AddBuff(Buff.Type type, int amount)
        {
            if (type == Buff.Type.vulnerable)
            {
                if (vulnerable.buffValue <= 0)
                {
                    vulnerable.buffObject = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                }
                vulnerable.buffValue += amount;
                vulnerable.buffObject.DisplayBuff(vulnerable);
            }
            else if (type == Buff.Type.weak)
            {
                if (weak.buffValue <= 0)
                {
                    weak.buffObject = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                }
                weak.buffValue += amount;
                weak.buffObject.DisplayBuff(weak);
            }
            else if (type == Buff.Type.strength)
            {
                if (strength.buffValue == 0)
                {
                    strength.buffObject = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                }
                strength.buffValue += amount;
                strength.buffObject.DisplayBuff(strength);
            }
            else if (type == Buff.Type.ritual)
            {
                if (ritual.buffValue <= 0)
                {
                    ritual.buffObject = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                }
                ritual.buffValue += amount;
                ritual.buffObject.DisplayBuff(ritual);
            }
            else if (type == Buff.Type.enrage)
            {
                if (enrage.buffValue <= 0)
                {
                    enrage.buffObject = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                }
                enrage.buffValue += amount;
                enrage.buffObject.DisplayBuff(enrage);
            }
        }
        public void EvaluateBuffsAtTurnEnd()
        {
            if (vulnerable.buffValue > 0)
            {
                vulnerable.buffValue -= 1;
                vulnerable.buffObject.DisplayBuff(vulnerable);

                if (vulnerable.buffValue <= 0)
                    Destroy(vulnerable.buffObject.gameObject);
            }
            else if (weak.buffValue > 0)
            {
                weak.buffValue -= 1;
                weak.buffObject.DisplayBuff(weak);

                if (weak.buffValue <= 0)
                    Destroy(weak.buffObject.gameObject);
            }
            else if (ritual.buffValue > 0)
            {
                AddBuff(Buff.Type.strength, ritual.buffValue);
            }
        }
        public void ResetBuffs()
        {
            if (vulnerable.buffValue > 0)
            {
                vulnerable.buffValue = 0;
                Destroy(vulnerable.buffObject.gameObject);
            }
            else if (weak.buffValue > 0)
            {
                weak.buffValue = 0;
                Destroy(weak.buffObject.gameObject);
            }
            else if (strength.buffValue != 0)
            {
                strength.buffValue = 0;
                Destroy(strength.buffObject.gameObject);
            }

            currentBlock = 0;
            fighterHealthBar.DisplayBlock(0);
        }
    }
}