using UnityEngine;

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
    public int actionTimeType; //BuffActionType
    public float actionTimeInterval;
    public bool isAdditive;
}

public class Buff {
    public BattleUnit target;
    public BuffConfig config;

    int curActTime;
    public Buff (BuffConfig bc) {
        config = bc;
    }

    public void Init (BattleUnit unit) {
        target = unit;
        StopWork();
    }

    bool isStartWork;
    public void StartWork () {
        curActTime = 0;
        timer = 0;
        
        if (config.actionTimeType == -1) {
            isStartWork = true;
            // 无限循环
            return;
        }
        if (config.actionTimeType == 1) {
            isStartWork = false;
            // 1次
            target?.ModifyProperty (CheckBattleUnitProperty (config.type), -5);
            return;
        }
        if (config.actionTimeType > 1) {
            isStartWork = true;
            // 多次
            return;
        }

        Debug.LogError ($"config.actionType value {config.actionTimeType} is not valid!");
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

    public void StopWork () {
        isStartWork = false;
        curActTime = 0;
        timer = 0;
    }

    float timer;

    public void Update () {
        if (!isStartWork)
            return;

        timer += Time.deltaTime;

        if (config.actionTimeType == -1) {
            if (timer >= config.actionTimeInterval) {
                timer = 0;
                curActTime += 1;
                target?.ModifyProperty (CheckBattleUnitProperty (config.type), -5);
            }
        } else if (config.actionTimeType > 1) {
            if (curActTime >= config.actionTimeType)
                StopWork ();
            else {
                if (timer >= config.actionTimeInterval) {
                    timer = 0;
                    curActTime += 1;
                    target?.ModifyProperty (CheckBattleUnitProperty (config.type), -5);
                }
            }
        }
    }
}