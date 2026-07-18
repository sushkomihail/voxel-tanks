using JetBrains.Annotations;
using Tank.Modules;

namespace ArmorSystem
{
    public sealed class Screen : Armor
    {
        [CanBeNull] public TankModule Module { get; private set; }

        public void SetModule(TankModule module)
        {
            Module = module;
        }

        public override void TakeDamage(int damage, object attacker)
        {
            Module?.TakeDamage(damage, attacker);
        }
    }
}