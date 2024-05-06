using LowoUN.Util;

public class BuffManager : Manager<BuffManager> {
    public void RemoveBuff_CreatorDie (BattleUnit creator) {
        BattleUnitManager.Instance.hero?.RemoveBuff (creator);
        foreach (var m in BattleUnitManager.Instance.monsters) {
            m.RemoveBuff (creator);
        }
    }
}