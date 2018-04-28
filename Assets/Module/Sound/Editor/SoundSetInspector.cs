using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LowoUN.Module.Sound {
    [CustomEditor (typeof (SoundSet))]
    public class SoundSetInspector : Editor {
        private SoundSet theScript;
        private int size = 0;
        private Vector2 pos;
        private Vector2[] poses;
        private bool[] showClips;
        private int[] sizes;

        public override void OnInspectorGUI () {
            EditorGUIUtility.LookLikeInspector ();
            theScript = (SoundSet) target;

            theScript.pitchRange.min = EditorGUILayout.FloatField ("pitchRange min", theScript.pitchRange.min);
            theScript.pitchRange.max = EditorGUILayout.FloatField ("pitchRange max", theScript.pitchRange.max);

            theScript.rolloffDistance.min = EditorGUILayout.FloatField ("rolloffDistance min", theScript.rolloffDistance.min);
            theScript.rolloffDistance.max = EditorGUILayout.FloatField ("rolloffDistance max", theScript.rolloffDistance.max);

            theScript.rolloffMode = (AudioRolloffMode) EditorGUILayout.EnumPopup ("rolloffMode", theScript.rolloffMode);

            size = EditorGUILayout.IntField ("Group Size: ", theScript.audioGroups.Count);

            if (size != theScript.audioGroups.Count || showClips == null) {
                poses = new Vector2[size];
                sizes = new int[size];
                showClips = new bool[size];
            }

            for (int i = theScript.audioGroups.Count; i < size; ++i)
                theScript.audioGroups.Add (new SoundGroup ());

            //Used to be: theScript.AudioClips.Length
            List<SoundGroup> tempArr = new List<SoundGroup> ();
            for (int i = 0; i < theScript.audioGroups.Count; ++i)
                tempArr.Add (theScript.audioGroups[i]);

            int si = theScript.audioGroups.Count;
            if (size < theScript.audioGroups.Count) {
                si = size;
            }

            theScript.audioGroups = new List<SoundGroup> ();
            for (int i = 0; i < si; i++) {
                //When enlarging > more audioclips than before > tempArr is out of range.
                theScript.audioGroups.Add (tempArr[i]);
            }
            EditorGUILayout.Space ();
            pos = EditorGUILayout.BeginScrollView (pos);
            for (int i = 0; i < size; i++) {
                EditorGUI.indentLevel = 1;
                showClips[i] = EditorGUILayout.Foldout (showClips[i], "Group " + i + ": " + theScript.audioGroups[i].groupName);
                if (showClips[i]) {
                    EditorGUI.indentLevel = 2;
                    theScript.audioGroups[i].groupName = EditorGUILayout.TextField ("Name: ", theScript.audioGroups[i].groupName);
                    EditorGUILayout.BeginHorizontal ();
                    GUILayout.Label ("       Volume:");
                    EditorGUILayout.Space ();
                    theScript.audioGroups[i].Volume = EditorGUILayout.Slider (theScript.audioGroups[i].Volume, 0f, 1f, GUILayout.MinWidth (50));
                    EditorGUILayout.EndHorizontal ();
                    sizes[i] = EditorGUILayout.IntField ("Clip Size: ", theScript.audioGroups[i].audioClips.Count);

                    for (int j = theScript.audioGroups[i].audioClips.Count; j < sizes[i]; ++j)
                        theScript.audioGroups[i].audioClips.Add (null);

                    //Used to be: theScript.AudioClips.Length
                    List<AudioClip> tempArr1 = new List<AudioClip> ();
                    for (int j = 0; j < theScript.audioGroups[i].audioClips.Count; ++j)
                        tempArr1.Add (theScript.audioGroups[i].audioClips[j]);

                    int si1 = theScript.audioGroups[i].audioClips.Count;
                    if (sizes[i] < theScript.audioGroups[i].audioClips.Count) {
                        si1 = sizes[i];
                    }

                    theScript.audioGroups[i].audioClips = new List<AudioClip> ();
                    for (int j = 0; j < si1; j++) {
                        //When enlarging > more audioclips than before > tempArr is out of range.
                        theScript.audioGroups[i].audioClips.Add (tempArr1[j]);
                    }

                    EditorGUI.indentLevel = 3;
                    poses[i] = EditorGUILayout.BeginScrollView (poses[i], GUILayout.Height (0));
                    for (int j = 0; j < theScript.audioGroups[i].audioClips.Count; j++)
                        theScript.audioGroups[i].audioClips[j] = (AudioClip) EditorGUILayout.ObjectField ("Clip " + (j + 1) + ":", theScript.audioGroups[i].audioClips[j], typeof (AudioClip));
                    EditorGUILayout.EndScrollView ();
                }
            }
            EditorGUILayout.EndScrollView ();

            //DrawDefaultInspector();
            if (GUI.changed) {
                EditorUtility.SetDirty (target);
            }
        }
    }
}