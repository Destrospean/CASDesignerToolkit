using System;
using System.Collections.Generic;
using System.Destrospean;
using Destrospean.S3PIExtensions;
using s3pi.Interfaces;

namespace Destrospean.DestrospeanCASPEditor
{
    public class AudioPlayer
    {
        readonly Dictionary<string, List<PackageResourceIndexEntryTuple>> mAudioResourcesByMode = new Dictionary<string, List<PackageResourceIndexEntryTuple>>(StringComparer.InvariantCultureIgnoreCase);

        LibVLCSharp.Shared.LibVLC mLibVLC = new LibVLCSharp.Shared.LibVLC(false, "--quiet", "--aout=" + (Platform.IsLinux ? "alsa" : Platform.IsMacOS ? "coreaudio" : Platform.IsWindows ? "waveout" : "oss"));

        LibVLCSharp.Shared.MediaPlayer mMediaPlayer;

        public string CurrentMusicModes;

        public AudioPlayer()
        {
            mMediaPlayer = new LibVLCSharp.Shared.MediaPlayer(mLibVLC);
        }

        public void AddMusic()
        {
            var audioTunerType = ResourceUtils.GetResourceType("AUDT");
            var nameMapDictionary = new Dictionary<ulong, string>();
            foreach (var package in ResourceUtils.GameContentPackages.Values)
            {
                foreach (var nameMapResource in package.GetNameMapResources())
                {
                    foreach (var nameMapKvp in nameMapResource.ToDictionary())
                    {
                        if (nameMapKvp.Value.ToLowerInvariant().StartsWith("music_"))
                        {
                            nameMapDictionary[nameMapKvp.Key] = nameMapKvp.Value;
                        }
                    }
                }
            }
            foreach (var package in ResourceUtils.GameContentPackages.Values)
            {
                foreach (var nameMapKvp in nameMapDictionary)
                {
                    if (!mAudioResourcesByMode.ContainsKey(nameMapKvp.Value))
                    {
                        mAudioResourcesByMode.Add(nameMapKvp.Value, new List<PackageResourceIndexEntryTuple>());
                    }
                    foreach (var resourceIndexEntry in package.FindAll(x => x.ResourceType == audioTunerType && x.Instance == nameMapKvp.Key))
                    {
                        foreach (var block in (new s3piwrappers.AudioTunerResource(0, ((APackage)package).GetResource(resourceIndexEntry))).Blocks)
                        {
                            if (block.Id == s3piwrappers.AudioTunerResource.SoundProperty.Samples)
                            {
                                foreach (var item in block.Items)
                                {
                                    mAudioResourcesByMode[nameMapKvp.Value].AddRange(package.FindAll(x => x.ResourceType == 0x1EEF63A && x.Instance == ((s3piwrappers.AudioTunerResource.SoundKeyData)item).Data.Instance).ConvertAll(x => new PackageResourceIndexEntryTuple(package, x)));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            mAudioResourcesByMode.Clear();
        }

        public void PlayMusic(params string[] modes)
        {
            var audioResources = new List<PackageResourceIndexEntryTuple>();
            foreach (var audioResourceByMode in mAudioResourcesByMode)
            {
                if (modes.Length == 0 || Array.Exists(modes, x => x == audioResourceByMode.Key))
                {
                    audioResources.AddRange(audioResourceByMode.Value);
                }
            }
            Shuffle(audioResources);
            while (true)
            {
                foreach (var audioResource in audioResources)
                {
                    using (var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            Arguments = "-pi -po",
                            CreateNoWindow = true,
                            FileName = "ealayer3",
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        }))
                    {
                        if (process == null)
                        {
                            Logger.WriteError(new Exception("Failed to start the executable."));
                            return;
                        }
                        using (var standardInput = process.StandardInput.BaseStream)
                        {
                            ((APackage)audioResource.Package).GetResource(audioResource.ResourceIndexEntry).CopyTo(standardInput);
                        }
                        using (var outputStream = new System.IO.MemoryStream())
                        {
                            process.StandardOutput.BaseStream.CopyTo(outputStream);
                            outputStream.Position = 0;
                            var wait = true;
                            mMediaPlayer.EndReached += (sender, e) => wait = false;
                            mMediaPlayer.Play(new LibVLCSharp.Shared.Media(mLibVLC, new LibVLCSharp.Shared.StreamMediaInput(outputStream), ":demux=avformat"));
                            while (wait)
                            {
                                System.Threading.Thread.Sleep(1);
                            }
                            process.WaitForExit();
                        }
                    }
                }
            }
        }

        public static void Shuffle<T>(IList<T> list)
        {
            var random = new Random();
            var count = list.Count;
            while (count > 1)
            {
                count--;
                var n = random.Next(count + 1);
                T value = list[n];
                list[n] = list[count];
                list[count] = value;
            }
        }

        public void Stop()
        {
            mMediaPlayer.Stop();
        }
    }
}
