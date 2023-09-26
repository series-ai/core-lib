using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Padoru.Core.Files;
using System.IO;
using Padoru.Core;
using UnityEngine;
using UnityEngine.Networking;
using Debug = Padoru.Diagnostics.Debug;

public class UnityAudioFileSystem : IFileSystem
{
    //TODO: We should support a file system that also works as serializer in the file manager(like an all-in-one option)
    //We need an all-in-one solution for 2 main reasons. First, the audio is not being streamed, which means it gets fully loaded on memory
    //from the start, which isn't great. Second, the current solution forces us into converting the audio clip into byte[]
    //and then deserializing it again, which consumes cpu that wouldn't be needed if we just had an all-in-one solution
    private readonly string basePath;
    private readonly CoroutineProxy coroutineProxy;
    private readonly IFileNameGenerator fileNameGenerator;
    private readonly string webRequestProtocol;

    public UnityAudioFileSystem(string basePath, CoroutineProxy coroutineProxy, IFileNameGenerator fileNameGenerator, string webRequestProtocol = "file://")
    {
        this.basePath = basePath;
        this.coroutineProxy = coroutineProxy;
        this.fileNameGenerator = fileNameGenerator;
        this.webRequestProtocol = webRequestProtocol;
    }

    public async Task<bool> Exists(string uri)
    {
        throw new NotImplementedException();
    }

    public async Task<File<byte[]>> Read(string uri)
    {
        var path = GetFullPath(uri);
        
        var requestUri = webRequestProtocol + path;
        
        var uwr = UnityWebRequestMultimedia.GetAudioClip(requestUri, AudioType.MPEG);
        
        ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = false;//TODO: set to true when we support an all-in-one solution
 
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
            return new File<byte[]>(uri ,AudioUtils.FromClipToWav(clip));
        }
        
        Debug.Log("The download process is not completely finished.");
            
        return null;
    }

    public async Task Write(File<byte[]> file)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(string uri)
    {
        throw new NotImplementedException();
    }

    private string GetFullPath(string uri)
    {
        return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
    }
}
