using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    public class SceneManager : MonoBehaviour
    {
        public GameObject menuScene;
        public GameObject combatScene;
        public GameObject mapScene;
        public GameObject restScene;
        public Map map;
        public CombatManager cm;
        public GameManager gm;
        public void StartGame()
        {
            menuScene.SetActive(false);
            combatScene.SetActive(false);
            mapScene.SetActive(true);
            cm.player.gameObject.SetActive(true);
            cm.player.Awake();
            cm.player.cm = cm;
            cm.BossFight(false);
            gm.StartGame();
        }
        public void LoadMap()
        {
            combatScene.SetActive(false);
            mapScene.SetActive(true);
        }
        public void StartHallFight()
        {
            mapScene.SetActive(false);
            combatScene.SetActive(true);
            StartCoroutine(HallHelper());
        }
        public void StartEliteFight()
        {
            mapScene.SetActive(false);
            combatScene.SetActive(true);
            StartCoroutine(EliteHelper());
        }
        IEnumerator EliteHelper()
        {
            yield return new WaitForSeconds(0.5f);
            cm.StartEliteFight();
        }
        IEnumerator HallHelper()
        {
            yield return new WaitForSeconds(0.5f);
            cm.StartHallwayFight();
        }
        public void GameOver()
        {
            combatScene.SetActive(false);
            menuScene.SetActive(true);
        }
        public void LoadRest()
        {
            restScene.SetActive(true);
        }
        public void RestHelper()
        {
            cm.player.currentHealth += (int)(cm.player.maxHealth * 0.3f);
            if (cm.player.currentHealth > cm.player.maxHealth)
            {
                cm.player.currentHealth = cm.player.maxHealth;
            }
            cm.player.UpdateHealthUI(cm.player.currentHealth);

            restScene.SetActive(false);
        }
    }
}
