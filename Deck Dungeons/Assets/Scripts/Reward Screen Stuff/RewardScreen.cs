using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BK {
    public class RewardScreen : MonoBehaviour
    {
        public RewardUI relicReward;
        public RewardUI cardReward;
        public List<RewardUI> cardRewardObjects;
        GameManager gm;
        void Start()
        {
            gm = FindObjectOfType<GameManager>();
        }
        public void PopulateCards()
        {
            gm.cardLibrary.Shuffle();

            for (int i = 0; i < 3; i++)
            {
                cardRewardObjects[i].gameObject.SetActive(true);
                cardRewardObjects[i].DisplayCard(gm.cardLibrary[i]);
            }
        }
        public void ChoseCard(int card)
        {
            gm.playerDeck.Add(gm.cardLibrary[card]);

            for(int i = 0; i < 3; i++)
            {
                cardRewardObjects[i].gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
        public void SkipCard()
        {
            for (int i = 0; i < 3; i++)
            {
                cardRewardObjects[i].gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
    }
}
