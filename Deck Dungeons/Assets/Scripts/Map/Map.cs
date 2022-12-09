using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BK
{
    public class Map : MonoBehaviour
    {
        public List<Floor> floors;
        public int bossFloor;
        public int eliteFloors;
        public int restFloors;
        public NodeEvent enemyEncounter;
        public NodeEvent eliteEncounter;
        public NodeEvent restEncounter;
        public int currentFloorNumber;
        private void Awake()
        {
            for (int i = 0; i < floors.Count; i++)
            {
                if (i == eliteFloors || i == bossFloor)
                    floors[i].SetNodesInteractable(eliteEncounter);
                else if (i == restFloors)
                    floors[i].SetNodesInteractable(restEncounter);
                else
                    floors[i].SetNodesInteractable(enemyEncounter);
            }
        }
        public void ShowOptions()
        {
            if (currentFloorNumber == floors.Count)
            {
                currentFloorNumber = 0;
                GenerateMap();
                return;
            }

            for (int i = 0; i < floors.Count; i++)
            {
                if (i == currentFloorNumber)
                    floors[i].SetNodesActiveInteractable();
            }
            currentFloorNumber++;
        }
        public void GenerateMap()
        {
            for (int i = 0; i < floors.Count; i++)
            {
                if (i == eliteFloors)
                    floors[i].SetNodesInteractable(eliteEncounter);
                else if (i == restFloors)
                    floors[i].SetNodesInteractable(restEncounter);
                else
                    floors[i].SetNodesInteractable(enemyEncounter);
            }
            ShowOptions();
        }
        public void ResetFloorNumber()
        {
            currentFloorNumber = 0;
        }
        private void OnEnable()
        {
            ShowOptions();
        }
    }
    [System.Serializable]
    public struct NodeEvent
    {
        public Type eventType;
        public enum Type { enemy, elite, chest, rest };
        public Sprite eventSprite;

    }
}
