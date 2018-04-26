using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Module.Sound {
	public class SoundSetCollector : MonoBehaviour {
		[SerializeField] private SoundSet[] GameSoundSets;

		protected Dictionary<string, int> soundSetDict = new Dictionary<string, int> ();

		public static SoundSetCollector instance;

		void Awake () {
			instance = this;

			for (int i = 0; i < GameSoundSets.Length; ++i) {
				if (GameSoundSets[i] != null) {
					if (!soundSetDict.ContainsKey (GameSoundSets[i].name))
						soundSetDict.Add (GameSoundSets[i].name, i);
					else
						Debug.LogError ("====== LowoUN / Module / Sound ===> Duplicate sound set: " + GameSoundSets[i].name + " @" + i);
				}
			}
		}

		public SoundSet GetSoundSet (string name) {
			SoundSet t = null;

			if (soundSetDict.ContainsKey (name))
				t = GameSoundSets[soundSetDict[name]];
			else
				Debug.LogWarning ("====== LowoUN / Module / Sound ===> Sound set not added to the SoundSetManager prefab: " + name);

			return t;
		}
	}
}