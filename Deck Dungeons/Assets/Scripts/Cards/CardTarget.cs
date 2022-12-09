using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    public class CardTarget : MonoBehaviour
    {
        CombatManager cm;
        Fighter enemyFighter;

        private void Awake()
        {
            cm = FindObjectOfType<CombatManager>();
            enemyFighter = GetComponent<Fighter>();
        }
        public void PointerEnter()
        {
            if (enemyFighter == null)
            {
                cm = FindObjectOfType<CombatManager>();
                enemyFighter = GetComponent<Fighter>();
            }
            cm.cardTarget = enemyFighter;
        }
        public void PointerExit()
        {
            cm.cardTarget = null;
        }
    }
}