using System;
using System.IO;
using System.Threading.Tasks;
using Padoru.Core;
using Padoru.Core.Files;
using UnityEngine;
using UnityEngine.Networking;

public class Mp3Serializer : ISerializer
{
    private readonly IFileNameGenerator fileNameGenerator;
    
    public Mp3Serializer(IFileNameGenerator fileNameGenerator)
    {
        this.fileNameGenerator = fileNameGenerator;
    }
    
    public Task<byte[]> Serialize(object value)
    {
        throw new NotImplementedException();
    }

    public Task<object> Deserialize(Type type, byte[] bytes, string uri)
    {
        var clip = AudioUtils.FromWavToAudioClip(fileNameGenerator.GetName(uri), bytes);

        return Task.FromResult<object>(clip);
    }
}
