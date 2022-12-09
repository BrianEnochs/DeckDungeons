using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BK
{
    public class CardActions : MonoBehaviour
    {
        Card card;
        public Fighter target;
        public Fighter player;
        CombatManager cm;
        private void Awake()
        {
            cm = FindObjectOfType<CombatManager>();
        }
        public void PerformAction(Card c, Fighter f)
        {
            card = c;
            target = f;

            switch (card.cardTitle)
            {
                case "Strike":
                    AttackEnemy();
                    break;
                case "Defend":
                    PerformBlock();
                    break;
                case "Bash":
                    AttackEnemy();
                    ApplyBuff(Buff.Type.vulnerable);
                    break;
                case "Inflame":
                    ApplyBuffToSelf(Buff.Type.strength);
                    break;
                case "Clothesline":
                    AttackEnemy();
                    ApplyBuff(Buff.Type.weak);
                    break;
                case "Shrug It Off":
                    PerformBlock();
                    cm.DrawCards(1);
                    break;
                case "Iron Wave":
                    AttackEnemy();
                    PerformBlock(); 
                    break;
                case "Bloodletting":
                    HurtPlayer(3);
                    cm.energy += 2;
                    break;
                case "Body Slam":
                    BodySlam();
                    break;
                case "Entrench":
                    Entrench();
                    break;
                case "Anger":
                    AttackEnemy();
                    break;
                case "Bite":
                    AttackEnemy();
                    HurtPlayer(-2);
                    break;
                case "Dropkick":
                    AttackEnemy();
                    if (target.vulnerable.buffValue > 0)
                    {
                        cm.energy += 1;
                        cm.DrawCards(1);
                    }
                    break;
                case "Hemokinesis":
                    HurtPlayer(2);
                    AttackEnemy();
                    break;
                case "Pommel Strike":
                    AttackEnemy();
                    cm.DrawCards(1);
                    break;
                default:
                    Debug.Log("Missing cardaction");
                    break;
            }
        }
        private void AttackEnemy()
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (player.weak.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 0.75f);
            }
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            target.TakeDamage(totalDamage);
        }
        private void BodySlam()
        {
            int totalDamage = player.currentBlock;
            if (player.weak.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 0.75f);
            }
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            target.TakeDamage(totalDamage);
        }
        private void Entrench()
        {
            player.AddBlock(player.currentBlock);
        }
        private void ApplyBuff(Buff.Type t)
        {
            target.AddBuff(t, card.GetBuffAmount());
        }
        private void ApplyBuffToSelf(Buff.Type t)
        {
            player.AddBuff(t, card.GetBuffAmount());
        }
        private void HurtPlayer(int dam)
        {
            player.currentHealth -= dam;
            player.TakeDamage(0);
        }
        private void PerformBlock()
        {
            player.AddBlock(card.GetCardEffectAmount());
        }
    }
}