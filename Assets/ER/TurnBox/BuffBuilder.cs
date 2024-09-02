using UnityEngine;

namespace ER.TurnBox
{
    public class BuffBuilder
    {
        /// <summary>
        /// 需要重写
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static Buff Build(RBuff configure)
        {
            return null;
        }
    }

#if false
switch (configure.RegistryName)
{
    case Agile.registryName:
        return new Agile(configure);

    case Anger.registryName:
        return new Anger(configure);

    case Bind.registryName:
        return new Bind(configure);

    case Charge.registryName:
        return new Charge(configure);

    case Confusion.registryName:
        return new Confusion(configure);

    case Dodge.registryName:
        return new Dodge(configure);

    case Exorcism.registryName:
        return new Exorcism(configure);

    case Flying.registryName:
        return new Flying(configure);

    case Fragile.registryName:
        return new Fragile(configure);

    case Fury.registryName:
        return new Fury(configure);

    case Ghost.registryName:
        return new Ghost(configure);

    case Immunity.registryName:
        return new Immunity(configure);

    case Strength.registryName:
        return new Strength(configure);

    case Vulnerability.registryName:
        return new Vulnerability(configure);

    case Weakness.registryName:
        return new Weakness(configure);

    case Elapse.registryName:
        return new Elapse(configure);

    case Lithe.registryName:
        return new Lithe(configure);

    case MoneyMad.registryName:
        return new MoneyMad(configure);

    case Powder.registryName:
        return new Powder(configure);

    case Snail.registryName:
        return new Snail(configure);

    case Thron.registryName:
        return new Thron(configure);

    case Maintenance.registryName:
        return new Maintenance(configure);

    case Empty.registryName:
        return new Empty(configure);

    case ForceEnemy.registryName:
        return new ForceEnemy(configure);

    case ShootingStar.registryName:
        return new ShootingStar(configure);

    case Defence.registryName:
        return new Defence(configure);

    case Crush.registryName:
        return new Crush(configure);

    case Gem.registryName:
        return new Gem(configure);

    case SafeNet.registryName:
        return new SafeNet(configure);

    case Killself.registryName:
        return new Killself(configure);

    case Battery.registryName:
        return new Battery(configure);

    case Map.registryName:
        return new Map(configure);

    case Wheel.registryName:
        return new Wheel(configure);

    case Emperor.registryName:
        return new Emperor(configure);

    case Split.registryName:
        return new Split(configure);
}
Debug.LogWarning($"构筑Buff失败: {configure.RegistryName}");
#endif
}