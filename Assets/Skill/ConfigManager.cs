using System.Collections.Generic;
using LowoUN.Util;
using UnityEngine;

public class ConfigManager : Manager<ConfigManager> {
    Dictionary<BattleUnitType, BattleUnitConfig> battleUnits = new ();
    public BattleUnitConfig GetBattleUnitConfig (BattleUnitType bt) {
        BattleUnitConfig bc = default (BattleUnitConfig);
        if (!battleUnits.TryGetValue (bt, out bc)) {
            throw new KeyNotFoundException ($"Key: {bt} was not found in Config Data");
        }
        return bc;
    }

    Dictionary<BuffType, BuffConfig> buffs = new ();
    public override void Init () {
        buffs = new ();
        buffs[BuffType.AddHP] = new BuffConfig () { type = BuffType.AddHP, actionTimeType = -1, actionTimeInterval = 3, isAdditive = false };
        Debug.Log ($"Config Buffs Count is {buffs.Count}");

        battleUnits = new ();
        battleUnits[BattleUnitType.Hero1] = new BattleUnitConfig () { unitType = BattleUnitType.Hero1, camp = UnitCamp.Hero, name = "Hero1" };
    }

    public BuffConfig GetBuffConfig (BuffType bt) {
        BuffConfig bc = default (BuffConfig);
        if (!buffs.TryGetValue (bt, out bc)) {
            throw new KeyNotFoundException ($"Key: {bt} was not found in Config Data");
        }
        return bc;
    }
}