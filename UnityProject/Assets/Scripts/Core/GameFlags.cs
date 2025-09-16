using System.Collections.Generic;

namespace Game.Core
{
    public static class Flags
    {
        // Core progression flags derived from original Ren'Py script
        public const string HasToy = "has_toy";
        public const string HasKey = "has_key";
        public const string HasPhotoAlbum = "has_photo_album";
        public const string HasDiary = "has_diary";

        public const string CafeFalseId = "cafe_false_id";
        public const string CafeMotherHint = "cafe_mother_hint";

        public const string ChapelShadowWhisper = "chapel_shadow_whisper";
        public const string ChapelVictimsKnowledge = "chapel_victims_knowledge";

        public const string EastMarketOccult = "east_market_occult";

        public const string FlagMemoryPhoto = "flag_memory_photo";

        public const string FoundArticle = "found_article";
        public const string FoundFalseId = "found_false_id";

        public const string MorgueBodyDouble = "morgue_body_double";
        public const string MorgueFatherExperiments = "morgue_father_experiments";

        public const string WestAsylumParanoia = "west_asylum_paranoia";
        public const string WestFoundBlueprint = "west_found_blueprint";
        public const string WestMemoryDoor = "west_memory_door";
        public const string WestSawPatientShadow = "west_saw_patient_shadow";

        public const string FoundTruthTunnel = "found_truth_tunnel"; // late unlock

        public const string EndingTruth = "ending_truth";
        public const string EndingEscape = "ending_escape";
        public const string EndingMadness = "ending_madness";
        public const string EndingDeath = "ending_death";

        public static readonly IReadOnlyList<string> All = new List<string>
        {
            HasToy,
            HasKey,
            HasPhotoAlbum,
            HasDiary,
            CafeFalseId,
            CafeMotherHint,
            ChapelShadowWhisper,
            ChapelVictimsKnowledge,
            EastMarketOccult,
            FlagMemoryPhoto,
            FoundArticle,
            FoundFalseId,
            MorgueBodyDouble,
            MorgueFatherExperiments,
            WestAsylumParanoia,
            WestFoundBlueprint,
            WestMemoryDoor,
            WestSawPatientShadow,
            FoundTruthTunnel,
            EndingTruth,
            EndingEscape,
            EndingMadness,
            EndingDeath,
        };
    }
}
