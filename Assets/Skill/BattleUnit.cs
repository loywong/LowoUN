using System;

// public enum BattleUnitState {
//     None,
//     Birth,
//     Idle,
//     Attack,
//     Hurt,
//     Dead
// }

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
    Hero1,
    Monster1
}
public class BattleUnitConfig {
    public BattleUnitType unitType;
    public string name;
    public UnitCamp camp;
}
public class BattleUnitProfile {
    BattleUnitConfig config;
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
    // public BattleUnitState state;
    // 各种组件
    BuffSys buffSys;

    //信息同步
    public Action onPropertyChange_Hp;

    public void ClearAll () { }

    public BattleUnit (BattleUnitConfig buc, int lv) {
        profile = new BattleUnitProfile (buc);
        profile.lv = lv;
        profile.maxHP = 1000;
        profile.curHP = 1000; // must be less than hp

        // state = BattleUnitState.None;
    }

    // public void SetState_Birth () {
    //     SetState (BattleUnitState.Birth);
    // }
    // private void SetState (BattleUnitState s) {
    //     state = s;
    // }

    public void AddBuff (Buff b) {
        buffSys = buffSys?? new BuffSys (this);
        buffSys.AddBuff (b);
    }
    public void RemoveBuff (Buff b) {
        if (buffSys == null)
            return;

        buffSys.RemoveBuff (b);
    }

    // 修改属性
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