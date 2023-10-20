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
			this.basePath = FileUtils.PathFromUri(basePath);
			this.coroutineProxy = coroutineProxy;
			this.fileNameGenerator = fileNameGenerator;
			this.webRequestProtocol = webRequestProtocol;
			this.streamAudio = streamAudio;
		}
		
		public Task<bool> Exists(string uri)
		{
			var path = GetFullPath(uri);

			return Task.FromResult(File.Exists(path));
		}

		public async Task<object> Read<T>(string uri)
		{
			var path = Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        
			var requestUri = webRequestProtocol + path;
        
			var dh = new DownloadHandlerAudioClip(requestUri, AudioType.MPEG);
			dh.compressed = true;
 
			using (UnityWebRequest wr = new UnityWebRequest(requestUri, "GET", dh, null)) 
			{
				await wr.SendWebRequest().AsTask(coroutineProxy);
				if (wr.responseCode == 200) 
				{
					return dh.audioClip;
				}
				Debug.LogError($"Download failed. Uri {requestUri} Response {wr.responseCode}. Error: {wr.error}");
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
		
		private string GetFullPath(string uri)
		{
			return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
		}
	}
}
