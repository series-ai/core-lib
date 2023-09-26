using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core
{
    public static class YieldInstructionsExtensions
    {
        public static Task AsTask(this YieldInstruction instruction, CoroutineProxy coroutineProxy)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            coroutineProxy.StartCoroutine(WaitForCompletion(instruction, taskCompletionSource));
            return taskCompletionSource.Task;
        }

        private static IEnumerator WaitForCompletion(YieldInstruction instruction, TaskCompletionSource<bool> tsc)
        {
            yield return instruction;
            tsc.SetResult(true);
        }
    }
}
