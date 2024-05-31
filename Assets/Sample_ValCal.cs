using LowoUN.ValCal;
using UnityEngine;

public class Sample_ValCal : MonoBehaviour {
    HeroA heroA;

    void CreateHero () {
        heroA = heroA?? new HeroA ();
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 20;
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;

        if (GUI.Button (new Rect (30, 5, 240, 40), "Create Hero")) {
            CreateHero ();
        }

        if (GUI.Button (new Rect (30, 55, 240, 40), "Equip Item Ring")) {
            CreateHero ();
            heroA.EquipItem_Ring ();
        }

        if (GUI.Button (new Rect (30, 105, 300, 40), "Equip Item Shoes")) {
            CreateHero ();
            heroA.EquipItem_Shoes ();
        }

        if (GUI.Button (new Rect (30, 155, 200, 40), "Unequip Item Shoes")) {
            if (heroA != null)
                heroA.UnequipItem_Shoes ();
            else
                Debug.LogError ("Create a Hero first!");
        }
        if (GUI.Button (new Rect (30, 205, 200, 40), "Unequip Item Ring")) {
            if (heroA != null)
                heroA.UnequipItem_Ring ();
            else
                Debug.LogError ("Create a Hero first!");
        }
    }
}