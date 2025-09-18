using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Common.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine; // Debug.Logのために追加

namespace Common.Vcontainer.Entity
{
    public class AudioEntity
    {
        // readonlyにして、コンストラクタ以外で変更できないようにする
        private readonly ICueController[] _controllers;

        // コンストラクタで、必要な依存性を全て受け取る
        public AudioEntity(CueSheetAsset cueSheetAsset)
        {
            if (cueSheetAsset == null)
            {
                Debug.LogError("CueSheetAsset is not injected correctly!");
                return;
            }

            var cueList = cueSheetAsset.cueSheet.cueList;
            _controllers = new ICueController[cueList.Count];

            // SetCueSheetのロジックをコンストラクタ内に移動
            _controllers[(int)Settings.AudioType.BGM] ??= AudioConductorInterface.CreateController(cueSheetAsset, (int)Settings.AudioType.BGM);
            _controllers[(int)Settings.AudioType.SE] ??= AudioConductorInterface.CreateController(cueSheetAsset, (int)Settings.AudioType.SE);
        }

        public async UniTask PlayBGM(string bgmName)
        {
            _controllers[(int)Settings.AudioType.BGM].Play(bgmName);
            await UniTask.CompletedTask;
        }

        public async UniTask PlaySE(string seName)
        {
            // コンストラクタで初期化が保証されているため、ここではnullチェックは不要
            _controllers[(int)Settings.AudioType.SE].Play(seName);
            await UniTask.CompletedTask;
        }

        public async UniTask StopBGM()
        {
            _controllers[(int)Settings.AudioType.BGM].Stop(false);
            await UniTask.CompletedTask;
        }
    }
}

