using System;

namespace LowoUN.Buff {
    public enum BattleUnitState {
        None,
        Birth,
        Idle,
        Attack,
        Hurt,
        Dead
    }

    public enum UnitCamp {
        Neutral,
        Hero,
        Monster,
    }

    public enum UnitType {
        Hero1,
        Monster1,
        Monster2,
        Monster3,
    }

    public enum BattleUnitProperty {
        None,
        HP,
        ATK,
    }

    public enum BattleUnitType {
        None,
        Scene, //Global, may do something like casting buffs, disabled when leave the scene
        Hero1,
        Monster1
    }

    public class BattleUnitConfig {
        public BattleUnitType unitType;
        public string name;
        public UnitCamp camp;
        public int BaseHP;
    }

    public class BattleUnitProfile {
        public BattleUnitConfig config { get; private set; }
        public int id;
        public UnitType unitType;
        // public Camp camp;
        public int lv;

        public int maxHP; //可以根据lv来读取配置
        public int curHP; //atk heal 等技能，或者buff修改的该值

        public BattleUnitProfile (BattleUnitConfig buc) {
            config = buc;
        }
    }

    public class BattleUnit {
        public BattleUnitProfile profile;
        public BattleUnitConfig config => profile.config;
        public BattleUnitState state { private set; get; }
        // 各种组件
        BuffSys buffSys;

        //信息同步
        public Action onPropertyChange_Hp;

        public BattleUnit (BattleUnitConfig buc, int lv) {
            profile = new BattleUnitProfile (buc);
            profile.lv = lv;
            profile.maxHP = 1000;
            profile.curHP = buc.BaseHP * lv; // must be less than hp

            state = BattleUnitState.None;
        }

        public void SetState (BattleUnitState s) {
            state = s;
        }

        public void AddBuff (Buff b) {
            buffSys = buffSys?? new BuffSys (this);
            buffSys.AddBuff (b);
        }

        public void RemoveBuff (BattleUnit bu) {
            buffSys?.RemoveBuff (bu);
        }

        public void RemoveBuff (Buff b) {
            buffSys?.RemoveBuff (b);
        }

        public void ModifyProperty (BattleUnitProperty propType, float value) {
            switch (propType) {
                case BattleUnitProperty.HP:
                    profile.curHP += (int) value;
                    onPropertyChange_Hp?.Invoke ();
                    break;
                case BattleUnitProperty.ATK:
                    break;
                default:
                    break;
            }
        }

        public void Update () {
            if (buffSys != null) buffSys.Update ();
        }
    }
}