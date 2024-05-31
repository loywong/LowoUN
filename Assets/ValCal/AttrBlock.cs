namespace LowoUN.ValCal {
    public enum Enum_BlockType {
        None,
        Base, //TODO 角色等级变化
        Equip,
        WorldBuff,
        BattleBuff,
    }

    public class AttrBlock {
        // 对目标原值的作用方式.
        // 权重 replace > addon > multiplication
        public enum ValueAction {
            Addon,
            Multi,
            Replace
        }
        public Enum_AttrID attrId = Enum_AttrID.None;
        public Enum_BlockType type = Enum_BlockType.None;

        public float srcValue = 0;
        public ValueAction action = ValueAction.Addon;
        public AttrBlock (Enum_AttrID attrId, Enum_BlockType type, float src, ValueAction action) {
            this.attrId = attrId;
            this.type = type;
            this.srcValue = src;
            this.action = action;
        }
    }
}