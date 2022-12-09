using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BK
{
    public class Node : MonoBehaviour
    {
        public Image pressedIcon;
        public Image interactableIcon;
        public Floor floor;
        public void Press()
        {
            floor.Pressed(this);
        }
    }
}