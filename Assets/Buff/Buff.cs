using UnityEngine;

namespace LowoUN.Buff {
    public enum BuffType {
        AddAttack,
        AddHP,
        ReduceAttack,
        ReduceHP,
    }

    // public enum BuffActionType {
    //     None,
    //     Once,
    //     Multi,
    //     Loop,
    // }
    // public enum BuffOverlapType {
    //     Additive,
    //     Replace
    // }

    public struct BuffConfig {
        public BuffType type;
        public int actionTimes;
        public int actionValue;
        public float actionTimeInterval;
        public bool isAdditive;
    }

    public class Buff {
        public BattleUnit creator { private set; get; }
        public BattleUnit target { private set; get; }
        public BuffConfig config { private set; get; }

        int curActTime;
        public Buff (BattleUnit creator, BuffConfig bc) {
            this.creator = creator;
            config = bc;
        }

        public void Init (BattleUnit target) {
            this.target = target;
            isStartWork = false;
            curActTime = 0;
            timer = 0;
        }

        bool isStartWork;
        public void Start () {
            curActTime = 0;
            timer = 0;
            isStartWork = true;

            if (config.actionTimes == 0 || config.actionTimes < -1)
                Debug.LogError ($"config.actionType value {config.actionTimes} is not valid!");
        }

        // BuffType --- BattleUnitProperty
        BattleUnitProperty CheckBattleUnitProperty (BuffType bt) {
            Debug.Log ($"CheckBattleUnitProperty with BuffType is {bt}");

            switch (bt) {
                case BuffType.AddAttack:
                case BuffType.ReduceAttack:
                    return BattleUnitProperty.ATK;
                case BuffType.AddHP:
                case BuffType.ReduceHP:
                    return BattleUnitProperty.HP;
                default:
                    return BattleUnitProperty.None;
            }
        }

        public void End () {
            isStartWork = false;
            curActTime = 0;
            timer = 0;

            target?.RemoveBuff (this);
        }

        float timer;

        public void Update () {
            if (!isStartWork)
                return;

            timer += Time.deltaTime;

            if (config.actionTimes == -1) {
                if (timer >= config.actionTimeInterval) {
                    timer = 0;
                    curActTime += 1;
                    target?.ModifyProperty (CheckBattleUnitProperty (config.type), config.actionValue);
                }
            } else if (config.actionTimes >= 1) {
                if (curActTime >= config.actionTimes)
                    End ();
                else {
                    if (timer >= config.actionTimeInterval) {
                        timer = 0;
                        curActTime += 1;
                        target?.ModifyProperty (CheckBattleUnitProperty (config.type), config.actionValue);
                    }
                }
            }
        }
    }
}