using System.IO;
using UnityEngine;

namespace AsteroiderCore.Settings
{
    [CreateAssetMenu(fileName = "AsteroiderSettings", menuName = "Asteroider/GameSettings")]
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField] private string saveFilePath;
        [SerializeField] private string saveFileName;
        [field:SerializeField] public int MenuSceneBuildId { get; private set; }
        [field:SerializeField] public int GameSceneBuildId { get; private set; }
        [field: Space]
        [field:SerializeField] public GameObject RocketPrefab { get; private set; }
        [field:SerializeField] public GameObject AsteroidPrefab { get; private set; }
        [field:SerializeField] public GameObject UFOPrefab { get; private set; }
        [field:SerializeField] public GameObject BulletPrefab { get; private set; }
        [field:SerializeField] public GameObject LaserPrefab { get; private set; }
        
        public string SavePath => Path.Combine(saveFilePath, saveFileName);
    }
}
