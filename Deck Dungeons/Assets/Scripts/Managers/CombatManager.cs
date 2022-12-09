using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace BK
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Cards")]
        public List<Card> deck;
        public List<Card> drawPile = new();
        public List<Card> cardsInHand = new();
        public List<Card> discardPile = new();
        public CardUI selectedCard;
        public List<CardUI> cardsInHandGameObjects = new();

        [Header("Stats")]
        public Fighter cardTarget;
        public Fighter player;
        public int maxEnergy;
        public int energy;
        public int drawAmount = 5;
        public Turn turn;
        public enum Turn { Player, Enemy };
        public bool bossFight = false;

        [Header("UI")]
        public Button endTurnButton;
        public TMP_Text drawPileCountText;
        public TMP_Text discardPileCountText;
        public TMP_Text energyText;
        public Transform topParent;
        public Transform enemyParent;

        [Header("Enemies")]
        public Enemy enemy;
        Fighter eFighter;
        public GameObject[] possibleEnemies;
        public GameObject[] possibleElites;
        bool eliteFight;
        public GameObject birdIcon;
        CardActions cardActions;
        GameManager gameManager;
        public RewardScreen rewardScreen;
        public Animator banner;
        public TMP_Text turnText;
        public GameObject gameover;
        public GameObject gamewon;
        public Map map;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            cardActions = GetComponent<CardActions>();
        }
        public void StartHallwayFight()
        {
            BeginBattle(possibleEnemies);
        }
        public void StartEliteFight()
        {
            eliteFight = true;
            BeginBattle(possibleElites);
        }
        public void BeginBattle(GameObject[] prefabsArray)
        {
            _ = Instantiate(prefabsArray[Random.Range(0, prefabsArray.Length)], enemyParent);

            Enemy e = FindObjectOfType<Enemy>();
            enemy = null;

            deck = gameManager.playerDeck;
            discardPile = new List<Card>();
            drawPile = new List<Card>();
            cardsInHand = new List<Card>();
            turn = Turn.Player;
            endTurnButton.enabled = true;

            enemy = e;
            eFighter = e.GetComponent<Fighter>();
            e.DisplayIntent();

            discardPile.AddRange(gameManager.playerDeck);
            ShuffleCards();
            DrawCards(drawAmount);
            energy = maxEnergy;
            energyText.text = energy.ToString();

            if (gameManager.CheckRelic("Anchor"))
            {
                player.AddBlock(10);
            }
            if (gameManager.CheckRelic("Bag of Marbles"))
            {
                eFighter.AddBuff(Buff.Type.vulnerable, 1);
            }
            if (gameManager.CheckRelic("Bag of Preparation"))
            {
                DrawCards(2);
            }
            if (gameManager.CheckRelic("Vajra"))
            {
                player.AddBuff(Buff.Type.strength, 1);
            }
            if (gameManager.CheckRelic("Pantograph"))
            {
                player.currentHealth += 25;
                if (player.currentHealth > player.maxHealth)
                    player.currentHealth = player.maxHealth;

                player.UpdateHealthUI(player.currentHealth);
            }
        }
        public void ShuffleCards()
        {
            discardPile.Shuffle();
            drawPile = discardPile;
            discardPile = new List<Card>();
            discardPileCountText.text = discardPile.Count.ToString();
        }
        public void DrawCards(int amountToDraw)
        {
            int cardsDrawn = 0;
            while (cardsDrawn < amountToDraw && cardsInHand.Count <= 7)
            {
                if (drawPile.Count == 0 && discardPile.Count == 0)
                    break;
                if (drawPile.Count < 1)
                    ShuffleCards();

                cardsInHand.Add(drawPile[0]);
                DisplayCardInHand(drawPile[0]);
                drawPile.Remove(drawPile[0]);
                drawPileCountText.text = drawPile.Count.ToString();
                cardsDrawn++;
            }
        }
        public void DisplayCardInHand(Card card)
        {
            foreach (CardUI c in cardsInHandGameObjects) {
                if (c.gameObject.activeSelf == false)
                {
                    c.LoadCard(card);
                    c.gameObject.SetActive(true);
                    break;
                }
            }
        }
        public void PlayCard()
        {
            if (selectedCard.card.cardType == Card.CardType.Attack && cardTarget == null) 
            {
                selectedCard = null;
                return; 
            }

            if (selectedCard.card.cardType == Card.CardType.Skill && eFighter.enrage.buffValue > 0)
                eFighter.AddBuff(Buff.Type.strength, eFighter.enrage.buffValue);
            
            cardActions.PerformAction(selectedCard.card, cardTarget);
           

            energy -= selectedCard.card.GetCardCostAmount();
            energyText.text = energy.ToString();

            
            selectedCard.gameObject.SetActive(false);
            cardsInHand.Remove(selectedCard.card);
            DiscardCard(selectedCard.card);
            selectedCard = null;
        }
        public void DiscardCard(Card card)
        {
            discardPile.Add(card);
            discardPileCountText.text = discardPile.Count.ToString();
        }
        public void ChangeTurn()
        {
            if (turn == Turn.Player)
            {
                turn = Turn.Enemy;
                endTurnButton.enabled = false;

                
                foreach (Card card in cardsInHand)
                {
                    DiscardCard(card);
                }
                foreach (CardUI cardUI in cardsInHandGameObjects)
                {
                    

                    cardUI.gameObject.SetActive(false);
                    cardsInHand.Remove(cardUI.card);
                }

                
                    if (enemy.thisEnemy == null)
                    enemy.thisEnemy = enemy.GetComponent<Fighter>();

                enemy.thisEnemy.currentBlock = 0;
                enemy.thisEnemy.fighterHealthBar.DisplayBlock(0);
                

                player.EvaluateBuffsAtTurnEnd();
                StartCoroutine(HandleEnemyTurn());
            }
            else
            {
                if (player == null) { return; }
                enemy.DisplayIntent();
                turn = Turn.Player;

                player.currentBlock = 0;
                player.fighterHealthBar.DisplayBlock(0);
                energy = maxEnergy;
                energyText.text = energy.ToString();

                endTurnButton.enabled = true;
                DrawCards(drawAmount);
            }
        }
        private IEnumerator HandleEnemyTurn()
        {
            yield return new WaitForSeconds(1.5f);

            
            enemy.midTurn = true;
            enemy.TakeTurn();
            while (enemy.midTurn)
                yield return new WaitForEndOfFrame();

            ChangeTurn();
        }
        public void BossFight(bool b)
        {
            bossFight = b;
        }
        public void EndFight(bool win)
        {
            foreach (CardUI cardUI in cardsInHandGameObjects)
            {
                cardUI.gameObject.SetActive(false);
                cardsInHand.Remove(cardUI.card);
            }

            if (gameManager.CheckRelic("Burning Blood"))
            {
                player.currentHealth += 6;
                if (player.currentHealth > player.maxHealth)
                    player.currentHealth = player.maxHealth;

                player.UpdateHealthUI(player.currentHealth);
            }

            if (!win)
            {
                gameover.SetActive(true);
                map.ResetFloorNumber();
                Destroy(enemy.gameObject);
            }
            else
            {
                player.ResetBuffs();
                if (bossFight)
                {
                    gamewon.SetActive(true);
                    map.ResetFloorNumber();
                    return;
                }
                DoRewards();
            }
        }
        public void DoRewards()
        {
            rewardScreen.gameObject.SetActive(true);
            rewardScreen.cardReward.gameObject.SetActive(true);

            if (enemy.elite)
            {
                gameManager.relicLibrary.Shuffle();
                rewardScreen.relicReward.gameObject.SetActive(true);
                rewardScreen.relicReward.DisplayRelic(gameManager.relicLibrary[0]);
                gameManager.relics.Add(gameManager.relicLibrary[0]);
                gameManager.relicLibrary.Remove(gameManager.relicLibrary[0]);
            }
            else
            {
                rewardScreen.relicReward.gameObject.SetActive(false);
            }
        }
    }

}