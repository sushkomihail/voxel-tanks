namespace Projectiles
{
    public class ProjectilePropsDecorator : ProjectileProps
    {
        protected ProjectileProps _props;
        
        public ProjectilePropsDecorator(ProjectileProps props)
        {
            _props = props;
        }
    }
}