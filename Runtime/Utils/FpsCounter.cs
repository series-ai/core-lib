using System;
using UnityEngine;
using UnityEngine.Events;

// TODO: Migrate this to a class, and instantiate it on the development screen
public class FpsCounter : MonoBehaviour
{
    [SerializeField] private UnityEvent<string> onFpsChanged;
    
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }

    private void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        var fps = Mathf.RoundToInt(CalculateFps());
        onFpsChanged?.Invoke(fps.ToString());
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
