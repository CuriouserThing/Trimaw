using System.Reflection;
using MegaCrit.Sts2.Core.Commands.Builders;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core;

public static class AttackCommandExt
{
    extension(AttackCommand cmd)
    {
        public AttackCommand FromFigment(Figment figment)
        {
            var attackerProp = typeof(AttackCommand).GetProperty(nameof(AttackCommand.Attacker),
                BindingFlags.Instance | BindingFlags.Public);
            if (attackerProp is null)
                throw new InvalidOperationException(
                    $"Could not get {nameof(AttackCommand)} property {nameof(AttackCommand.Attacker)}.");

            attackerProp.SetValue(cmd, figment.Creature);

            const string attackerAnimName = "_attackerAnimName";
            var attackerAnimNameField = typeof(AttackCommand).GetField(attackerAnimName,
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (attackerAnimNameField is null)
                throw new InvalidOperationException(
                    $"Could not get {nameof(AttackCommand)} field {attackerAnimName}.");

            // Callers are free to use AttackCommand.WithAttackerAnim afterward, but that method
            // checks to see if a From method has *first* set _attackerAnimName to anything, so we do it here.
            // That said...
            attackerAnimNameField.SetValue(cmd, "Attack");

            // ...regardless, we currently don't even allow AttackCommands to play animations for us.
            cmd.WithNoAttackerAnim();

            // TODO: consider setting cmd._sourceType to some value
            // Osty is special in that all of his damage comes from a CardModel, so we're maybe better off keeping
            // the default SourceType.None. Vanilla currently only checks for SourceType.Monster, so keep an eye out.

            return cmd;
        }
    }
}