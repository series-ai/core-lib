using System;
using UnityEngine;

namespace Padoru.Core.Utils
{
    public class FpsCounter : ITickable, IShutdowneable
    {
        private readonly ITickManager tickManager;
    
        private int lastFrameIndex;
        private float[] frameDeltaTimeArray;
        
        public event Action<string> OnFpsChanged;

        public FpsCounter(ITickManager tickManager)
        {
            this.tickManager = tickManager;
            frameDeltaTimeArray = new float[50];
            
            tickManager.Register(this);
        }

        public void Tick(float deltaTime)
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

            var fps = Mathf.RoundToInt(CalculateFps());
            OnFpsChanged?.Invoke(fps.ToString());
        }

        public void Shutdown()
        {
            tickManager.Unregister(this);
        }

        private float CalculateFps()
        {
            var total = 0f;
            foreach (var deltaTime in frameDeltaTimeArray)
            {
                total += deltaTime;
            }

            return frameDeltaTimeArray.Length / total;
        }
    }
}
