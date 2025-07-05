using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class ProjectileBuilder
    {
        public Vector2 Direction;
        public Vector2 Position;
        public Quaternion Rotation = Quaternion.identity;
        public GameObject Owner;

        public ProjectileBuilder SetDirection(Vector2 direction)
        {
            Direction = direction;
            return this;
        }

        public ProjectileBuilder SetPosition(Vector2 position)
        {
            Position = position;
            return this;
        }

        public ProjectileBuilder SetOwner(GameObject owner)
        {
            Owner = owner;
            return this;
        }

        public ProjectileBuilder SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
            return this;
        }
    }
}