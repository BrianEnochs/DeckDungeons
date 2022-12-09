using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BK
{
    public class RewardUI : MonoBehaviour
    {
        public Image rewardImage;
        public TMP_Text rewardName;
        public TMP_Text rewardDescription;
        public TMP_Text cardCost;

        public void DisplayRelic(Relic r)
        {
            rewardImage.sprite = r.relicImage;
            rewardName.text = r.relicName;
            rewardDescription.text = r.relicDescription;
        }
        public void DisplayCard(Card c)
        {
            rewardImage.sprite = c.cardIcon;
            rewardName.text = c.cardTitle;
            rewardDescription.text = c.GetCardDescriptionAmount();
            cardCost.text = c.GetCardCostAmount().ToString();
        }
    }
}
