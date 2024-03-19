using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Sound {
	[System.Serializable]
	public class Range {
		public float max;
		public float min;

		public Range (float min, float max) {
			this.min = min;
			this.max = max;
		}

		public float GetRandom () {
			return Random.Range (min, max);
		}
	}

	[System.Serializable]
	public class SoundGroup {
		public string groupName = "";
		public List<AudioClip> audioClips = new List<AudioClip> ();
		public float Volume = 1f;
	}

	public class SoundSet : MonoBehaviour {
		public Range pitchRange = new Range (1f, 1f);
		public Range rolloffDistance = new Range (1f, 100f);
		public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
		public List<SoundGroup> audioGroups = new List<SoundGroup> ();
		protected AudioClip curClip;
		protected SoundGroup curGroup;

		public void Awake () {
			if (audioGroups != null && audioGroups.Count > 0)
				curGroup = audioGroups[0];
		}

		public bool SetCurGroup (string groupName) {
			foreach (SoundGroup group in audioGroups) {
				if (group.groupName.Equals (groupName, System.StringComparison.OrdinalIgnoreCase)) {
					curGroup = group;
					return true;
				}
			}

			return false;
		}

		protected AudioClip GetRandomClip () {
			List<AudioClip> list = new List<AudioClip> ();
			foreach (AudioClip clip in curGroup.audioClips) {
				if ((clip != null) && ((clip != curClip) || (curGroup.audioClips.Count < 2))) {
					list.Add (clip);
				}
			}

			if (list.Count > 0)
				curClip = list[Random.Range (0, list.Count)];
			else
				curClip = null;

			return curClip;
		}

		public bool HasSounds () {
			return (audioGroups.Count > 0);
		}

		public AudioSource Play (string GroupName, GameObject sourceObject) {
			return Play (GroupName, sourceObject, SoundManager.Instance.sfxVolume, false, true);
		}

		public AudioSource Play (string GroupName, Vector3 position) {
			return Play (GroupName, position, SoundManager.Instance.sfxVolume, false);
		}

		private void Play (AudioSource source, float volume) {
			//SoundVolumeControl controller = null;
			if (SetupAudioSource (source, volume /*, out controller*/ )) {
				source.Play ();
			}
		}

		public AudioSource Play (string GroupName, GameObject sourceObject, bool attachToObject) {
			return Play (GroupName, sourceObject, SoundManager.Instance.sfxVolume, false, attachToObject);
		}

		public AudioSource Play (string GroupName, GameObject sourceObject, float volume) {
			return Play (GroupName, sourceObject, volume, false, true);
		}

		public AudioSource Play (string GroupName, Vector3 position, float volume) {
			return Play (GroupName, position, volume, false);
		}

		private AudioSource Play (string GroupName, Vector3 position, float volume, bool loop) {
			if (!HasSounds ())
				return null;

			if (!SetCurGroup (GroupName))
				return null;

			GameObject obj2 = new GameObject ("AudioSource") { transform = { position = position } };
			AudioSource source = obj2.AddComponent<AudioSource> ();
			Play (source, volume);
			obj2.name = obj2.name + ": " + (!loop ? "Clip[" : ", Looping clip[") + ((curClip == null) ? "null]" : (curClip.name + "]"));
			if (curClip == null) {
				Object.Destroy (obj2);
				return null;
			}

			if (loop) {
				source.loop = true;
				return source;
			}

			Object.Destroy (obj2, curClip.length);

			return source;
		}

		//for 3d world game object
		private AudioSource Play (string GroupName, GameObject sourceObject, float volume, bool loop, bool attachToObject) {
			AudioSource source = Play (GroupName, sourceObject.transform.position, volume, loop);
			if (source != null) {
				if (attachToObject)
					source.transform.parent = sourceObject.transform;

				source.name = source.name + " Spawned from " + ((sourceObject == null) ? "null" : sourceObject.name);

				return source;
			}

			if (audioGroups.Count > 0) {
				string name;
				if (sourceObject == null) {
					name = "[Null source object specified]!";
					return source;
				}
				name = sourceObject.name;

				for (Transform transform = sourceObject.transform; transform.parent != null; transform = transform.parent)
					name = transform.parent.name + "/" + name;

				name = name + ".";
			}

			return source;
		}

		public AudioSource PlayLoop (string GroupName, GameObject sourceObject) {
			return Play (GroupName, sourceObject, SoundManager.Instance.sfxVolume, true, true);
		}

		public void PlayOneShot (AudioSource source, float volume) {
			//SoundVolumeControl controller = null;
			if (SetupAudioSource (source, volume /*, out controller*/ ))
				source.PlayOneShot (source.clip);
		}

		private bool SetupAudioSource (AudioSource source, float volume /*, out SoundVolumeControl controller*/ ) {
			if ((source == null) || !HasSounds ()) {
				//controller = null;
				return false;
			}

			source.clip = GetRandomClip ();
			if (source.clip == null) {
				string name = source.name;
				for (Transform transform = source.transform; transform.parent != null; transform = transform.parent) {
					name = transform.parent.name + "/" + name;
				}
				return false;
			}

			source.spatialBlend = 0;
			source.volume = curGroup.Volume * volume;
			source.pitch = pitchRange.GetRandom ();
			source.dopplerLevel = 0f;
			source.minDistance = rolloffDistance.min;
			source.maxDistance = rolloffDistance.max;
			source.rolloffMode = rolloffMode;
			source.playOnAwake = false;

			return true;
		}

		public void Stop (AudioSource s, float fadeOutTime) {
			if (s != null && s.isPlaying) {
				//s.Stop();
				SoundManager.Instance.PlayFadeAudio (s, false, fadeOutTime);
			}
		}
	}
}