using System;
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
			int protocolIndex = this.basePath.IndexOf("://");

			if (protocolIndex != -1)
			{
				this.basePath = this.basePath.Substring(protocolIndex + 3);
			}
			this.coroutineProxy = coroutineProxy;
			this.fileNameGenerator = fileNameGenerator;
			this.webRequestProtocol = webRequestProtocol;
			this.streamAudio = streamAudio;
		}
		
		public async Task<bool> Exists(string uri)
		{
			var path = GetFullPath(uri);

			return await Task.FromResult(File.Exists(path));
		}

		public async Task<object> Read<T>(string uri)
		{
			var path = Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        
			var requestUri = webRequestProtocol + path;
        
			var dh = new DownloadHandlerAudioClip(requestUri, AudioType.MPEG);
			dh.compressed = true; // This
 
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

		public async Task<File<T>> Write<T>(string uri, T value)
		{
			var path = GetFullPath(uri);

			var directory = Path.GetDirectoryName(path) ?? ".";
            
			Directory.CreateDirectory(directory);

			var bytes = value as byte[];
			
			await File.WriteAllBytesAsync(path, bytes);
			return new File<T>(uri, value);
		}

		public async Task Delete(string uri)
		{
			var path = GetFullPath(uri);

			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"Could not find file. Uri {uri}");
			}
            
			File.Delete(path);
            
			await Task.CompletedTask;
		}
		
		private string GetFullPath(string uri)
		{
			return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
		}
	}
}
