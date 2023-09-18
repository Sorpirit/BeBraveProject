using System.Collections;
using Core.GameStates;
using Core.RoomsSystem.RoomVariants;
using DG.Tweening;
using Game;
using Game.RoomFactories;
using Scripts.DependancyInjector;
using UI.Extentsions;
using UnityEngine;

namespace UI
{
    public class FightController : MonoBehaviour
    {
        [SerializeField] private float delayBetweenRounds = 1f;
        [SerializeField] private GameObject player;
        [SerializeField] private float offsetDistance = 0.3f;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private SpriteRenderer fightSplash;
        
        private GameObject enemy;
        private Vector3 playerOriginalPosition;
        
        [Inject]
        private IFightCallbacks fightCallbacks;
        
        [Inject]
        private IRoomContentCallBack<FightEncounterContext> fightEncounterContextCallback;

        [Inject]
        private ICameraEffects cameraEffects;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
        }
        
        private void Start()
        {
            fightCallbacks.OnFightRoundFinished += OnFightRoundFinished;
            fightCallbacks.OnEncounterFinished += OnEncounterFinished;
            fightEncounterContextCallback.OnRoomContentCreated += OnRoomContentCreated;
        }

        private void GameContextCreated(GameContext context)
        {
            context.PlayerEnterRoomState.OnStateEnter += OnPlayerEnterRoom;
        }

        private void OnPlayerEnterRoom()
        {
            if(enemy == null)
                return;
            
            playerOriginalPosition = player.transform.position;
            var sequence = DOTween.Sequence();
            sequence
                .Append(enemy.transform.DOMove(enemy.transform.position + Vector3.right * offsetDistance, moveDuration))
                .Join(player.transform.DOMove(playerOriginalPosition + Vector3.left * offsetDistance, moveDuration))
                .Join(cameraEffects.ZoomIn())
                .OnComplete(StartEncounter);
        }

        private void StartEncounter()
        {
            fightCallbacks.TriggerRound();
        }

        private void OnRoomContentCreated(FightEncounterContext obj)
        {
            enemy = obj.EnemyGameObject;
        }

        private void OnEncounterFinished()
        {
            Destroy(enemy);
            enemy = null;
            var sequence = DOTween.Sequence();
            sequence
                .Append(player.transform.DOMove(playerOriginalPosition, moveDuration))
                .Join(cameraEffects.ZoomOut());
        }

        private void OnFightRoundFinished()
        {
            StartCoroutine(TriggerRound());
        }
        
        private IEnumerator TriggerRound()
        {
            var sq = DOTween.Sequence();
            sq
                .Append(fightSplash.DoAlpha(1, .08f).SetEase(Ease.InQuint))
                .Append(fightSplash.DoAlpha(0, .16f));
            
            yield return new WaitForSeconds(delayBetweenRounds);
            fightCallbacks.TriggerRound();
        }
    }
}