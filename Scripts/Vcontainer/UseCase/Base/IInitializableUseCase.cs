using Cysharp.Threading.Tasks;

namespace Common.Vcontainer.UseCase.Base
{
    /// <summary>
    /// 初期化処理を持つUseCaseが実装すべき、共通のインターフェース
    /// </summary>
    public interface IInitializableUseCase
    {
        /// <summary>
        /// 実行順序。数値が小さいものから先に実行される。
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 実行される初期化処理
        /// </summary>
        UniTask InitializeAsync();
    }
}
