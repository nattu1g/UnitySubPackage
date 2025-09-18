using System.Threading;
using Common.Vcontainer.UseCase.Base;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Common.Vcontainer.EntryPoint
{
    /// <summary>
    /// アプリケーション起動の起点(EntryPoint)。
    /// GameInitializeUseCaseを呼び出すことだけを責務とする。
    /// </summary>
    public class ApplicationInitializer : IAsyncStartable
    {
        private readonly GameInitializeUseCase _gameInitializeUseCase;

        public ApplicationInitializer(GameInitializeUseCase gameInitializeUseCase)
        {
            _gameInitializeUseCase = gameInitializeUseCase;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _gameInitializeUseCase.InitializeAsync();
        }
    }
}
