namespace Projectiles
{
    public class ArmorDamageDecorator : ProjectilePropsDecorator
    {
        private readonly float _damageRatio;

        public override int ArmorDamage => (int)(_props.ArmorDamage * _damageRatio);

        public ArmorDamageDecorator(ProjectileProps props, float damageRatio) : base(props)
        {
            _damageRatio = damageRatio;
        }
    }
}