using ShootingSystems;

namespace UI.Aims
{
    public class CyclicAim : Aim
    {
        protected override void UpdateReloadIndicator()
        {
            if (_gun.ShootingSystem is CyclicSystem cyclicSystem)
            {
                _reloadIndicator.fillAmount = cyclicSystem.GetReloadingRate();
            }
        }
    }
}
