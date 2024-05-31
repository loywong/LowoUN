using System.Collections.Generic;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Buff {
    public class ConfigManager : Manager<ConfigManager> {
        Dictionary<BattleUnitType, BattleUnitConfig> battleUnits = new ();
        Dictionary<BuffType, BuffConfig> buffs = new ();

        public BattleUnitConfig GetBattleUnitConfig (BattleUnitType bt) {
            BattleUnitConfig bc = default (BattleUnitConfig);
            if (!battleUnits.TryGetValue (bt, out bc)) {
                throw new KeyNotFoundException ($"Key: {bt} was not found in Config Data");
            }
            return bc;
        }

        public override void Init () {
            buffs[BuffType.ReduceHP] = new BuffConfig () { type = BuffType.ReduceHP, actionTimes = 5, actionTimeInterval = 2, actionValue = -6, isAdditive = false };

            // battleUnits[BattleUnitType.Scene] = new BattleUnitConfig () { unitType = BattleUnitType.Scene, camp = UnitCamp.Neutral, name = "Scene" };
            battleUnits[BattleUnitType.Hero1] = new BattleUnitConfig () { unitType = BattleUnitType.Hero1, camp = UnitCamp.Hero, BaseHP = 1000, name = "Hero1" };
            battleUnits[BattleUnitType.Monster1] = new BattleUnitConfig () { unitType = BattleUnitType.Scene, camp = UnitCamp.Monster, BaseHP = 300, name = "Monster1" };

            Debug.Log ($"ConfigManager Init -- Buffs Count is {buffs.Count}, battleUnits Count is {battleUnits.Count}");
        }

        public BuffConfig GetBuffConfig (BuffType bt) {
            BuffConfig bc = default (BuffConfig);
            if (!buffs.TryGetValue (bt, out bc)) {
                throw new KeyNotFoundException ($"Key: {bt} was not found in Config Data");
            }
            return bc;
        }
    }
}