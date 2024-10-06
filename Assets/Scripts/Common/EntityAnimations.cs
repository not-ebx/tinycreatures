using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class EntityAnimations
    {
        [SerializeField] public AnimationClip idle;
        [SerializeField] public AnimationClip die;
    }
}