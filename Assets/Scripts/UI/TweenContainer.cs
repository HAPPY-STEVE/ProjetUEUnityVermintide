using UnityEngine;
namespace UI.Tween
{
    public abstract class TweenContainer : MonoBehaviour
    {
        public virtual void OnActive()
        {
            // Default implementation does nothing.
        }
        public virtual void OnComplete()
        {
            // Default implementation does nothing.
        }
        public virtual void OnClose()
        {
            // Default implementation does nothing.
        }
    }
}