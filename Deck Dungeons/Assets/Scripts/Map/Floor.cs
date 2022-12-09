using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BK
{
    public class Floor : MonoBehaviour
    {
        public List<Image> nodes;
        public List<Image> interactableNodes;
        public NodeEvent ne;
        SceneManager sceneManager;
        Map map;
        HorizontalLayoutGroup horizontalLayoutGroup;

        public void StartEvent()
        {
            if (ne.eventType == NodeEvent.Type.enemy)
                sceneManager.StartHallFight();
            else if (ne.eventType == NodeEvent.Type.elite)
                sceneManager.StartEliteFight();
            else if (ne.eventType == NodeEvent.Type.rest)
                sceneManager.LoadRest();
        }
        public void SetNodesInteractable(NodeEvent _encounter)
        {
            horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
            sceneManager = FindObjectOfType<SceneManager>();
            map = FindObjectOfType<Map>();

            foreach (Transform n in this.gameObject.transform)
                nodes.Add(n.GetComponent<Image>());

            ne = _encounter;
            foreach (Image n in nodes)
            {
                n.enabled = false;
                n.GetComponent<Node>().interactableIcon.enabled = false;
                n.GetComponent<Node>().pressedIcon.enabled = false;
                n.GetComponent<Node>().floor = this;
            }

            horizontalLayoutGroup.spacing = Random.Range(25f, 125f);
            horizontalLayoutGroup.padding.left = Random.Range(0, 75);
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, Random.Range(50, 60));
            interactableNodes.Clear();
            nodes.Shuffle();
            EnableNode(nodes[0]);

            for (int i = 1; i < nodes.Count; i++)
            {
                if (Random.Range(0, 1f) >= i * (1f / (nodes.Count)))
                    EnableNode(nodes[i]);
            }
        }
        public void SetNodesActiveInteractable()
        {
            foreach (Image n in interactableNodes)
                n.GetComponent<Node>().interactableIcon.enabled = true;
        }
        public void EnableNode(Image n)
        {
            n.enabled = true;
            n.sprite = ne.eventSprite;
            interactableNodes.Add(n);
        }
        public void Pressed(Node pressedNode)
        {
            if (!pressedNode.interactableIcon.enabled)
                return;

            //pressedNode.pressedIcon.enabled = true;

            foreach (Image n in interactableNodes)
                n.GetComponent<Node>().interactableIcon.enabled = false;

            StartEvent();
        }

    }
}
