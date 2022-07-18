using System;
using UnityEngine;

namespace AsteroiderCore.Settings
{
    [Flags]
    public enum CollisionLayer
    {
        Player = 1,
        Asteroid = 2,
        UFO = 4,
        Bullet = 8
    }

    [CreateAssetMenu(fileName = "AsteroiderLayerCollisionMatrix", menuName = "Asteroider/LayerCollisionMatrix")]
    public sealed class LayerCollisionMatrix : ScriptableObject
    {
        [SerializeField] private CollisionLayer player;
        [SerializeField] private CollisionLayer asteroid;
        [SerializeField] private CollisionLayer ufo;
        [SerializeField] private CollisionLayer bullet;

        public CollisionLayer Player => player;
        public CollisionLayer Asteroid => asteroid;
        public CollisionLayer Ufo => ufo;
        public CollisionLayer Bullet => bullet;

        public bool CheckCollision(CollisionLayer layer1, CollisionLayer layer2)
        {
            switch (layer1)
            {
                case CollisionLayer.Player:
                    return player.HasFlag(layer2);
                case CollisionLayer.Asteroid:
                    return asteroid.HasFlag(layer2);
                case CollisionLayer.UFO:
                    return ufo.HasFlag(layer2);
                case CollisionLayer.Bullet:
                    return bullet.HasFlag(layer2);
            }

            return false;
        }
    }
}