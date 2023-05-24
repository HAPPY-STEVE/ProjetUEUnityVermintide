using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Tween
{
    public class SlidingTween : TweenContainer
    {
        [Header("Ease de l'animation ")]
        public Ease inType;
        public Ease outType;
        public float duration;
        [Header("Parametres anim")]
        public float delay;
        private float delayBeginning, delayEnd;
        public bool noDelayOnEnd = false, noDelayOnBeginning = false;
        [Header("Events")]
        public UnityEvent onCompleteCallback;
        public enum Direction { Left, Right };
        [Header("Direction du slide")]
        public Direction typeOfSlide;
        RectTransform rt, canvasRT;

        Vector3 offScreenLeftPosition;
        Vector3 offScreenRightPosition;
        Vector3 offScreenDirection;
        Vector3 centerPosition;

        void Awake()
        {
            rt = gameObject.GetComponent<RectTransform>();

            centerPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);
            offScreenLeftPosition = new Vector3(-Screen.width, rt.localPosition.y, rt.localPosition.z);
            offScreenRightPosition = new Vector3(Screen.width, rt.localPosition.y, rt.localPosition.z);
            rt.localPosition = resolveDirection(offScreenDirection);
            resolveDelay();
        }

        private Vector3 resolveDirection(Vector3 offScreenPos)
        {
            switch (typeOfSlide)
            {
                case Direction.Left:
                    offScreenPos = offScreenLeftPosition;
                    break;
                case Direction.Right:
                    offScreenPos = offScreenRightPosition;
                    break;
                default:
                    offScreenPos = offScreenLeftPosition;
                    break;
            }
            return offScreenPos;
        }

        override public void OnActive()
        {
            rt.DOLocalMove(centerPosition, duration).SetEase(inType).SetDelay(delayBeginning);
        }

        override public void OnClose()
        {
            rt.DOLocalMove(resolveDirection(offScreenDirection), duration).SetEase(outType).SetDelay(delayEnd).OnComplete(OnComplete);
        }

        override public void OnComplete()
        {
            if (onCompleteCallback != null)
            {
                onCompleteCallback.Invoke();
            }
        }

        private void resolveDelay()
        {
            delayBeginning = delay;
            delayEnd = delay;

            if (noDelayOnBeginning == true)
            {
                delayBeginning = 0;
            }

            if (noDelayOnEnd == true)
            {
                delayEnd = 0;
            }
        }
    }
}