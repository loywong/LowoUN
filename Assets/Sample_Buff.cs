using LowoUN.Buff;
using UnityEngine;

// TEST CASE:A monster cast a buff to Hero
public class Sample_Buff : MonoBehaviour {
    void Start () {
        ConfigManager.Instance.Init ();
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 20;
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;

        if (GUI.Button (new Rect (30, 5, 240, 40), "Create Hero")) {
            BattleUnitManager.Instance.CreateHero ();
        }
        if (GUI.Button (new Rect (300, 5, 240, 40), "Create Monster")) {
            BattleUnitManager.Instance.CreateMonster ();
        }

        if (GUI.Button (new Rect (30, 55, 240, 40), "Monster add Buff to Hero")) {
            if (BattleUnitManager.Instance.monsters.Count == 0)
                Debug.LogError ("Create a monster first!");
            if (BattleUnitManager.Instance.hero == null)
                Debug.LogError ("Create a hero first!");

            if (BattleUnitManager.Instance.monsters.Count > 0 && BattleUnitManager.Instance.hero != null) {
                BuffConfig bc = ConfigManager.Instance.GetBuffConfig (BuffType.ReduceHP);
                var buff = new Buff (BattleUnitManager.Instance.monsters[0], bc);
                BattleUnitManager.Instance.hero.AddBuff (buff);
            }
        }

        if (GUI.Button (new Rect (30, 105, 300, 40), "Kill Monster(Buff Creator)")) {
            if (BattleUnitManager.Instance.monsters.Count > 0)
                BattleUnitManager.Instance.DestroyMonster (BattleUnitManager.Instance.monsters[0]);
        }

        if (GUI.Button (new Rect (30, 155, 200, 40), "Kill_Hero")) {
            BattleUnitManager.Instance.DestroyHero ();
        }
    }

    void Update () {
        BattleUnitManager.Instance.Update ();
    }
}