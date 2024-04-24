using System.Collections.Generic;
using LowoUN.Util;
using UnityEngine;

class BattleUnitManager : Manager<BattleUnitManager> {
    public BattleUnit Hero;
    public List<BattleUnit> allMonsters = new ();
    
    void CallBack_HP_1 () {
        Debug.Log ($"CallBack_HP_1 curHP {Hero.profile.curHP}");
    }
    void CallBack_HP_2 () {
        Debug.Log ($"CallBack_HP_2 curHP {Hero.profile.curHP}");
    }

    public void CreateHero () {
        BattleUnitConfig buc = ConfigManager.Instance.GetBattleUnitConfig (BattleUnitType.Hero1);
        Hero = new BattleUnit (buc, 1);
        Hero.onPropertyChange_Hp += CallBack_HP_1;
        Hero.onPropertyChange_Hp += CallBack_HP_2;
        Debug.Log ($"CreateHero, Hero curHP is {Hero.profile.curHP}");
    }

    public void DestroyHero () {
        Debug.Log ("DestroyHero, Hero1");
        // Hero.onPropertyChange_Hp -= CallBack_HP_1;
        // Hero.onPropertyChange_Hp -= CallBack_HP_2;
        Hero = null;
    }

    public void Update () {
        Hero?.Update ();

        foreach (var m in allMonsters)
            m.Update ();
    }
}