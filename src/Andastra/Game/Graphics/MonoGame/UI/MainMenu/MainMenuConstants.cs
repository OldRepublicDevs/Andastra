// ---------------------------------------------------------------------------
// MainMenuConstants.cs
// Exhaustive constants for KOTOR I/II main menu. References:
// - vendor/KotOR.js/src/game/kotor/menu/MainMenu.ts
// - vendor/KotOR.js/src/game/tsl/menu/MainMenu.ts
// - vendor/reone/include/reone/game/gui/mainmenu.h
// - vendor/reone/src/libs/game/gui/mainmenu.cpp
// - docs/MONOGAME_REVA_UI_MAPPING.md (Reva addresses)
// ---------------------------------------------------------------------------

using Microsoft.Xna.Framework;

namespace Andastra.Game.Graphics.MonoGame.UI.MainMenu
{
    /// <summary>
    /// Control tags for main menu. Must match GFF and all three implementations.
    /// reone mainmenu.h struct Controls (lines 43-60).
    /// KotOR.js MainMenu.ts declared fields (lines 22-36 K1, 21-34 TSL).
    /// </summary>
    public static class MainMenuControlTags
    {
        // Buttons - reone: BTN_* ; KotOR.js: this.BTN_*
        public const string BTN_EXIT = "BTN_EXIT";
        public const string BTN_LOADGAME = "BTN_LOADGAME";
        public const string BTN_MOREGAMES = "BTN_MOREGAMES";
        public const string BTN_MOVIES = "BTN_MOVIES";
        public const string BTN_MUSIC = "BTN_MUSIC";           // TSL only
        public const string BTN_NEWGAME = "BTN_NEWGAME";
        public const string BTN_OPTIONS = "BTN_OPTIONS";
        public const string BTN_TSLRCM = "BTN_TSLRCM";         // TSL RCM
        public const string BTN_WARP = "BTN_WARP";             // Developer / warp to module

        // Labels - reone: LBL_* ; KotOR.js: this.LBL_*
        public const string LBL_3DVIEW = "LBL_3DVIEW";         // K1: 3D mainmenu model view; TSL: mainmenu01
        public const string LBL_BW = "LBL_BW";                 // BioWare logo
        public const string LBL_GAMELOGO = "LBL_GAMELOGO";      // Game logo
        public const string LBL_LUCAS = "LBL_LUCAS";            // LucasArts logo
        public const string LBL_MENUBG = "LBL_MENUBG";         // Menu background (K1)
        public const string LBL_NEWCONTENT = "LBL_NEWCONTENT";  // New content label

        // Listbox - reone: LB_MODULES ; KotOR.js: this.LB_MODULES
        public const string LB_MODULES = "LB_MODULES";         // Module list for warp
    }

    /// <summary>
    /// GUI and asset resrefs per game. KotOR.js MainMenu constructor (K1 lines 41-46, TSL 38-43).
    /// reone mainmenu.cpp constructor (lines 46-55).
    /// </summary>
    public static class MainMenuResRefs
    {
        /// <summary>K1: mainmenu16x12 (KotOR.js line 42, reone line 52).</summary>
        public const string K1_GuiResRef = "mainmenu16x12";

        /// <summary>TSL: mainmenu8x6_p (KotOR.js line 39, reone line 49).</summary>
        public const string K2_GuiResRef = "mainmenu8x6_p";

        /// <summary>K1 background texture (KotOR.js line 43).</summary>
        public const string K1_Background = "1600x1200back";

        /// <summary>TSL has no background texture (KotOR.js line 40: this.background = '').</summary>
        public const string K2_Background = "";

        /// <summary>K1 main menu music (reone line 53, KotOR.js line 38).</summary>
        public const string K1_MusicResRef = "mus_theme_cult";

        /// <summary>TSL main menu music (reone line 50, KotOR.js TSL line 35).</summary>
        public const string K2_MusicResRef = "mus_sion";

        /// <summary>K1 3D model for LBL_3DVIEW (KotOR.js line 90: 'mainmenu').</summary>
        public const string K1_ModelResRef = "mainmenu";

        /// <summary>TSL 3D model for LBL_3DVIEW (KotOR.js TSL line 93: 'mainmenu01').</summary>
        public const string K2_ModelResRef = "mainmenu01";
    }

    /// <summary>
    /// Default colors. KotOR.js GUIControl.ts (lines 186-192): defaultColor, defaultHighlightColor (K1 vs TSL).
    /// reone mainmenu.cpp setButtonColors (lines 119-122), GameGUI base color members.
    /// </summary>
    public static class MainMenuColors
    {
        // K1 - KotOR.js GUIControl.ts line 186-187
        public static readonly Color K1_BaseColor = new Color(0.0f, 0.658824f, 0.980392f, 1f);       // Cyan-blue
        public static readonly Color K1_HilightColor = new Color(1f, 1f, 0f, 1f);                     // Yellow

        // TSL - KotOR.js GUIControl.ts lines 190-191
        public static readonly Color K2_BaseColor = new Color(0.10196078568697f, 0.69803923368454f, 0.549019634723663f, 1f);  // Teal
        public static readonly Color K2_HilightColor = new Color(0.8f, 0.8f, 0.6980392336845398f, 1f);                        // Light yellow

        // Void fill - KotOR.js GameMenu.ts loadBackground (line 166): u_color.value.setRGB
        public static readonly Color VoidFillK1 = new Color(0.10196078568697f, 0.69803923368454f, 0.549019634723663f, 1f);   // Teal
        public static readonly Color VoidFillK2 = new Color(0.10196078568697f, 0.69803923368454f, 0.549019634723663f, 1f);   // Same

        // Fallback background (no texture) - dark blue
        public static readonly Color FallbackBackground = new Color(20, 30, 60, 255);
    }

    /// <summary>
    /// Visibility and behavior per control. reone onGUILoaded (lines 69-86).
    /// KotOR.js menuControlInitializer (K1 55-58, TSL 49-51).
    /// </summary>
    public static class MainMenuVisibility
    {
        /// <summary>Hidden by default in all: LB_MODULES.</summary>
        public const string HiddenAlways = "LB_MODULES,LBL_BW,LBL_LUCAS,LBL_NEWCONTENT";

        /// <summary>K1: BTN_WARP hidden unless developer. reone line 84-86.</summary>
        public const string WarpButton = "BTN_WARP";

        /// <summary>reone line 75-76: BTN_MOVIES, BTN_OPTIONS disabled in reone (we show enabled).</summary>
        public const string OptionalButtons = "BTN_MOVIES,BTN_OPTIONS";

        /// <summary>reone lines 76-81: BTN_MOREGAMES, BTN_TSLRCM hidden if present.</summary>
        public const string OptionalHidden = "BTN_MOREGAMES,BTN_TSLRCM";
    }

    /// <summary>
    /// Button order for keyboard/gamepad. KotOR.js triggerControllerDDownPress (lines 183-204):
    /// NEWGAME -> LOADGAME -> MOVIES -> OPTIONS -> EXIT (wrap to NEWGAME).
    /// </summary>
    public static class MainMenuButtonOrder
    {
        /// <summary>Order for D-pad down / keyboard down. Index 0 = default selected.</summary>
        public static readonly string[] DownOrder = new[]
        {
            MainMenuControlTags.BTN_NEWGAME,
            MainMenuControlTags.BTN_LOADGAME,
            MainMenuControlTags.BTN_MOVIES,
            MainMenuControlTags.BTN_OPTIONS,
            MainMenuControlTags.BTN_EXIT
        };

        /// <summary>D-pad up reverses the order (EXIT -> OPTIONS -> ... -> NEWGAME).</summary>
        public static readonly string[] UpOrder = new[]
        {
            MainMenuControlTags.BTN_EXIT,
            MainMenuControlTags.BTN_OPTIONS,
            MainMenuControlTags.BTN_MOVIES,
            MainMenuControlTags.BTN_LOADGAME,
            MainMenuControlTags.BTN_NEWGAME
        };
    }

    /// <summary>
    /// Layout for fallback (no GFF) main menu. Matches typical 800x600 / scaled.
    /// reone preload sets resolution 800x600 (mainmenu.cpp line 59).
    /// </summary>
    public static class MainMenuLayout
    {
        public const int ReferenceWidth = 800;
        public const int ReferenceHeight = 600;
        public const int ButtonWidth = 280;
        public const int ButtonHeight = 44;
        public const int ButtonSpacing = 56;
        /// <summary>Start Y for first button (NEW GAME).</summary>
        public const int ButtonStartY = 200;
        /// <summary>reone mainmenu.cpp kKotorModelSize.</summary>
        public const float KotorModelSize = 1.4f;
    }
}
