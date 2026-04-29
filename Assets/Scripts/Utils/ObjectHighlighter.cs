using QuickOutline.Scripts;
using UnityEngine;

namespace Utils
{
    public class ObjectHighlighter
    {
        private Transform _lastFocusTransform;
        private Outline _lastEnabledOutline;
        
        public void TryHighlightFocusObject(RaycastHit cameraHit)
        {
            if (!cameraHit.transform || cameraHit.transform == _lastFocusTransform) return;
            
            _lastFocusTransform = cameraHit.transform;
            
            if (cameraHit.transform.TryGetComponent(out Outline outline))
            {
                DisableLastEnabledOutline();
                
                if (outline is SwitchableOutline { IsInteractive: false }) return;

                _lastEnabledOutline = outline;
                _lastEnabledOutline.enabled = true;
            }
            else
            {
                DisableLastEnabledOutline();
            }
        }

        private void DisableLastEnabledOutline()
        {
            if (!_lastEnabledOutline) return;
            
            _lastEnabledOutline.enabled = false;
        }
    }
}