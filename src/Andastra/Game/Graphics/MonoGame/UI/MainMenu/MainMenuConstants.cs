// ---------------------------------------------------------------------------
// MainMenuConstants.cs
// Exhaustive constants for KOTOR I/II main menu. Reva (k1_win_gog_swkotor.exe):
// - CSWGuiMainMenu::LoadFromLayout @ 0x0067ace0: control tags LB_MODULES, BTN_EXIT, BTN_WARP, LBL_3DVIEW,
//   BTN_NEWGAME, BTN_LOADGAME, BTN_MOVIES, BTN_OPTIONS, LBL_NEWCONTENT, LBL_GAMELOGO, LBL_MENUBG
// - CSWGuiMainMenu::CSWGuiMainMenu @ 0x0067c4c0: ResRef "mainmenu" for layout; 3D model "mainmenu"
// ---------------------------------------------------------------------------

using Microsoft.Xna.Framework;

namespace Andastra.Game.Graphics.MonoGame.UI.MainMenu
{
    /// <summary>
    /// Control tags for main menu. Must match GFF and Reva LoadFromLayout bindings.
    /// Reva (k1_win_gog_swkotor.exe): CSWGuiMainMenu::LoadFromLayout @ 0x0067ace0 InitControl(tag) for each control.
    /// </summary>
    public static class MainMenuControlTags
    {
        // Buttons - Reva LoadFromLayout: BTN_*
        public const string BTN_EXIT = "BTN_EXIT";
        public const string BTN_LOADGAME = "BTN_LOADGAME";
        public const string BTN_MOREGAMES = "BTN_MOREGAMES";
        public const string BTN_MOVIES = "BTN_MOVIES";
        public const string BTN_MUSIC = "BTN_MUSIC";           // TSL only
        public const string BTN_NEWGAME = "BTN_NEWGAME";
        public const string BTN_OPTIONS = "BTN_OPTIONS";
        public const string BTN_TSLRCM = "BTN_TSLRCM";         // TSL RCM
        public const string BTN_WARP = "BTN_WARP";             // Developer / warp to module

        // Labels - Reva LoadFromLayout: LBL_*
        public const string LBL_3DVIEW = "LBL_3DVIEW";         // K1: 3D mainmenu model view; TSL: mainmenu01
        public const string LBL_BW = "LBL_BW";                 // BioWare logo
        public const string LBL_GAMELOGO = "LBL_GAMELOGO";      // Game logo
        public const string LBL_LUCAS = "LBL_LUCAS";            // LucasArts logo
        public const string LBL_MENUBG = "LBL_MENUBG";         // Menu background (K1)
        public const string LBL_NEWCONTENT = "LBL_NEWCONTENT";  // New content label

        // Listbox - Reva LoadFromLayout: LB_MODULES
        public const string LB_MODULES = "LB_MODULES";         // Module list for warp
    }

    /// <summary>
    /// GUI and asset resrefs per game. Reva: CSWGuiMainMenu::LoadFromLayout @ 0x0067ace0 uses ResRef "mainmenu";
    /// GetFullScreenBG @ 0x0040a900 yields e.g. "1600x1200back" for K1; 3D model "mainmenu" in CSWGuiMainMenu @ 0x0067c4c0.
    /// </summary>
    public static class MainMenuResRefs
    {
        /// <summary>K1: mainmenu16x12. Reva LoadFromLayout uses layout "mainmenu".</summary>
        public const string K1_GuiResRef = "mainmenu16x12";

        /// <summary>TSL: mainmenu8x6_p.</summary>
        public const string K2_GuiResRef = "mainmenu8x6_p";

        /// <summary>K1 background texture. Reva GetFullScreenBG @ 0x0040a900 returns resolution+"back" (e.g. 1600x1200back).</summary>
        public const string K1_Background = "1600x1200back";

        /// <summary>TSL has no background texture.</summary>
        public const string K2_Background = "";

        /// <summary>K1 main menu music.</summary>
        public const string K1_MusicResRef = "mus_theme_cult";

        /// <summary>TSL main menu music.</summary>
        public const string K2_MusicResRef = "mus_sion";

        /// <summary>K1 3D model for LBL_3DVIEW. Reva CSWGuiMainMenu @ 0x0067c4c0: CSWGuiScene::AddModel("mainmenu").</summary>
        public const string K1_ModelResRef = "mainmenu";

        /// <summary>TSL 3D model for LBL_3DVIEW: mainmenu01.</summary>
        public const string K2_ModelResRef = "mainmenu01";
    }

    /// <summary>
    /// Default colors for main menu buttons (K1 vs TSL). Reva: button colors from GUI/control data.
    /// </summary>
    public static class MainMenuColors
    {
        // K1
        public static readonly Color K1_BaseColor = new Color(0.0f, 0.658824f, 0.980392f, 1f);       // Cyan-blue
        public static readonly Color K1_HilightColor = new Color(1f, 1f, 0f, 1f);                     // Yellow

        // TSL
        public static readonly Color K2_BaseColor = new Color(0.10196078568697f, 0.69803923368454f, 0.549019634723663f, 1f);  // Teal
        public static readonly Color K2_HilightColor = new Color(0.8f, 0.8f, 0.6980392336845398f, 1f);                        // Light yellow

        // Void fill
        public static readonly Color VoidFillK1 = new Color(0.10196078568697f, 0.69803923368454f, 0.549019634723663f, 1f);   // Teal
        public static readonly Color VoidFillK2 = new Color(0.10196078568697f, 0.69803923368454f, 0.549019634723663f, 1f);   // Same

        // Fallback background (no texture) - dark blue
        public static readonly Color FallbackBackground = new Color(20, 30, 60, 255);
    }

    /// <summary>
    /// Visibility and behavior per control. Reva: CSWGuiMainMenu @ 0x0067c4c0 sets warp_button, newcontent_button,
    /// modules_listbox bit_flags &amp;= 0xfffffffd (hidden).
    /// </summary>
    public static class MainMenuVisibility
    {
        /// <summary>Hidden by default in all: LB_MODULES.</summary>
        public const string HiddenAlways = "LB_MODULES,LBL_BW,LBL_LUCAS,LBL_NEWCONTENT";

        /// <summary>K1: BTN_WARP hidden unless developer. Reva: warp_button.bit_flags &amp;= 0xfffffffd.</summary>
        public const string WarpButton = "BTN_WARP";

        /// <summary>BTN_MOVIES, BTN_OPTIONS (we show enabled).</summary>
        public const string OptionalButtons = "BTN_MOVIES,BTN_OPTIONS";

        /// <summary>BTN_MOREGAMES, BTN_TSLRCM hidden if present.</summary>
        public const string OptionalHidden = "BTN_MOREGAMES,BTN_TSLRCM";
    }

    /// <summary>
    /// Button order for keyboard/gamepad: NEWGAME -> LOADGAME -> MOVIES -> OPTIONS -> EXIT (wrap to NEWGAME).
    /// Reva: CSWGuiPanel::SetActiveControl(newgame_button) in CSWGuiMainMenu @ 0x0067c4c0.
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
    /// Reva: CSWGuiManager @ 0x0040bad0 sets resolution string "800x600"; GetScreenResolutionString @ 0x0040a3e0.
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
        /// <summary>K1 3D model scale. Reva CSWGuiMainMenu: camera FOV 0x41b5ced9, model "mainmenu".</summary>
        public const float KotorModelSize = 1.4f;
    }
}
