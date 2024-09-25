﻿using System.Collections;
using UnityEngine;

namespace Dev2
{
    public abstract class SceneInitializer : MonoBehaviour
    {
        private bool autoDestroy = true;

        public SceneInitializer(bool autoDestroy)
        {
            this.autoDestroy = autoDestroy;
        }

        public IEnumerator Initialize()
        {
            yield return InitContentAsync();
            if (autoDestroy)
            {
                Destroy(this);
            }
        }

        protected abstract IEnumerator InitContentAsync();
    }
}