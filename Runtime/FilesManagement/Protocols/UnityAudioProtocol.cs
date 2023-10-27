using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Padoru.Core.Files
{
	public class UnityAudioProtocol : IProtocol
	{
		private readonly string basePath;
		private readonly CoroutineProxy coroutineProxy;
		private readonly string webRequestProtocol;
		private readonly bool streamAudio;
		private readonly Regex protocolRegex;

		public UnityAudioProtocol(string basePath, CoroutineProxy coroutineProxy, string webRequestProtocol, bool streamAudio)
		{
			this.basePath = basePath;
			this.coroutineProxy = coroutineProxy;
			this.protocolRegex = new Regex(@"^[a-zA-Z]+://");;
			this.webRequestProtocol = webRequestProtocol;
			this.streamAudio = streamAudio;
		}
		
		public Task<bool> Exists(string uri)
		{
			throw new System.NotImplementedException();
		}

		public async Task<object> Read<T>(string uri)
		{
			var requestUri = GetRequestUri(uri);
			
			Debug.Log($"Sending Get Web Request. Uri: {requestUri}");
        
			var dh = new DownloadHandlerAudioClip(requestUri, AudioType.MPEG);
			dh.compressed = true;
			dh.streamAudio = streamAudio;
 
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

		private string GetRequestUri(string uri)
		{
			var path = GetFullPath(uri);

			if (protocolRegex.IsMatch(path))
			{
				Debug.LogWarning($"Skipped adding web protocol to URI '{path}' because it already has a protocol.");
				return path;
			}
            
			return webRequestProtocol + path;
		}
	}
}
