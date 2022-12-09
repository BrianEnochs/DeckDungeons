using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    public class Player : MonoBehaviour
    {
        public string playerName;
        public int currentHealth;
        public int maxHealth;
        public List<Card> cards;
    }
}