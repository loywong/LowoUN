using System.Collections.Generic;
using UnityEngine;

public class BuffSys {
    public BattleUnit hoster;
    public List<Buff> buffs;
    public BuffSys (BattleUnit unit) {
        hoster = unit;
    }

    public void AddBuff (Buff b) {
        buffs = buffs??new List<Buff> ();

        if (!b.config.isAdditive) {
            // TODO 移除已有的同类型的Buff
        }
        buffs.Add (b);
        Debug.Log ($"AddBuff, Cur buffs Count is {buffs.Count}");

        b.Init (hoster);
        b.StartWork ();
    }

    public void RemoveBuff (Buff b) {
        if (buffs == null || buffs.Count == 0)
            return;

        b.StopWork ();
        buffs.Remove (b);

        Debug.Log ($"RemoveBuff {b.config.type}, buffs Count is {buffs.Count}");
    }

    public void Update () {
        for (int i = 0; i < buffs.Count; i++) {
            buffs[i].Update ();
        }
    }
}