using AsteroiderCore.GameFSM;

namespace AsteroiderCore.UI
{
    public sealed class MenuPresenter : IPresenter
    {
        private readonly MenuView _view;
        private readonly GameStateMachine _stateMachine;
        private readonly PlayerData _playerData;

        public MenuPresenter(MenuView view, GameStateMachine stateMachine, PlayerData playerData)
        {
            _view = view;
            _stateMachine = stateMachine;
            _playerData = playerData;
        }

        public void Initialize()
        {
            _view.StartButton.onClick.AddListener(OnClickStartButton);
            _view.QuitButton.onClick.AddListener(OnClickQuitButton);
            _view.TotalRecordText.text = _playerData.TotalRecord.ToString();
            _view.LastRecordText.text = _playerData.LastRecord.ToString();
        }

        public void Clear()
        {
            _view.StartButton.onClick.RemoveListener(OnClickStartButton);
            _view.QuitButton.onClick.RemoveListener(OnClickQuitButton);
        }

        private void OnClickStartButton()
        {
            _stateMachine.Start();
        }

        private void OnClickQuitButton()
        {
            _stateMachine.Quit();
        }
    }
}
