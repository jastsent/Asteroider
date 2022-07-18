using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Components
{
    public sealed class ViewLaser : EcsComponent
    {
        public LineRenderer LineRenderer;
        public float Timer;
    }
}