using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont
{
    public class BoneSync : MonoBehaviour
    {
        [Serializable]
        public struct Pair
        {
            public Transform a;
            public Transform b;
        }

        public bool updateBone = false;
        public SkinnedMeshRenderer[] masterRenderers;
        public SkinnedMeshRenderer[] slaveRenderers;
        [Header("debug data, don`t modify")]
        public List<Pair> pairList;


        void OnEnable()
        {
            if (updateBone)
                UpdateBoneMapping();
        }

        void LateUpdate()
        {
            for (int i = 0, iMax = pairList.Count; i < iMax; i++)
            {
                var pair = pairList[i];

                pair.b.transform.localPosition = pair.a.transform.localPosition;
                pair.b.transform.localRotation = pair.a.transform.localRotation;
                pair.b.transform.localScale = pair.a.transform.localScale;
            }
        }

        [ContextMenu("Manual UpdateBoneMapping")]
        void UpdateBoneMapping()
        {
            var slaveBonesList = new List<Transform>(slaveRenderers.Length * 10);
            for (int i = 0; i < slaveRenderers.Length; i++)
            {
                var renderer = slaveRenderers[i];
                for (int j = 0, jMax = renderer.bones.Length; j < jMax; j++)
                {
                    var bone = renderer.bones[j];
                    slaveBonesList.Add(bone);
                }
            }

            pairList = new List<Pair>(slaveBonesList.Count);
            for (int i = 0; i < masterRenderers.Length; i++)
            {
                var renderer = masterRenderers[i];
                for (int j = 0, jMax = renderer.bones.Length; j < jMax; j++)
                {
                    var bone = renderer.bones[j];

                    var slaveBone = slaveBonesList.Find(m => m.name == bone.name);

                    if (slaveBone != null)
                        pairList.Add(new Pair() { a = bone, b = slaveBone });
                }
            }
        }
    }
}
