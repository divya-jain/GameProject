using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animating a group of letters with a timegap
/// </summary>
/// 
namespace Game.Utils
{
    public class SequentialAnimator : MonoBehaviour
    {
        public float waitBetween;
        public float waitEnd;

        List<Animator> animators;

        void Start()
        {
            UpdateAnimatorList();

            StartCoroutine(Play());
        }

        public IEnumerator Play()
        {
            while (true)
            {
                IEnumerable<Animator> tempAnimators = animators;

                foreach (Animator animator in tempAnimators)
                {
                    animator.SetTrigger("DoAnimation");
                    yield return new WaitForSeconds(waitBetween);
                }

                yield return new WaitForSeconds(waitEnd);
            }
        }

        public void UpdateAnimatorList()
        {
            animators = GetComponentsInChildren<Animator>().ToList();
        }
    }
}