using UnityEngine;

namespace AsteroiderCore.Settings
{
    [CreateAssetMenu(fileName = "AsteroiderBattleConfiguration", menuName = "Asteroider/BattleConfiguration")]
    public sealed class BattleConfiguration : ScriptableObject
    {
        [field:Header("Battlefield")]
        [field:SerializeField] public Vector2 BattlefieldOriginPoint { get; private set; }
        [field:SerializeField] public float BattlefieldWidth { get; private set; }
        [field:SerializeField] public float BattlefieldHeight { get; private set; }
        [field:Header("Rocket")]
        [field:SerializeField] public int LifeCount { get; private set; }
        [field:SerializeField] public float RespawnInvulnerabilityTime { get; private set; }
        [field:SerializeField] public float RocketSize { get; private set; }
        [field:SerializeField] public float AccelerationSpeed { get; private set; }
        [field:SerializeField] public float SlowdownSpeed { get; private set; }
        [field:SerializeField] public float MaxMoveSpeed { get; private set; }
        [field:SerializeField] public float RotationSpeed { get; private set; }
        [field:Header("Attack")]
        [field:SerializeField] public float AttackSpeed { get; private set; }
        [field:SerializeField] public float BulletSpeed { get; private set; }
        [field:SerializeField] public float BulletSize { get; private set; }
        [field:Header("Laser")]
        [field:SerializeField] public int MaxLaserCharges { get; private set; }
        [field:SerializeField] public float LaserRechargeTime { get; private set; }
        [field:SerializeField] public float LaserCooldownTime { get; private set; }
        [field:SerializeField] public float LaserWidth { get; private set; }
        [field:Header("Asteroids")]
        [field:SerializeField] public int AsteroidsMaxCount { get; private set; }
        [field:SerializeField] public float AsteroidSpawnRate { get; private set; }
        [field:SerializeField] public float AsteroidSizeMin { get; private set; }
        [field:SerializeField] public float AsteroidSizeMax { get; private set; }
        [field:SerializeField] public float AsteroidMoveSpeedMin { get; private set; }
        [field:SerializeField] public float AsteroidMoveSpeedMax { get; private set; }
        [field:SerializeField] public int AsteroidSplitCount { get; private set; }
        [field:SerializeField] public int AsteroidShardsCountMin { get; private set; }
        [field:SerializeField] public int AsteroidShardsCountMax { get; private set; }
        [field:SerializeField] public float AsteroidShardsSizeMultiplierMin { get; private set; }
        [field:SerializeField] public float AsteroidShardsSizeMultiplierMax { get; private set; }
        [field:SerializeField] public float AsteroidShardMoveSpeedMultiplierMin { get; private set; }
        [field:SerializeField] public float AsteroidShardMoveSpeedMultiplierMax { get; private set; }
        [field:Header("UFO")]
        [field:SerializeField] public int UfoMaxCount { get; private set; }
        [field:SerializeField] public float UfoSpawnRate { get; private set; }
        [field:SerializeField] public float UfoSize { get; private set; }
        [field:SerializeField] public float UfoMoveSpeed { get; private set; }
        [field:Header("Score")]
        [field:SerializeField] public int PointsPerSecond { get; private set; }
        [field:SerializeField] public int PointsPerUfo { get; private set; }
        [field:SerializeField] public int PointsPerAsteroid { get; private set; }

        public float BattlefieldWidthMin => BattlefieldOriginPoint.x - BattlefieldWidth / 2f;
        public float BattlefieldWidthMax => BattlefieldOriginPoint.x + BattlefieldWidth / 2f;
        public float BattlefieldHeightMin => BattlefieldOriginPoint.y - BattlefieldHeight / 2f;
        public float BattlefieldHeightMax => BattlefieldOriginPoint.y + BattlefieldHeight / 2f;
    }
}