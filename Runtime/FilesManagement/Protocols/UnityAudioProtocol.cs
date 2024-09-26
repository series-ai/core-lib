using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
	public class UnityAudioProtocol : IProtocol
	{
		private readonly string basePath;
		private readonly CoroutineProxy coroutineProxy;
		private readonly string webRequestProtocol;
		private readonly AudioImportSettings importSettings;
		private readonly Regex protocolRegex;

		public UnityAudioProtocol(string basePath, CoroutineProxy coroutineProxy, string webRequestProtocol, AudioImportSettings importSettings)
		{
			this.basePath = basePath;
			this.coroutineProxy = coroutineProxy;
			this.protocolRegex = new Regex(Constants.PROTOCOL_REGEX_PATTERN);
			this.webRequestProtocol = webRequestProtocol;
			this.importSettings = importSettings;
		}
		
		public Task<bool> Exists(string uri, CancellationToken cancellationToken)
		{
			var filePath = GetFullPath(uri);

			return Task.FromResult(File.Exists(filePath));
		}

		public async Task<object> Read<T>(string uri, CancellationToken cancellationToken, string version = null)
		{
			var requestUri = GetRequestUri(uri);
        
			// Cannot compress audio if stream is set to true
			var dh = new DownloadHandlerAudioClip(requestUri, AudioType.MPEG);
			dh.streamAudio = importSettings.StreamAudio;
			dh.compressed = !importSettings.StreamAudio && importSettings.CompressAudio;
 
			using (UnityWebRequest wr = new UnityWebRequest(requestUri, "GET", dh, null)) 
			{
				await wr.SendWebRequest().AsTask(coroutineProxy, cancellationToken);
				if (wr.responseCode == 200) 
				{
					return dh.audioClip;
				}
				
				Debug.LogError($"Download failed. Uri {requestUri} Response {wr.responseCode}. Error: {wr.error}", Constants.DEBUG_CHANNEL_NAME);
			}
        
			Debug.LogError("The download process is not completely finished.", Constants.DEBUG_CHANNEL_NAME);
			
			return null;
		}

		public Task<File<T>> Write<T>(string uri, T value, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public Task Delete(string uri, CancellationToken cancellationToken)
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
				return path;
			}
            
			return webRequestProtocol + path;
		}
	}
}
