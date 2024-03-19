using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Sound {
	public class SoundManager : MonoBehaviour {
		private static SoundManager _Instance;
		public static SoundManager Instance {
			get {
				if (_Instance == null)
					Debug.Log ("sound_ No sound module found!");

				return _Instance;
			}
		}

		[SerializeField] float m_sfxVolume = 1.0f;
		[SerializeField] float m_bgmVolume = 1.0f;
		[SerializeField] SoundSet m_setterUI;
		[SerializeField] SoundSet m_setterEvt;
		[SerializeField] SoundSetCollector m_settersActor;
		[SerializeField] bool m_isBgmEnable = true;
		[SerializeField] bool m_isSfxEnable = true;

		//inclule evt and ui type
		public float sfxVolume { get { return m_sfxVolume; } private set { m_sfxVolume = value; } }

		public float bgmVolume { get { return m_bgmVolume; } private set { m_bgmVolume = value; } }
		private AudioSource bgmSource;
		private bool isStopOrPlayCurBgm = false;
		private float fadeVolume = 0.0f;
		private float fadeInTime = 1.0f;
		private float fadeOutTime = 1.0f;
		private Transform listener;
		private Transform followThing = null;
		private string curBgmName = "";

		private bool hasInit = false;

		void Awake () {
			_Instance = this;
		}

		void Start () {
			OnInit ();
		}

		void OnApplicationQuit () {
			DestroyImmediate (listener.gameObject);
		}

		void OnDestroy () {
			_Instance = null;
			hasInit = false;
			evts.Clear ();
		}

		private void OnInit () {
			if (hasInit)
				return;

			hasInit = true;

			listener = new GameObject ("Listener").transform;
			listener.parent = transform;
			listener.gameObject.AddComponent<AudioListener> ();

			bgmSource = listener.gameObject.AddComponent<AudioSource> ();
			bgmSource.spatialBlend = 0.0f;
			bgmSource.volume = fadeVolume * bgmVolume;
		}

		public void Reset () {
			evts.Clear ();
		}

		//0, public settings --------------------------------------------------
		public void SetBgmVolume (float volume) {
			bgmVolume = volume;
		}

		public void SetSfxVolume (float volume) {
			sfxVolume = volume;
		}

		public void ToggleBgm () {
			m_isBgmEnable = !m_isBgmEnable;

			if (!m_isBgmEnable)
				StopBgm ();
		}
		public void ToggleSfx () {
			m_isSfxEnable = !m_isSfxEnable;

			if (!m_isSfxEnable) {
				StopEvts ();
			}
		}

		//1, bgm ----------------------------------------------------------------
		public void PlayBgm (string bgmBundleName, bool loop = true, float fadeInTime = 1f) {
			if (!m_isBgmEnable)
				return;
			if (curBgmName == bgmBundleName)
				return;

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

			Debug.Log ("sound_ LoadAndPlayBgm with bundleName: " + bundleName);

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
			if (!m_isSfxEnable)
				return;

			if (m_setterEvt != null) {
				AudioSource s = m_setterEvt.Play (groupName, go??gameObject);
				s.spatialBlend = 0.0f;
				m_setterEvt.PlayOneShot (s, sfxVolume);

				evts.Add (s);
			}
		}

		public void PlayAct (string soundID, string evtName, GameObject go, bool loop = false, float fadeTime = 0.5f) {
			if (!m_isSfxEnable)
				return;

			SoundSet setter_act = m_settersActor.GetSoundSet ("sfx_" + soundID);
			if (setter_act != null) {
				AudioSource s = setter_act.Play (evtName, gameObject);
				s.spatialBlend = 0.0f;
				s.volume = sfxVolume;
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
			if (m_setterEvt != null)
				m_setterEvt.Stop (s, fadeOutTime);
		}

		//3, ui ----------------------------------------------------------------
		public void PlayUI (string groupName, bool isLoop = false) {
			if (!m_isSfxEnable)
				return;

			if (m_setterUI != null) {
				if (isLoop) {
					m_setterUI.PlayLoop (groupName, gameObject);
				} else {
					AudioSource a = m_setterUI.Play (groupName, gameObject);
					m_setterUI.PlayOneShot (a, 1f);
				}
			}
		}

		private void CheckListenerToFollowSth () {
			if (followThing == null) {
				//need set one camera with the tag "MainCamera"
				if (Camera.main != null) {
					followThing = Camera.main.transform;
				}
			}

			if (followThing != null) {
				listener.position = followThing.position;
				listener.rotation = followThing.rotation;
			}
			// else {
			// 	Log.Warn ("sound", "No followThing found!");
			// }
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

				bgmSource.volume = fadeVolume * bgmVolume;
			}
		}

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