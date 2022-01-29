using R2API;
using System;

namespace HenryMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Henry
            string prefix = HitPlugin.developerPrefix + "_HIT_BODY_";

            string desc = "Hit, renowned as 'Never - Miss Hit', 'Hit the Infallible' and as the 'Legendary Hitman' is the legendary assassin of Universe 6.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > DirectHits are powerfull and fast, also have a chance to stun enemies" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Time Release is a good way to aproach enemies sinse this skill stun the enemy." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Tides Of Time can be used for both, scape or extra damage" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Time Skip can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new job.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Hit");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Bonty Hunter");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_KS_NAME", "Ki Sensee");
            LanguageAPI.Add(prefix + "PASSIVE_KS_DESCRIPTION", "Hit uses his Ki to pressure and find enemies.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_DH_NAME", "Direct Hit");
            LanguageAPI.Add(prefix + "PRIMARY_DH_DESCRIPTION", "Fast and powerfull punches that deals <style=cIsDamage> 110% ~ 150% damage </style> and has a chance to <style=cIsUtility>stun</style> enemies.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_TR_NAME", "Time Release");
            LanguageAPI.Add(prefix + "SECONDARY_TR_DESCRIPTION", "An Ranged attack, skip time to hit an enemy in range dealing <style=cIsDamage> 3x 40% damage </style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_TOT_NAME", "Tides Of Time");
            LanguageAPI.Add(prefix + "UTILITY_TOT_DESCRIPTION", "This is the final stance that Hit has, <style=cIsUtility>Hit becomes invulnerable and boost attack speed</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TS_NAME", "Time Skip");
            LanguageAPI.Add(prefix + "SPECIAL_TS_DESCRIPTION", "The continuous improvement of Hit makes him able to <style=cIsUtility>Skip time extremely fast, so every enemy on rage will suffer</style>.");
            #endregion

            #region Achievements
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}