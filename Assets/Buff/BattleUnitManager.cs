using System.Collections.Generic;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Buff {
    class BattleUnitManager : Manager<BattleUnitManager> {
        // public BattleUnit scene;
        public BattleUnit hero { private set; get; }
        public List<BattleUnit> monsters { private set; get; } = new List<BattleUnit> ();

        void UI_Refresh_HP_1 () {
            Debug.Log ($"UI_Refresh_HP_1 curHP {hero.profile.curHP}");
        }
        void UI_Refresh_HP_2 () {
            Debug.Log ($"UI_Refresh_HP_2 curHP {hero.profile.curHP}");
        }

        public void CreateMonster () {
            BattleUnitConfig buc = ConfigManager.Instance.GetBattleUnitConfig (BattleUnitType.Monster1);
            var monster = new BattleUnit (buc, 1);
            monsters.Add (monster);
            Debug.Log ($"CreateMonster, Monster curHP is {monster.profile.curHP}, monsters count is {monsters.Count}");
        }

        public void CreateHero () {
            BattleUnitConfig buc = ConfigManager.Instance.GetBattleUnitConfig (BattleUnitType.Hero1);
            hero = new BattleUnit (buc, 1);
            hero.onPropertyChange_Hp += UI_Refresh_HP_1;
            hero.onPropertyChange_Hp += UI_Refresh_HP_2;
            Debug.Log ($"CreateHero, Hero curHP is {hero.profile.curHP}");
        }

        public void DestroyHero () {
            if (hero != null) {
                Debug.Log ("Destroy Hero1 Succ");
                // Hero.onPropertyChange_Hp -= UI_Refresh_HP_1;
                // Hero.onPropertyChange_Hp -= UI_Refresh_HP_1;
                hero = null;
            }
        }

        public void DestroyMonster (BattleUnit m) {
            Debug.Log ($"DestroyMonster, name is {m.config.name}");
            m.SetState (BattleUnitState.Dead);
            BuffManager.Instance.RemoveBuff_CreatorDie (m);
            monsters.Remove (m);
            Debug.Log ($"monsters count is {monsters.Count}");
        }

        public void Update () {
            hero?.Update ();

            foreach (var m in monsters)
                m.Update ();
        }
    }
}