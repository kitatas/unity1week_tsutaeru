using System.Threading;
using Cysharp.Threading.Tasks;
using Tsutaeru.InGame.Domain.UseCase;
using Tsutaeru.InGame.Presentation.View;
using Tsutaeru.OutGame;
using Tsutaeru.OutGame.Domain.UseCase;

namespace Tsutaeru.InGame.Presentation.Controller
{
    public sealed class ResultState : BaseState
    {
        private readonly SceneUseCase _sceneUseCase;
        private readonly TimeUseCase _timeUseCase;
        private readonly HintView _hintView;

        public ResultState(SceneUseCase sceneUseCase, TimeUseCase timeUseCase, HintView hintView)
        {
            _sceneUseCase = sceneUseCase;
            _timeUseCase = timeUseCase;
            _hintView = hintView;
        }

        public override GameState state => GameState.Result;

        public override async UniTask InitAsync(CancellationToken token)
        {
            _hintView.TweenHeightAsync(-100.0f, 0.0f, token).Forget();

            await UniTask.Yield(token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _hintView.ResetAsync(UiConfig.ANIMATION_TIME, token);
            await _hintView.RenderAsync("タイムアタック ランキング", UiConfig.ANIMATION_TIME, token);
            await _hintView.TweenHeightAsync(-50.0f, UiConfig.ANIMATION_TIME, token);

            var score = _timeUseCase.value;
            RankingLoader.Instance.SendScoreAndShowRanking(score);

            // TODO: push button
            await UniTask.Yield(token);

            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneName.Ranking.ToString());
            _sceneUseCase.Load(SceneName.Main);

            return GameState.None;
        }
    }
}