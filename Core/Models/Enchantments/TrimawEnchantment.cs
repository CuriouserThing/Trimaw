using BaseLib.Abstracts;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public abstract class TrimawEnchantment : CustomEnchantmentModel
{
    protected sealed override string CustomIconPath => Icon64Path;

    public virtual string Icon64Path => Pathfinder.Enchantment64(this);
}