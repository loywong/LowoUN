using System.Collections;
using System.Collections.Generic;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Module.Sound {
	public class SoundSetCollector : MonoBehaviour {
		public static SoundSetCollector instance;
		
		[SerializeField] private SoundSet[] gameSoundSets;
		protected Dictionary<string, int> soundSetDict = new Dictionary<string, int> ();

		void Awake () {
			instance = this;

			for (int i = 0; i < gameSoundSets.Length; ++i) {
				if (gameSoundSets[i] != null) {
					if (!soundSetDict.ContainsKey (gameSoundSets[i].name))
						soundSetDict.Add (gameSoundSets[i].name, i);
					else
						Log.Error ("sound", "Duplicate sound set: " + gameSoundSets[i].name + " @" + i);
				}
			}
		}

		public SoundSet GetSoundSet (string name) {
			if (soundSetDict.ContainsKey (name))
				return gameSoundSets[soundSetDict[name]];
			
			Log.Warn ("sound", "Sound set not added to the SoundSetManager prefab: " + name);

			return null;
		}
	}
}