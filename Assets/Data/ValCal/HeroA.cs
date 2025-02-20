using UnityEngine;

namespace LowoUN.ValCal {
    public class EquipItemProfile_Ring {
        public float hpParam = 0.3f; //extra
        public AttrBlock.ValueAction hpAction = AttrBlock.ValueAction.Multi;
    }

    public class EquipItemProfile_Shoes {
        public int hpParam = 33;
        public AttrBlock.ValueAction hpAction = AttrBlock.ValueAction.Addon;
    }

    public class HeroProfile {
        public int curLevelHP; //根据lv来读取配置
        public AttrCube curHPCub;
        public int curHP => curHPCub.result;

        public HeroProfile () {
            curLevelHP = 100;
            curHPCub = new AttrCube (Enum_AttrID.HP);
        }
    }

    public class HeroA {
        HeroProfile profile = new HeroProfile ();
        // TEMP
        AttrBlock hp_equipItem_Shoes;
        AttrBlock hp_equipItem_Ring;

        public HeroA () {
            profile.curHPCub.SetBaseValue (profile.curLevelHP);
            Debug.Log ($"HeroA's HP:{profile.curHP}");
        }

        public void EquipItem_Shoes () {
            if (hp_equipItem_Shoes != null) {
                Debug.LogWarning ("Shoes has equipped!");
                return;
            }

            var equipProfile = new EquipItemProfile_Shoes ();
            hp_equipItem_Shoes = profile.curHPCub.AttachBlock (Enum_BlockType.Equip, equipProfile.hpParam, equipProfile.hpAction);
        }

        public void EquipItem_Ring () {
            if (hp_equipItem_Ring != null) {
                Debug.LogWarning ("Ring has equipped!");
                return;
            }

            var equipProfile = new EquipItemProfile_Ring ();
            hp_equipItem_Ring = profile.curHPCub.AttachBlock (Enum_BlockType.Equip, equipProfile.hpParam, equipProfile.hpAction);
        }

        public void UnequipItem_Shoes () {
            profile.curHPCub.DetachBlock (hp_equipItem_Shoes);
            hp_equipItem_Shoes = null;
        }

        public void UnequipItem_Ring () {
            profile.curHPCub.DetachBlock (hp_equipItem_Ring);
            hp_equipItem_Ring = null;
        }
    }
}