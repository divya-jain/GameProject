using UnityEngine;

namespace Game.Utils
{
    /// <summary>
    /// Detect BoxCollider Interaaction on click. can be ectended for Trigger functions
    /// </summary>
    public class ClickableObject : MonoBehaviour
    {
        public static System.Action<string> ObjectClicked;

        private void OnMouseDown()
        {
            ObjectClicked?.Invoke(tag);
        }
    }
}