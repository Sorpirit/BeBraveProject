using Core.CardSystem.Data.Scriptable;
using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    public abstract class CommonCardDescriptionSO : CardDescriptionSO
    {
        [Space]
        [Header("Common Card Info")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite sprite;
        
        public GameObject Prefab => prefab;

        public Sprite Sprite => sprite;
    }
}