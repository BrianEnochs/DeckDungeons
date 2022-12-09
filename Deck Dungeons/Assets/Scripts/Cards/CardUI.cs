using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BK
{
    public class CardUI : MonoBehaviour
    {
        public Card card;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardDescription;
        public TextMeshProUGUI cardCost;
        public Image cardImage;
        CombatManager cm;
        public float cardHoverAmount = 50f;

        private void Awake()
        {
            cm = FindObjectOfType<CombatManager>();
        }

        public void LoadCard(Card _card)
        {
            card = _card;
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            cardName.text = card.cardTitle;
            cardDescription.text = card.GetCardDescriptionAmount();
            cardCost.text = card.GetCardCostAmount().ToString();
            cardImage.sprite = card.cardIcon;
        }

        public void OnPointerEnter()
        {
            if (cm.selectedCard == null)
            {
                transform.Translate(new Vector3(0, cardHoverAmount, 0));
            }
        }

        public void OnPointerExit()
        {
            if (cm.selectedCard == null)
            {
                transform.Translate(new Vector3(0, -cardHoverAmount, 0));
            }
        }

        public void OnBeginDrag()
        {
            cm.selectedCard = this;
        }

        public void OnEndDrag()
        {
            if (cm.energy < card.GetCardCostAmount())
            {
                transform.Translate(new Vector3(0, -cardHoverAmount, 0));
                cm.selectedCard = null;
                return;
            }

            if (card.cardType == Card.CardType.Attack)
            {
                cm.PlayCard();
                transform.Translate(new Vector3(0, -cardHoverAmount, 0));
            }
            else if (card.cardType != Card.CardType.Attack)
            {
                transform.Translate(new Vector3(0, -cardHoverAmount, 0));
                cm.PlayCard();
            }
        }
    }
}