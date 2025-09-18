using System;
using Common.Features;
using Common.Vcontainer.Entity;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Common.Vcontainer.UseCase.Audio
{
    public class AudioUseCase : IDisposable
    {
        readonly VolumeEntity _volumeEntity;
        readonly AudioEntity _audioEntity;
        readonly ComponentAssembly _componentAssembly;

        public ReadOnlyReactiveProperty<float> BgmVolume { get; }
        public ReadOnlyReactiveProperty<float> SeVolume { get; }

        private readonly CompositeDisposable _disposables = new();

        public AudioUseCase(
            VolumeEntity volumeEntity,
            AudioEntity audioEntity,
            ComponentAssembly componentAssembly)
        {
            _volumeEntity = volumeEntity;
            _audioEntity = audioEntity;
            _componentAssembly = componentAssembly;

            BgmVolume = _volumeEntity.BgmVolume.ToReadOnlyReactiveProperty(0f).AddTo(_disposables);
            SeVolume = _volumeEntity.SeVolume.ToReadOnlyReactiveProperty(0f).AddTo(_disposables);
        }

        public async UniTask BgmUp()
        {
            var currentVolume = (int)this.BgmVolume.CurrentValue;
            await _volumeEntity.SetBGMVolume(currentVolume + _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
        }

        public async UniTask BgmDown()
        {
            var currentVolume = (int)this.BgmVolume.CurrentValue;
            await _volumeEntity.SetBGMVolume(currentVolume - _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
        }

        public async UniTask SeUp()
        {
            var currentVolume = (int)this.SeVolume.CurrentValue;
            await _volumeEntity.SetSEVolume(currentVolume + _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
        }

        public async UniTask SeDown()
        {
            var currentVolume = (int)this.SeVolume.CurrentValue;
            await _volumeEntity.SetSEVolume(currentVolume - _volumeEntity.VolumeIncrement, _componentAssembly.AudioMixer);
        }
        public async UniTask PlayUISound(string soundName)
        {
            await _audioEntity.PlaySE(soundName);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
