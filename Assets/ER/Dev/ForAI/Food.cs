using ER;
using ER.ForEditor;
using UnityEngine;

namespace Dev.AI
{
    public class Food : Water
    {
        [DisplayLabel("营养价值")]
        public float value = 2;

        public enum Mode
        {
            NewPosition,
            Destory
        }

        public Mode mode = Mode.NewPosition;

        public void Eated()
        {
            switch (mode)
            {
                case Mode.NewPosition:
                    transform.position += (Vector3)Random.insideUnitCircle * Random.value * 5f;
                    break;

                case Mode.Destory:
                    Destroy();
                    break;
            }
        }
    }
}