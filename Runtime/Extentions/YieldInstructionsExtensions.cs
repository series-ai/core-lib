using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Padoru.Core
{
    public static class YieldInstructionsExtensions
    {
        public static Task AsTask(this YieldInstruction instruction, CoroutineProxy coroutineProxy, CancellationToken token = default)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            coroutineProxy.StartCoroutine(WaitForCompletion(instruction, taskCompletionSource, token));
            return taskCompletionSource.Task;
        }

        private static IEnumerator WaitForCompletion(YieldInstruction instruction, TaskCompletionSource<bool> tsc, CancellationToken token = default)
        {
            yield return instruction;
            
            if (token.IsCancellationRequested)
            {
                tsc.SetCanceled();
                yield break;
            }
            
            tsc.SetResult(true);
        }
    }
}
