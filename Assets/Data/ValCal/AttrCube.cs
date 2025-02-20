using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.ValCal {
    public enum Enum_AttrID : byte {
        None,
        HP,
        ATK
    }

    public class AttrCube {
        public Enum_AttrID id { private set; get; }
        public int baseValue { private set; get; }
        public int result { private set; get; }
        private List<AttrBlock> attrBlocks = new ();

        public AttrCube (Enum_AttrID id) {
            this.id = id;
        }

        public void SetBaseValue (int val) {
            baseValue = val;
            result = baseValue;
        }

        public AttrBlock AttachBlock (Enum_BlockType type, float srcVal, AttrBlock.ValueAction action) {
            var ab = new AttrBlock (id, type, srcVal, action);
            attrBlocks.Add (ab);
            Rebuild ();

            Debug.Log ($"AttachBlock result: {result}");
            return ab;
        }

        public bool DetachBlock (AttrBlock ab) {
            if (!attrBlocks.Contains (ab))
                return false;

            attrBlocks.Remove (ab);
            Rebuild ();
            Debug.Log ($"DetachBlock result: {result}");
            return true;
        }

        void Rebuild () {
            result = baseValue;
            foreach (var item in attrBlocks) {
                if (item.action == AttrBlock.ValueAction.Replace) {
                    result = (int) item.srcValue;
                    return;
                }
            }

            foreach (var item in attrBlocks) {
                if (item.action == AttrBlock.ValueAction.Addon) {
                    result += (int) item.srcValue;
                }
            }

            foreach (var item in attrBlocks) {
                if (item.action == AttrBlock.ValueAction.Multi) {
                    float r = result * (1 + item.srcValue);
                    result = (int) Mathf.Round (r);
                }
            }
        }
    }
}