using UnityEngine;

namespace OutlineSystem
{
    public class ObjectHighlighter
    {
        private Transform _lastFocusTransform;
        private IOutlineTrigger _lastTrigger;
        
        public void TryHighlightFocusObject(RaycastHit cameraHit)
        {
            if (!cameraHit.transform || cameraHit.transform == _lastFocusTransform) return;
            
            _lastFocusTransform = cameraHit.transform;
            
            if (cameraHit.transform.TryGetComponent(out IOutlineTrigger trigger))
            {
                _lastTrigger?.SetOutlineEnabled(false);
                _lastTrigger = trigger;
                _lastTrigger.SetOutlineEnabled(true);
            }
            else
            {
                _lastTrigger?.SetOutlineEnabled(false);
            }
        }
    }
}