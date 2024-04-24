using UnityEngine;

public class Sample_Skill_Buff : MonoBehaviour {
    void Start () {
        ConfigManager.Instance.Init ();
    }

    Buff buff;
    // 一个HP相关的Buff
    Buff CreateBuff () {
        BuffConfig bc = ConfigManager.Instance.GetBuffConfig (BuffType.AddHP);
        return new Buff (bc);
    }

    void AddBuff () {
        buff = buff??CreateBuff ();
        BattleUnitManager.Instance.Hero?.AddBuff (buff);
    }

    void RemoveBuff () {
        BattleUnitManager.Instance.Hero?.RemoveBuff (buff);
        buff = null;
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 20;

        if (GUI.Button (new Rect (30, 5, 200, 40), "Create_Hero1")) {
            BattleUnitManager.Instance.CreateHero ();
        }

        if (GUI.Button (new Rect (250, 55, 200, 40), "Add Buff")) {
            AddBuff ();
        }
        if (GUI.Button (new Rect (470, 55, 200, 40), "Remove Buff")) {
            RemoveBuff ();
        }

        if (GUI.Button (new Rect (30, 105, 200, 40), "Kill_Hero1")) {
            BattleUnitManager.Instance.DestroyHero ();
        }
    }

    void Update () {
        BattleUnitManager.Instance.Update ();
    }
}