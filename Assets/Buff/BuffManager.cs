using LowoUN.Util;

namespace LowoUN.Buff {
    public class BuffManager : Manager<BuffManager> {
        public void RemoveBuff_CreatorDie (BattleUnit creator) {
            BattleUnitManager.Instance.hero?.RemoveBuff (creator);
            foreach (var m in BattleUnitManager.Instance.monsters) {
                m.RemoveBuff (creator);
            }
        }
    }
}