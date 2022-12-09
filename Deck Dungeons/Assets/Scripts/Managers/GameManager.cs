using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace BK
{
    public class GameManager : MonoBehaviour
    {
        [Header("Character")]
        public List<Card> starterDeck;
        public List<Card> playerDeck;
        public List<Card> cardLibrary = new();
        public List<Relic> relics = new();
        public List<Relic> relicLibrary = new();

        public void StartGame()
        {
            playerDeck = new();
            foreach (Card c in starterDeck)
            {
                playerDeck.Add(c);
            }
            
        }
        public bool CheckRelic(string rName)
        {
            foreach (Relic r in relics)
            {
                if (r.relicName == rName)
                    return true;
            }
            return false;
        }
    }
}