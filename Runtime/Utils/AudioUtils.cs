using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Padoru.Core.Files
{
    public static class AudioUtils
    {
        #region WAV
        public static byte[] FromClipToWav(AudioClip audioClip)
        {
            float[] samples = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(samples, 0);

            int sampleRate = audioClip.frequency;
            int numChannels = audioClip.channels;

            byte[] wavFile = ToWav(samples, sampleRate, numChannels);

            return wavFile;
        }

        private static byte[] ToWav(float[] samples, int sampleRate, int numChannels)
        {

            int headerSize = 44; // Size of the WAV header
            int totalSize = samples.Length * sizeof(short) + headerSize - 8;

            // Create the WAV header
            byte[] header = new byte[headerSize];
            Buffer.BlockCopy(new[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' }, 0, header, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(totalSize), 0, header, 4, 4);
            Buffer.BlockCopy(
                new[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E', (byte)'f', (byte)'m', (byte)'t', (byte)' ' }, 0,
                header, 8, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(16), 0, header, 16, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, header, 20, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)numChannels), 0, header, 22, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, header, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate * numChannels * sizeof(short)), 0, header, 28, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((short)(numChannels * sizeof(short))), 0, header, 32, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)16), 0, header, 34, 2);
            Buffer.BlockCopy(new[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' }, 0, header, 36, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length * sizeof(short)), 0, header, 40, 4);

            // Create the byte array for the entire WAV file
            byte[] wavFile = new byte[headerSize + samples.Length * sizeof(short)];
            Buffer.BlockCopy(header, 0, wavFile, 0, headerSize);

            // Convert the float samples to short and add them to the byte array
            for (int i = 0; i < samples.Length; i++)
            {
                short sample = (short)(samples[i] * 32767);
                byte[] sampleData = BitConverter.GetBytes(sample);
                Buffer.BlockCopy(sampleData, 0, wavFile, i * 2 + headerSize, 2);
            }

            return wavFile;
        }

        public static AudioClip FromWavToAudioClip(string audioName,byte[] audioData)
        {
            int offset, length, channels, frequency;
            GetWavInfo(audioData,out offset, out length, out channels, out frequency);
            
            // Convert byte array to float array
            float[] samples = ConvertBytesToFloats(audioData, offset, length);

            // Create AudioClip
            AudioClip audioClip = AudioClip.Create(audioName, samples.Length, channels, frequency, false);
            audioClip.SetData(samples, 0);

            return audioClip;
        }
        
        private static void GetWavInfo(byte[] wavData, out int offset, out int length, out int channels, out int frequency)
        {
            offset = BitConverter.ToInt32(wavData, 16);
            length = BitConverter.ToInt32(wavData, 40);
            channels = BitConverter.ToInt16(wavData, 22);
            frequency = BitConverter.ToInt32(wavData, 24);
        }

        private static float[] ConvertBytesToFloats(byte[] audioData, int offset, int length)
        {
            float[] samples = new float[length / 2];

            for (int i = 0; i < length / 2; i++)
            {
                short val = BitConverter.ToInt16(audioData, offset + i * 2);
                samples[i] = val / 32768.0f;
            }

            return samples;
        }
        #endregion

        #region  MP3

        
        private static void GetMp3Info(byte[] mp3Data, out int channels, out int frequency)
        {
            /*using (var ms = new MemoryStream(mp3Data))
            using (var mp3Reader = new Mp3FileReader(ms))
            {
                channels = mp3Reader.WaveFormat.Channels;
                frequency = mp3Reader.WaveFormat.SampleRate;
            }*/
            channels = 0;
            frequency = 0;
        }

        #endregion
    }
}
