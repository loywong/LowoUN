using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Module.Sound {
	public class Module_Sound : MonoBehaviour {
		private static Module_Sound _instance;
		public static Module_Sound instance {
			get {
				if (_instance == null)
					Debug.LogError ("====== LowoUN / Module / Sound ===> no sound module found!");

				return _instance;
			}
		}

		[SerializeField] private float _mainSfxVolume = 1.0f;
		[SerializeField] private float _mainBgmVolume = 1.0f;

		[SerializeField] private SoundSet setter_UI;
		[SerializeField] private SoundSet setter_Evt;
		[SerializeField] private SoundSetCollector setters_Act_Collector;

		[SerializeField] private bool isBgmEnable = true;
		[SerializeField] private bool isSfxEnable = true;

		public float mainBgmVolume { get { return _mainBgmVolume; } private set { _mainBgmVolume = value; } }
		//inclule evt and ui type
		public float mainSfxVolume { get { return _mainSfxVolume; } private set { _mainSfxVolume = value; } }

		private AudioSource bgmSource;
		//private bool readyToPlayBgm = false;
		//private bool readyToPlayAmb = false;
		private bool isStopOrPlayCurBgm = false;
		private float fadeVolume = 0.0f;
		private float fadeInTime = 1.0f;
		private float fadeOutTime = 1.0f;
		private Transform listener;
		private Transform followThing = null;

		private string curBgmName = "";

		private bool hasInit = false;

		void Awake () {
			_instance = this;
		}

		void Start () {
			Init ();
		}

		void OnApplicationQuit () {
			DestroyImmediate (listener.gameObject);
		}

		void OnDestroy () {
			_instance = null;
			hasInit = false;
			evts.Clear ();
		}

		private void Init () {
			if (!hasInit) {
				hasInit = true;

				listener = new GameObject ("Listener").transform;
				listener.parent = transform;
				listener.gameObject.AddComponent<AudioListener> ();

				bgmSource = listener.gameObject.AddComponent<AudioSource> ();
				bgmSource.spatialBlend = 0.0f;
				bgmSource.volume = fadeVolume * mainBgmVolume;
			}
		}

		public void Reset () {
			evts.Clear ();
		}

		//0, public settings --------------------------------------------------
		public void SetBgmVolume (float volume) {
			mainBgmVolume = volume;
		}
		public void SetSfxVolume (float volume) {
			mainSfxVolume = volume;
		}
		private void ToggleBgm () {
			isBgmEnable = !isBgmEnable;

			if (!isBgmEnable)
				StopBgm ();
		}
		public void ToggleSfx () {
			isSfxEnable = !isSfxEnable;

			if (!isSfxEnable) {
				StopEvts ();
			}
		}

		//1, bgm ----------------------------------------------------------------
		public void PlayBgm (string bgmBundleName, bool loop = true, float fadeInTime = 1f) {
			if (!isBgmEnable) return;
			if (curBgmName == bgmBundleName) return;

			curBgmName = bgmBundleName;
			float fIn = 1.0f / fadeInTime;
			isStopOrPlayCurBgm = false; //bgmSource.isPlaying;
			StartCoroutine (LoadAndPlayBgm (curBgmName, loop, fIn));
		}

		public void StopBgm () {
			curBgmName = "";
			if (bgmSource.isPlaying)
				isStopOrPlayCurBgm = true;
		}

		private IEnumerator LoadAndPlayBgm (string bundleName, bool loop, float fadeInTime) {
			yield return null;

			Debug.Log ("====== LowoUN / Module / Sound ===> play " + bundleName);

			if (bgmSource.clip != null) {
				Resources.UnloadAsset (bgmSource.clip);
				bgmSource.clip = null;
			}

			bgmSource.clip = Resources.Load (bundleName) as AudioClip;
			this.fadeInTime = fadeInTime;
			bgmSource.loop = loop;
			bgmSource.Play ();
		}

		//2, sfx ------------------------------------------------------------------
		private List<AudioSource> evts = new List<AudioSource> ();
		public void PlayEvt (string groupName, bool loop = false, float fadeTime = 0.5f, GameObject go = null) {
			if (!isSfxEnable) return;

			if (setter_Evt != null) {
				AudioSource s = setter_Evt.Play (groupName, go??gameObject);
				s.spatialBlend = 0.0f;
				setter_Evt.PlayOneShot (s, mainSfxVolume);

				evts.Add (s);
			}
		}

		public void PlayAct (string soundID, string evtName, GameObject go, bool loop = false, float fadeTime = 0.5f) {
			if (!isSfxEnable) return;

			SoundSet setter_act = SoundSetCollector.instance.GetSoundSet ("sfx_" + soundID);
			if (setter_act != null) {
				AudioSource s = setter_act.Play (evtName, gameObject);
				s.spatialBlend = 0.0f;
				s.volume = mainSfxVolume;
			}
		}

		public void StopEvts () {
			if (evts.Count > 0) {
				foreach (var item in evts) {
					StopEvt (item);
				}
			}

			evts.Clear ();
		}
		public void StopEvt (AudioSource s, float fadeOutTime = 0.5f) {
			if (setter_Evt != null)
				setter_Evt.Stop (s, fadeOutTime);
		}

		//3, ui ----------------------------------------------------------------
		public void PlayUI (string groupName, bool isLoop = false) {
			if (!isSfxEnable) return;

			if (setter_UI != null) {
				if (isLoop) {
					setter_UI.PlayLoop (groupName, gameObject);
				} else {
					AudioSource a = setter_UI.Play (groupName, gameObject);
					setter_UI.PlayOneShot (a, 1f);
				}
			}
		}

		private void CheckListenerToFollowSth () {
			if (followThing) {
				listener.position = followThing.position;
				listener.rotation = followThing.rotation;
			} else {
				//need set one camera with the tag "MainCamera"
				if (Camera.main != null) {
					followThing = Camera.main.transform;
				} else {
#if UNITY_EDITOR
					Debug.LogWarning ("=== LowoUN / Module / Sound => No main camera found!");
#endif
				}
			}
		}

		private void Fade4Bgm () {
			if (bgmSource.isPlaying) {
				if (isStopOrPlayCurBgm) {
					if (fadeVolume > 0f) {
						fadeVolume -= Time.deltaTime * fadeOutTime;
						if (fadeVolume < 0f) {
							fadeVolume = 0f;
							bgmSource.Stop ();
							isStopOrPlayCurBgm = false;
						}
					}
				} else {
					if (fadeVolume < 1f) {
						fadeVolume += Time.deltaTime * fadeInTime;
						if (fadeVolume > 1f)
							fadeVolume = 1f;
					}
				}

				bgmSource.volume = fadeVolume * mainBgmVolume;
			}
		}

		// Update is called once per frame
		void Update () {
			CheckListenerToFollowSth ();
			Fade4Bgm ();
		}

		//TEMP audio fade out
		public void PlayFadeAudio (AudioSource source, bool In, float time) {
			StartCoroutine (FadeAudioSource (source, In, time));
		}

		IEnumerator FadeAudioSource (AudioSource source, bool In, float time) {
			float origVolume = source.volume;
			float precent = 0.0f;

			if (In) source.volume = 0.0f;
			while (precent < 1.0f) {
				precent += Time.deltaTime * (1.0f / time);
				if (source == null) yield break;
				source.volume = origVolume * (In ? precent : (1.0f - precent));
				yield return null;
			}
		}
	}
}