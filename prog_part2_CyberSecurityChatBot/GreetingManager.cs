using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace prog_part2_CyberSecurityChatBot
{
    public class GreetingManager
    {

        private string audioPath;

        public GreetingManager()
        {
            audioPath = AppDomain.CurrentDomain.BaseDirectory + "greet.wav";
        }

        public void PlayVoiceGreeting()
        {
            try
            {
                if (File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Voice greeting error: {ex.Message}");
            }
        }

        public bool HasAudioFile()
        {
            return File.Exists(audioPath);
        }
    }
}
