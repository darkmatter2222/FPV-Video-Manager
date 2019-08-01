using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using NAudio.Wave;

namespace FPV_Video_Manager.AudioPlayer
{
    class AudioManager
    {
        public enum AudioFile { OneFR, TwoFR, ThreeFR, FourFR, FiveFR, SixFR, SevenFR, EightFR, NineFR, TenFR, MoreTenFR, ContentDiscovered, FileMoved }

        public void PlayFile(AudioFile audioFile)
        {
            string fileName = "";
           
            switch (audioFile)
            {
                case AudioFile.OneFR:
                    fileName = "OneFileRemaining.wav";
                    break;
                case AudioFile.TwoFR:
                    fileName = "TwoFilesRemaining.wav";
                    break;
                case AudioFile.ThreeFR:
                    fileName = "ThreeFilesRemaining.wav";
                    break;
                case AudioFile.FourFR:
                    fileName = "FourFilesRemaining.wav";
                    break;
                case AudioFile.FiveFR:
                    fileName = "FiveFilesRemaining.wav";
                    break;
                case AudioFile.SixFR:
                    fileName = "SixFilesRemaining.wav";
                    break;
                case AudioFile.SevenFR:
                    fileName = "SevenFilesRemaining.wav";
                    break;
                case AudioFile.EightFR:
                    fileName = "EightFilesRemaining.wav";
                    break;
                case AudioFile.NineFR:
                    fileName = "NineFilesRemaining.wav";
                    break;
                case AudioFile.TenFR:
                    fileName = "TenFilesRemaining.wav";
                    break;
                case AudioFile.MoreTenFR:
                    fileName = "MoreThanTenFilesRemaining.wav";
                    break;
                case AudioFile.ContentDiscovered:
                    fileName = "ContentDiscovered.wav";
                    break;
                case AudioFile.FileMoved:
                    fileName = "FileMoved.wav";
                    break;
            }

            object FilePath = $@"VoiceFiles\{fileName}";

            new Thread(PlayFile).Start(FilePath);
        }

        private static void PlayFile(object Filepath)
        {
            using (var ms = File.OpenRead(Filepath.ToString()))
            using (var rdr = new Mp3FileReader(ms))
            using (var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr))
            using (var baStream = new BlockAlignReductionStream(wavStream))
            using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
            {
                waveOut.Init(baStream);
                waveOut.Play();
                while (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
