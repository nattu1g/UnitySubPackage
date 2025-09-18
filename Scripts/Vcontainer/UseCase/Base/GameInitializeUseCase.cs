using System.Collections.Generic;
using System.Linq;
using Common.Events;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;

namespace Common.Vcontainer.UseCase.Base
{
    /// <summary>
    /// 登録された全ての IInitializableUseCase を正しい順序で実行する、汎用的な初期化オーケストレーター
    /// </summary>
    public class GameInitializeUseCase
    {
        private readonly IEnumerable<IInitializableUseCase> _useCases;
        private readonly IPublisher<GameInitializedEvent> _publisher;

        public GameInitializeUseCase(
            IEnumerable<IInitializableUseCase> useCases,
            IPublisher<GameInitializedEvent> publisher)
        {
            _useCases = useCases;
            _publisher = publisher;
        }

        public async UniTask InitializeAsync()
        {
            Debug.Log("[GameInitializeUseCase][InitializeAsync] Start");

            // 1. Orderの値でUseCaseをグループ化し、Orderの昇順に並び替える
            var initializersByOrder = _useCases
                .GroupBy(uc => uc.Order)
                .OrderBy(g => g.Key);

            // 2. グループごとにループ処理
            foreach (var group in initializersByOrder)
            {
                // 3. 同じOrder値を持つUseCaseたちは並列で実行する
                await UniTask.WhenAll(group.Select(uc => uc.InitializeAsync()));
            }

            // 4. 全ての初期化が完了したら、イベントを発行して通知する
            _publisher.Publish(new GameInitializedEvent());
        }
    }
}
