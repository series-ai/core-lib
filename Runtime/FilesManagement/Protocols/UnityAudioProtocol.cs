using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
	public class UnityAudioProtocol : IProtocol
	{
		private readonly string basePath;
		private readonly CoroutineProxy coroutineProxy;
		private readonly IFileNameGenerator fileNameGenerator;
		private readonly string webRequestProtocol;
		private readonly bool streamAudio;

		public UnityAudioProtocol(string basePath, CoroutineProxy coroutineProxy, IFileNameGenerator fileNameGenerator, string webRequestProtocol, bool streamAudio)
		{
			this.basePath = basePath;
			this.coroutineProxy = coroutineProxy;
			this.fileNameGenerator = fileNameGenerator;
			this.webRequestProtocol = webRequestProtocol;
			this.streamAudio = streamAudio;
		}
		
		public Task<bool> Exists(string uri)
		{
			throw new System.NotImplementedException();
		}

		public async Task<object> Read<T>(string uri)
		{
			var path = Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        
			var requestUri = webRequestProtocol + path;
        
			var uwr = UnityWebRequestMultimedia.GetAudioClip(requestUri, AudioType.MPEG);
        
			((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = streamAudio;
 
			await uwr.SendWebRequest().AsTask(coroutineProxy);
        
			if (uwr.isNetworkError || uwr.isHttpError)
			{
				throw new FileNotFoundException($"File not found for uri: {requestUri}");
			}
            
			var dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;
 
			if (dlHandler.isDone)
			{
				var clip = DownloadHandlerAudioClip.GetContent(uwr);
				clip.name = fileNameGenerator.GetName(uri);
				return clip;
			}
        
			Debug.LogError("The download process is not completely finished.");
			return null;
		}

		public Task<File<T>> Write<T>(string uri, T value)
		{
			throw new System.NotImplementedException();
		}

		public Task Delete(string uri)
		{
			throw new System.NotImplementedException();
		}
	}
}