using System.Collections.Generic;
using System.Media;

namespace Flappy_Bird.Managers
{
    public class SoundManager
    {
        // SoundPlayer for each different sound. Therefore, we can play multiple sounds simultaneously without waiting for one to finish before starting another.
        private readonly Dictionary<string, SoundPlayer> _players;

        // Sound ON/OFF
        public bool IsMuted { get; set; } = false;

        public SoundManager()
        {
            _players = new Dictionary<string, SoundPlayer>
            {
                { "wing",   new SoundPlayer("Assets/Audio/wing.wav")   },
                { "hit",    new SoundPlayer("Assets/Audio/hit.wav")    },
                { "die",    new SoundPlayer("Assets/Audio/die.wav")    },
                { "point",  new SoundPlayer("Assets/Audio/point.wav")  },
                { "swoosh", new SoundPlayer("Assets/Audio/swoosh.wav") }
            };

            // Upload all sounds into memory to prevent delays during gameplay when a sound is played for the first time.
            foreach (var player in _players.Values)
                player.Load();
        }

        public void Play(string soundName)
        {
            if (IsMuted) return;

            if (_players.TryGetValue(soundName, out SoundPlayer player))
                player.Play();
        }
    }
}