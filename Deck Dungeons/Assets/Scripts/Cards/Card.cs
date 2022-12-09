using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    [CreateAssetMenu]
    public class Card : ScriptableObject
    {
        public string cardTitle;
        public string cardDescription;
        public int cardCost;
        public int cardEffect;
        public int buffAmount;
        public Sprite cardIcon;
        public CardType cardType;
        public enum CardType { Attack, Skill, Power }
        public CardTargetType cardTargetType;
        public enum CardTargetType { self, enemy };

        public int GetCardCostAmount()
        {
            return cardCost;
        }
        public int GetCardEffectAmount()
        {
            return cardEffect;
        }
        public string GetCardDescriptionAmount()
        {
            return cardDescription;
        }
        public int GetBuffAmount()
        {
            return buffAmount;
        }
    }
}
