using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    [Serializable]
    public class InitializationStage
    {
        // TODO: Get the modules automatically when detecting changes in the hierarchy and only allow to change order
        [SerializeField] private GameObject[] modules;

        private List<Task> initializationTasks = new();
        private List<GameObject> initializedModules = new();
        
        public async Task Init(StringBuilder sb, CancellationToken cancellationToken)
        {
            foreach (var module in modules)
            {
                var task = InitModule(module, sb, cancellationToken);
                initializationTasks.Add(task);
            }

            await Task.WhenAll(initializationTasks);
            
            initializationTasks.Clear();
        }
        
        public void Shutdown()
        {
            // Shutdown in reverse order so that dependencies are shutdown
            // before that which they depend on
            for (int i = modules.Length - 1; i >= 0; i--)
            {
                var module = modules[i];
                
                if (ModuleIsInitialized(module))
                {
                    ShutdownModule(module);
                }                
            }
            
            initializedModules.Clear();
        }

        private async Task InitModule(GameObject module, StringBuilder sb, CancellationToken cancellationToken)
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
                    await initializableAsync.Init(cancellationToken);
                }
            }
            
            watch.Stop();
            
            if (cancellationToken.IsCancellationRequested)
            {
                var cancelledMessage = $"Module {module.name} initialization interrupted. Elapsed time: {watch.ElapsedMilliseconds} ms";
                Debug.Log(cancelledMessage, DebugChannels.INIT);
                sb.Append(cancelledMessage);
                sb.Append(Environment.NewLine);
                return;
            }

            initializedModules.Add(module);

            var finishMessage = $"Module {module.name} initialization time: {watch.ElapsedMilliseconds} ms";
            Debug.Log(finishMessage, DebugChannels.INIT);
            sb.Append(finishMessage);
            sb.Append(Environment.NewLine);
        }

        private void ShutdownModule(GameObject module)
        {
            var shutdownable = module.GetComponent<IShutdowneable>();
            
            if (shutdownable != null)
            {
                shutdownable.Shutdown();
            }
        }
        
        private bool ModuleIsInitialized(GameObject moduleGameObject)
        {
            return initializedModules.Contains(moduleGameObject);
        }
    }
}