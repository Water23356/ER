using UnityEngine;

namespace Dev
{
    public class TimeScaleController:MonoBehaviour
    {
        [SerializeField]
        [Range(0,3f)]
        private float scale = 1f;
        private void OnValidate()
        {
            Time.timeScale = scale;
        }
    }
}