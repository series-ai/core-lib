using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core
{
    [Serializable]
    public class InitializationStage
    {
        // TODO: Get the modules automatically when detecting changes in the hierarchy and only allow to change order
        [SerializeField] private GameObject[] modules;

        private List<Task> initializationTasks = new();
        
        public async Task Init(StringBuilder sb)
        {
            foreach (var module in modules)
            {
                var task = InitModule(module, sb);
                initializationTasks.Add(task);
            }

            await Task.WhenAll(initializationTasks);
            
            initializationTasks.Clear();
        }

        private async Task InitModule(GameObject module, StringBuilder sb)
        {
            var watch = new Stopwatch();
            watch.Start();
            
            var initializable = module.GetComponent<IInitializable>();
            if (initializable != null)
            {
                initializable.Init();
            }
            else
            {
                var initializableAsync = module.GetComponent<IInitializableAsync>();
                if (initializableAsync != null)
                {
                    await initializableAsync.Init();
                }
            }

            watch.Stop();
            sb.Append($"Module {module.name} initialization time: {watch.ElapsedMilliseconds}");
            sb.Append(Environment.NewLine);
        }
    }
}