namespace Andastra.Parsing.Common
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/misc.py:250-285
    // Original: class Game(IntEnum):
    // Extended to support all BioWare engine games: Odyssey (KOTOR), Aurora (NWN), Eclipse (DA/ME), Infinity (BG/IWD/PST)
    // This enum is kept in Andastra.Parsing for backward compatibility with patcher tools (HoloPatcher.NET, HolocronToolset, NCSDecomp, KotorDiff)
    /// <summary>
    /// Represents which BioWare engine game / platform variant.
    /// </summary>
    public enum Game
    {
        // Odyssey Engine
        K1 = 1,
        K2 = 2,
        K1_XBOX = 3,
        K2_XBOX = 4,
        K1_IOS = 5,
        K2_IOS = 6,
        K1_ANDROID = 7,
        K2_ANDROID = 8,
        TSL = K2,

        // Eclipse Engine
        DA = 10,
        DA_ORIGINS = DA,
        DA2 = 11,
        DRAGON_AGE_2 = DA2,

        // Aurora Engine
        NWN = 30,
        NEVERWINTER_NIGHTS = NWN,
        NWN2 = 31,
        NEVERWINTER_NIGHTS_2 = NWN2,

        // Infinity Engine
        BG1 = 40,
        BALDURS_GATE = BG1,
        BG2 = 41,
        BALDURS_GATE_2 = BG2,
        IWD = 42,
        ICEWIND_DALE = IWD,
        IWD2 = 43,
        ICEWIND_DALE_2 = IWD2,
        PST = 44,
        PLANESCAPE_TORMENT = PST
    }

    public static class GameExtensions
    {
        public static bool IsK1(this Game game)
        {
            return ((int)game) % 2 != 0 && game >= Game.K1 && game <= Game.K2_ANDROID;
        }

        public static bool IsK2(this Game game)
        {
            return ((int)game) % 2 == 0 && game >= Game.K1 && game <= Game.K2_ANDROID;
        }

        public static bool IsTSL(this Game game)
        {
            return game == Game.K2;
        }

        public static bool IsXbox(this Game game)
        {
            return game == Game.K1_XBOX || game == Game.K2_XBOX;
        }

        public static bool IsPc(this Game game)
        {
            return game == Game.K1 || game == Game.K2;
        }

        public static bool IsMobile(this Game game)
        {
            return game == Game.K1_IOS || game == Game.K2_IOS || game == Game.K1_ANDROID || game == Game.K2_ANDROID;
        }

        public static bool IsAndroid(this Game game)
        {
            return game == Game.K1_ANDROID || game == Game.K2_ANDROID;
        }

        public static bool IsIOS(this Game game)
        {
            return game == Game.K1_IOS || game == Game.K2_IOS;
        }

        // Eclipse Engine
        public static bool IsDragonAge(this Game game)
        {
            return game == Game.DA || game == Game.DA_ORIGINS || game == Game.DA2 || game == Game.DRAGON_AGE_2;
        }

        public static bool IsDragonAgeOrigins(this Game game)
        {
            return game == Game.DA || game == Game.DA_ORIGINS;
        }

        public static bool IsDragonAge2(this Game game)
        {
            return game == Game.DA2 || game == Game.DRAGON_AGE_2;
        }

        // Aurora Engine
        public static bool IsNeverwinterNights(this Game game)
        {
            return game == Game.NWN || game == Game.NEVERWINTER_NIGHTS ||
                   game == Game.NWN2 || game == Game.NEVERWINTER_NIGHTS_2;
        }

        public static bool IsNWN1(this Game game)
        {
            return game == Game.NWN || game == Game.NEVERWINTER_NIGHTS;
        }

        public static bool IsNWN2(this Game game)
        {
            return game == Game.NWN2 || game == Game.NEVERWINTER_NIGHTS_2;
        }

        // Engine family checks
        public static bool IsOdyssey(this Game game)
        {
            return game >= Game.K1 && game <= Game.K2_ANDROID;
        }

        public static bool IsEclipse(this Game game)
        {
            return IsDragonAge(game);
        }

        public static bool IsAurora(this Game game)
        {
            return IsNeverwinterNights(game);
        }

        // Infinity Engine
        public static bool IsBaldursGate(this Game game)
        {
            return game == Game.BG1 || game == Game.BALDURS_GATE ||
                   game == Game.BG2 || game == Game.BALDURS_GATE_2;
        }

        public static bool IsBaldursGate1(this Game game)
        {
            return game == Game.BG1 || game == Game.BALDURS_GATE;
        }

        public static bool IsBaldursGate2(this Game game)
        {
            return game == Game.BG2 || game == Game.BALDURS_GATE_2;
        }

        public static bool IsIcewindDale(this Game game)
        {
            return game == Game.IWD || game == Game.ICEWIND_DALE ||
                   game == Game.IWD2 || game == Game.ICEWIND_DALE_2;
        }

        public static bool IsIcewindDale1(this Game game)
        {
            return game == Game.IWD || game == Game.ICEWIND_DALE;
        }

        public static bool IsIcewindDale2(this Game game)
        {
            return game == Game.IWD2 || game == Game.ICEWIND_DALE_2;
        }

        public static bool IsPlanescapeTorment(this Game game)
        {
            return game == Game.PST || game == Game.PLANESCAPE_TORMENT;
        }

        public static bool IsInfinity(this Game game)
        {
            return game >= Game.BG1 && game <= Game.PLANESCAPE_TORMENT;
        }
    }
}
