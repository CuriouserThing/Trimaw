using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.HarmonyPatches;

// Courtesy of https://github.com/HalfFocused/EnigmasOfTheSpire/blob/master/SpireEnigmasCode/Patches/ChirpPatches.cs#L111
// (Discord user @halfocused)
[HarmonyPatch(MethodType.Async)]
[HarmonyPatch(typeof(PersonalHivePower), nameof(PersonalHivePower.AfterDamageReceived))]
public class PersonalHivePowerAfterDamagedReceivedPatch
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var codeMatcher = new CodeMatcher(instructions, generator);

        codeMatcher.MatchStartForward(
            CodeMatch.IsLdarg(0), //0 - this
            new CodeMatch(OpCodes.Ldfld), //1 - loads the 'dealer' variable
            new CodeMatch(OpCodes.Callvirt), //2 - calls getMonster()
            new CodeMatch(OpCodes.Isinst), //3 - checks if it's an Osty
            new CodeMatch(OpCodes.Brfalse_S), //4 - skips past 5-10 if not Osty
            CodeMatch.IsLdarg(0), //5 - this
            CodeMatch.IsLdarg(0), //6 - this
            new CodeMatch(OpCodes.Ldfld), //7 - loads 'dealer' again
            new CodeMatch(OpCodes.Callvirt), //8 - getPetOwner()
            new CodeMatch(OpCodes.Callvirt), //9 - getCreature()
            new CodeMatch(OpCodes.Stfld) //10 - sets 'dealer' to the result
        ).ThrowIfInvalid("Could not find Osty check & reassignment");

        var dealerField = codeMatcher.InstructionAt(1).operand;
        var getMonsterMethod = codeMatcher.InstructionAt(2).operand;

        var getPetOwnerMethod = codeMatcher.InstructionAt(8).operand;
        var getCreatureMethod = codeMatcher.InstructionAt(9).operand;

        /*
         * they call this move the pointer shuffle
         */

        //we advance late for the sake of setting those variables
        codeMatcher.Advance(11);

        //save this position.
        codeMatcher.CreateLabel(out var startOfChirpCheck);
        //go back to the BrFalse instruction
        codeMatcher.Advance(-7);
        //instead of the Osty check skipping to end, we want it to skip to here
        codeMatcher.SetOperandAndAdvance(startOfChirpCheck);
        //go back to where the chirp check is being created
        codeMatcher.Advance(7);

        codeMatcher.InsertAndAdvance(
            /*
             * All this is a functional copy of the Osty check, except with Chirp instead.
             * This is why we needed to grab those 4 variables to ensure resistance against field or method renames
             */
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldfld, dealerField),
            new CodeInstruction(OpCodes.Callvirt, getMonsterMethod),
            new CodeInstruction(OpCodes.Isinst, typeof(Figment)),
            new CodeInstruction(OpCodes
                .Brfalse_S), //created without an operand, this gets set later once we have a label
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldfld, dealerField),
            new CodeInstruction(OpCodes.Callvirt, getPetOwnerMethod),
            new CodeInstruction(OpCodes.Callvirt, getCreatureMethod),
            new CodeInstruction(OpCodes.Stfld, dealerField)
        );
        //grab this position
        codeMatcher.CreateLabel(out var endOfChirpCheck);
        //go back to the BrFalse we created without an Operand and tell it to jump here.
        codeMatcher.Advance(-7);
        codeMatcher.SetOperandAndAdvance(endOfChirpCheck);

        return codeMatcher.Instructions();
    }
}