using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroiderCore.UI
{
    public sealed class GameView : MonoBehaviour, IView
    {
        [field: SerializeField] public Button PauseButton { get; private set; }
        [field: SerializeField] public Button ContinueButton { get; private set; }
        [field: SerializeField] public Button ExitPauseScreenButton { get; private set; }
        [field: SerializeField] public Button ExitGameOverScreenButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI PositionText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AngleText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI SpeedText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ChargesText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI RechargeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CooldownText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LifeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ScoreGameOverText { get; private set; }
        [field: SerializeField] public GameObject PauseScreenContainer { get; private set; }
        [field: SerializeField] public GameObject GameOverScreenContainer { get; private set; }
    }
}