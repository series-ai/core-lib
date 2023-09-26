using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
	public class WavSerializer : ISerializer
	{
		private readonly IFileNameGenerator fileNameGenerator;

		public WavSerializer(IFileNameGenerator fileNameGenerator)
		{
			this.fileNameGenerator = fileNameGenerator;
		}

		public Task<byte[]> Serialize(object value)
		{
			var clip = (AudioClip)value;

			var bytes = AudioUtils.FromClipToWav(clip);
			
			return Task.FromResult(bytes);
		}
		
		public Task<object> Deserialize(Type type, byte[] bytes, string uri)
		{
			var name = fileNameGenerator.GetName(uri);
			
			var clip = AudioUtils.FromWavToAudioClip(name, bytes);
			object value = clip;
			return Task.FromResult(value);
		}
	}
}
