using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Components
{
    public class ViewObject : EcsComponent
    {
        public ObjectType ObjectType;
        public GameObject GameObject;
    }
}