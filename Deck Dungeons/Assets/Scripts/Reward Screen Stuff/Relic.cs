using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BK
{
    [CreateAssetMenu]
    public class Relic : ScriptableObject
    {
        public string relicName;
        public string relicDescription;
        public Sprite relicImage;
    }
}
