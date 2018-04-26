using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Module.Sound {
	[System.Serializable]
	public class Range {
		public float m_max;
		public float m_min;

		public float max { get { return m_max; } }
		public float min { get { return m_min; } }

		public Range (float min, float max) {
			m_min = min;
			m_max = max;
		}

		public float GetRandom () {
			return Random.Range (m_min, m_max);
		}
	}

	[System.Serializable]
	public class SoundGroup {
		public string groupName = "";
		public List<AudioClip> m_audioClips = new List<AudioClip> ();
		public float Volume = 1f;
	}

	public class SoundSet : MonoBehaviour {
		public Range m_pitchRange = new Range (1f, 1f);
		public Range m_rolloffDistance = new Range (1f, 100f);
		public AudioRolloffMode m_rolloffMode = AudioRolloffMode.Linear;
		public List<SoundGroup> m_audioGroups = new List<SoundGroup> ();
		protected AudioClip currentClip;
		protected SoundGroup currentGroup;

		public void Awake () {
			if (m_audioGroups != null && m_audioGroups.Count > 0)
				currentGroup = m_audioGroups[0];
		}

		public bool SetCurGroup (string groupName) {
			foreach (SoundGroup group in m_audioGroups) {
				if (group.groupName.Equals (groupName, System.StringComparison.OrdinalIgnoreCase)) {
					currentGroup = group;
					return true;
				}
			}

			return false;
		}

		protected AudioClip GetRandomClip () {
			List<AudioClip> list = new List<AudioClip> ();
			foreach (AudioClip clip in currentGroup.m_audioClips) {
				if ((clip != null) && ((clip != currentClip) || (currentGroup.m_audioClips.Count < 2))) {
					list.Add (clip);
				}
			}

			if (list.Count > 0)
				currentClip = list[Random.Range (0, list.Count)];
			else
				currentClip = null;

			return currentClip;
		}

		public bool HasSounds () {
			return (m_audioGroups.Count > 0);
		}

		public AudioSource Play (string GroupName, GameObject sourceObject) {
			return Play (GroupName, sourceObject, Module_Sound.instance.mainSfxVolume, false, true);
		}

		public AudioSource Play (string GroupName, Vector3 position) {
			return Play (GroupName, position, Module_Sound.instance.mainSfxVolume, false);
		}

		private void Play (AudioSource source, float volume) {
			//SoundVolumeControl controller = null;
			if (SetupAudioSource (source, volume /*, out controller*/ )) {
				source.Play ();
			}
		}

		public AudioSource Play (string GroupName, GameObject sourceObject, bool attachToObject) {
			return Play (GroupName, sourceObject, Module_Sound.instance.mainSfxVolume, false, attachToObject);
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
			obj2.name = obj2.name + ": " + (!loop ? "Clip[" : ", Looping clip[") + ((currentClip == null) ? "null]" : (currentClip.name + "]"));
			if (currentClip == null) {
				Object.Destroy (obj2);
				return null;
			}

			if (loop) {
				source.loop = true;
				return source;
			}

			Object.Destroy (obj2, currentClip.length);

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

			if (m_audioGroups.Count > 0) {
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
			return Play (GroupName, sourceObject, Module_Sound.instance.mainSfxVolume, true, true);
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
			source.volume = currentGroup.Volume * volume; //1f;
			source.pitch = m_pitchRange.GetRandom ();
			source.dopplerLevel = 0f;
			source.minDistance = m_rolloffDistance.min;
			source.maxDistance = m_rolloffDistance.max;
			source.rolloffMode = m_rolloffMode;
			source.playOnAwake = false;
			return true;
		}

		public void Stop (AudioSource s, float fadeOutTime) {
			if (s != null && s.isPlaying)
				Module_Sound.instance.PlayFadeAudio (s, false, fadeOutTime); //s.Stop();
		}
	}
}