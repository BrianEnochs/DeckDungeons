using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    [System.Serializable]
    public class EnemyAction
    {
        public IntentType intentType;
        public enum IntentType { Attack, Block, StrategicBuff, StrategicDebuff, AttackDebuff, AttackBlock, BuffBlock }
        public int damage;
        public int block;
        public int debuffAmount;
        public Buff.Type buffType;
        public int chance;
        public Sprite icon;
    }
}