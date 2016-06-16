using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.Sound;
using Element.ResourceManagement.RegionGeneration;

namespace Element.ResourceManagement.Sound
{
    public static class SoundMapper
    {
        private static Dictionary<SoundName, List<RegionNames>> _soundRegions; 
        private static Dictionary<SoundName, string> _soundFiles;

        static SoundMapper()
        {
            _soundRegions = new Dictionary<SoundName, List<RegionNames>>();
            _soundFiles = new Dictionary<SoundName, string>();
        }

        public static List<SoundName> GetCrossRegionSoundEffects(List<RegionNames> regions)
        {
            var soundNames = new List<SoundName>();

            foreach (var region in regions)
            {
                var soundsToAdd = RegionFactory.GetInfoForRegion(region).CrossRegionSounds;

                foreach (var sound in soundsToAdd)
                {
                    if (!soundNames.Contains(sound))
                        soundNames.Add(sound);
                }
            }

            return soundNames;
        }

        public static List<RegionNames> GetRegionsForSound(SoundName sound)
        {
            return _soundRegions[sound];
        }

        public static List<CrossRegionContent> CreateCrossRegionSoundContent(IServiceProvider serviceProvider, string rootDirectory, List<SoundName> soundsToCreate)
        {
            var loadedContent = new List<CrossRegionContent>();

            foreach (var sound in soundsToCreate)
            {
                var content = new CrossRegionSoundContent();
                var contentManager = new ContentManager(serviceProvider, rootDirectory);
                var soundEffect = contentManager.Load<SoundEffect>(_soundFiles[sound]);
                content.Id = sound;
                content.ContentManager = contentManager;
                content.Sound = soundEffect;
                loadedContent.Add(content);
            }

            return loadedContent;
        }

        public static Dictionary<SoundName, SoundEffect> LoadSoundsForRegion(ContentManager contentManager, RegionNames region)
        {
            var soundDictionary = new Dictionary<SoundName, SoundEffect>();

            var sounds = RegionFactory.GetInfoForRegion(region).RegionSounds;

            foreach (var sound in sounds)
                soundDictionary.Add(sound, contentManager.Load<SoundEffect>(_soundFiles[sound]));

            return soundDictionary;
        }
    }
}
