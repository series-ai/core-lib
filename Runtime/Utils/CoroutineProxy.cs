using System.Collections;
using UnityEngine;

namespace Padoru.Core
{
    public class CoroutineProxy
    {
        private readonly MonoBehaviour monoBehaviour;

        private Coroutine myCoroutine;

        public CoroutineProxy(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            myCoroutine = monoBehaviour.StartCoroutine(routine);
            return myCoroutine;
        }

        public void StopCoroutine()
        {
            StopCoroutine(myCoroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            monoBehaviour.StopCoroutine(coroutine);
        }
    }
}
