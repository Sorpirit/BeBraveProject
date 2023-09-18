using System.Collections;
using Core.RoomsSystem.RoomVariants;
using Scripts.DependancyInjector;
using UnityEngine;

namespace UI
{
    public class FightController : MonoBehaviour
    {
        [SerializeField] private float delayBetweenRounds = 1f;
        
        [Inject]
        private IFightCallbacks fightCallbacks;

        private void Start()
        {
            fightCallbacks.OnFightRoundFinished += OnFightRoundFinished;
            fightCallbacks.OnEncounterFinished += OnEncounterFinished;
        }

        private void OnEncounterFinished()
        {
            
        }

        private void OnFightRoundFinished()
        {
            StartCoroutine(TriggerRound());
        }
        
        private IEnumerator TriggerRound()
        {
            yield return new WaitForSeconds(delayBetweenRounds);
            fightCallbacks.TriggerRound();
        }
    }
}