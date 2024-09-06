using UnityEngine;

namespace ER.STG
{
    public interface IRole
    {
        public bool IsMoving { get; }
        public void StopMove();
        public void Move(Vector2 dir, bool slowMode = false);

        public void Shoot();

        public void Bomb();

        public void Action(int index);
    }
}