using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Sound {
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
						Debug.LogError ("sound_ Duplicate sound set: " + gameSoundSets[i].name + " @" + i);
				}
			}
		}

		public SoundSet GetSoundSet (string name) {
			if (soundSetDict.ContainsKey (name))
				return gameSoundSets[soundSetDict[name]];

			Debug.LogWarning ("sound_ Sound set not added to the SoundSetManager prefab: " + name);

			return null;
		}
	}
}