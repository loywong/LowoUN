using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LowoUN.Module.Sound {
    [CustomEditor (typeof (SoundSet))]
    public class SoundSetInspector : Editor {
        private SoundSet theScript;
        private int m_Size = 0;
        Vector2 pos;
        Vector2[] s_Pos;
        bool[] showClips;
        int[] s_Size;

        public override void OnInspectorGUI () {
            EditorGUIUtility.LookLikeInspector ();
            theScript = (SoundSet) target;

            theScript.m_pitchRange.m_min = EditorGUILayout.FloatField ("pitchRange min", theScript.m_pitchRange.m_min);
            theScript.m_pitchRange.m_max = EditorGUILayout.FloatField ("pitchRange max", theScript.m_pitchRange.m_max);

            theScript.m_rolloffDistance.m_min = EditorGUILayout.FloatField ("rolloffDistance min", theScript.m_rolloffDistance.m_min);
            theScript.m_rolloffDistance.m_max = EditorGUILayout.FloatField ("rolloffDistance max", theScript.m_rolloffDistance.m_max);

            theScript.m_rolloffMode = (AudioRolloffMode) EditorGUILayout.EnumPopup ("rolloffMode", theScript.m_rolloffMode);

            m_Size = EditorGUILayout.IntField ("Group Size: ", theScript.m_audioGroups.Count);

            if (m_Size != theScript.m_audioGroups.Count || showClips == null) {
                s_Pos = new Vector2[m_Size];
                s_Size = new int[m_Size];
                showClips = new bool[m_Size];
            }

            for (int i = theScript.m_audioGroups.Count; i < m_Size; ++i)
                theScript.m_audioGroups.Add (new SoundGroup ());

            //Used to be: theScript.AudioClips.Length
            List<SoundGroup> tempArr = new List<SoundGroup> ();
            for (int i = 0; i < theScript.m_audioGroups.Count; ++i)
                tempArr.Add (theScript.m_audioGroups[i]);

            int si = theScript.m_audioGroups.Count;
            if (m_Size < theScript.m_audioGroups.Count) {
                si = m_Size;
            }

            theScript.m_audioGroups = new List<SoundGroup> ();
            for (int i = 0; i < si; i++) {
                //When enlarging > more audioclips than before > tempArr is out of range.
                theScript.m_audioGroups.Add (tempArr[i]);
            }
            EditorGUILayout.Space ();
            pos = EditorGUILayout.BeginScrollView (pos);
            for (int i = 0; i < m_Size; i++) {
                EditorGUI.indentLevel = 1;
                showClips[i] = EditorGUILayout.Foldout (showClips[i], "Group " + i + ": " + theScript.m_audioGroups[i].groupName);
                if (showClips[i]) {
                    EditorGUI.indentLevel = 2;
                    theScript.m_audioGroups[i].groupName = EditorGUILayout.TextField ("Name: ", theScript.m_audioGroups[i].groupName);
                    EditorGUILayout.BeginHorizontal ();
                    GUILayout.Label ("       Volume:");
                    EditorGUILayout.Space ();
                    theScript.m_audioGroups[i].Volume = EditorGUILayout.Slider (theScript.m_audioGroups[i].Volume, 0f, 1f, GUILayout.MinWidth (50));
                    EditorGUILayout.EndHorizontal ();
                    s_Size[i] = EditorGUILayout.IntField ("Clip Size: ", theScript.m_audioGroups[i].m_audioClips.Count);

                    for (int j = theScript.m_audioGroups[i].m_audioClips.Count; j < s_Size[i]; ++j)
                        theScript.m_audioGroups[i].m_audioClips.Add (null);

                    //Used to be: theScript.AudioClips.Length
                    List<AudioClip> tempArr1 = new List<AudioClip> ();
                    for (int j = 0; j < theScript.m_audioGroups[i].m_audioClips.Count; ++j)
                        tempArr1.Add (theScript.m_audioGroups[i].m_audioClips[j]);

                    int si1 = theScript.m_audioGroups[i].m_audioClips.Count;
                    if (s_Size[i] < theScript.m_audioGroups[i].m_audioClips.Count) {
                        si1 = s_Size[i];
                    }

                    theScript.m_audioGroups[i].m_audioClips = new List<AudioClip> ();
                    for (int j = 0; j < si1; j++) {
                        //When enlarging > more audioclips than before > tempArr is out of range.
                        theScript.m_audioGroups[i].m_audioClips.Add (tempArr1[j]);
                    }

                    EditorGUI.indentLevel = 3;
                    s_Pos[i] = EditorGUILayout.BeginScrollView (s_Pos[i], GUILayout.Height (0));
                    for (int j = 0; j < theScript.m_audioGroups[i].m_audioClips.Count; j++)
                        theScript.m_audioGroups[i].m_audioClips[j] = (AudioClip) EditorGUILayout.ObjectField ("Clip " + (j + 1) + ":", theScript.m_audioGroups[i].m_audioClips[j], typeof (AudioClip));
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