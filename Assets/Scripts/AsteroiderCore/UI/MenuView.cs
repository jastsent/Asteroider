using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroiderCore.UI
{
    public sealed class MenuView : MonoBehaviour, IView
    {
        [field: SerializeField] public Button StartButton { get; private set; }
        [field: SerializeField] public Button QuitButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TotalRecordText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LastRecordText { get; private set; }
    }
}