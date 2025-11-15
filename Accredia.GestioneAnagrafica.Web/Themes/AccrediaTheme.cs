using MudBlazor;

namespace Accredia.GestioneAnagrafica.Web.Themes;

/// <summary>
/// Tema personalizzato MudBlazor 6.20.0 per Accredia
/// Implementa la corporate identity ufficiale di Accredia
/// </summary>
public class AccrediaTheme : MudTheme
{
    public AccrediaTheme()
    {
        // Colori Accredia Corporate Identity
        PaletteLight = new PaletteLight
        {
            // Colori Primari Accredia
            Primary = "#1a1a2e",           // Grafite - Text, navbar, primary elements
            Secondary = "#d4a574",         // Ocra - Accents, hover states, emphasis
            Tertiary = "#f8f7f5",          // Écru - Alternative light color

            // Colori Semantici
            Success = "#2e7d32",           // Verde - Positive actions and confirmations
            Info = "#0277bd",              // Blu - Informational messages
            Warning = "#f57c00",           // Arancione - Alerts and warnings
            Error = "#c62828",             // Rosso - Errors and destructive actions

            // Backgrounds
            Background = "#f8f7f5",        // Écru - Page backgrounds, light surfaces
            BackgroundGray = "#f5f5f5",    // Light grey for alternative surfaces
            Surface = "#ffffff",           // Bianco - Cards, surfaces, contrast

            // Text Colors
            TextPrimary = "#1a1a2e",       // Grafite - Primary text
            TextSecondary = "#666666",     // Grey - Descriptions and meta
            TextDisabled = "#bdbdbd",      // Light grey - Inactive elements

            // AppBar specifico per Accredia
            AppbarBackground = "#1a1a2e",  // Grafite - Navbar background
            AppbarText = "#ffffff",        // White text on dark navbar

            // Drawer
            DrawerBackground = "#ffffff",  // White drawer
            DrawerText = "#1a1a2e",        // Grafite text in drawer
            DrawerIcon = "#1a1a2e",        // Grafite icons

            // Borders and Dividers
            Divider = "#e0e0e0",
            DividerLight = "#f0f0f0",

            // Links
            PrimaryContrastText = "#ffffff",
            SecondaryContrastText = "#1a1a2e",

            // Hover states con Ocra
            HoverOpacity = 0.08,

            // Action colors
            ActionDefault = "#666666",
            ActionDisabled = "#bdbdbd",
            ActionDisabledBackground = "#f5f5f5",

            // Table specifics
            TableLines = "#e0e0e0",
            TableStriped = "#fafafa",
            TableHover = "#f5f5f5",

            // Overlays
            OverlayDark = "rgba(26, 26, 46, 0.5)",  // Semi-transparent Grafite
            OverlayLight = "rgba(255, 255, 255, 0.5)",
        };

        PaletteDark = new PaletteDark
        {
            // Dark Mode con inversione colori Accredia
            Primary = "#d4a574",           // Ocra diventa primary in dark mode
            Secondary = "#f8f7f5",         // Écru come secondary
            Tertiary = "#1a1a2e",          // Grafite come tertiary

            // Semantic colors rimangono uguali ma più brillanti
            Success = "#4caf50",
            Info = "#29b6f6",
            Warning = "#ff9800",
            Error = "#ef5350",

            // Dark backgrounds
            Background = "#121212",
            BackgroundGray = "#1e1e1e",
            Surface = "#1e1e1e",

            // Text for dark mode
            TextPrimary = "#ffffff",
            TextSecondary = "#b0b0b0",
            TextDisabled = "#6c6c6c",

            // AppBar in dark mode
            AppbarBackground = "#1a1a2e",  // Mantiene Grafite
            AppbarText = "#ffffff",

            // Drawer in dark mode
            DrawerBackground = "#1e1e1e",
            DrawerText = "#ffffff",
            DrawerIcon = "#d4a574",        // Ocra per icone in dark mode

            // Borders and Dividers
            Divider = "#373737",
            DividerLight = "#2d2d2d",

            PrimaryContrastText = "#1a1a2e",
            SecondaryContrastText = "#1a1a2e",

            // Action colors dark mode
            ActionDefault = "#b0b0b0",
            ActionDisabled = "#6c6c6c",
            ActionDisabledBackground = "#2d2d2d",

            // Table in dark mode
            TableLines = "#373737",
            TableStriped = "#1a1a1a",
            TableHover = "#2d2d2d",

            // Overlays
            OverlayDark = "rgba(0, 0, 0, 0.7)",
            OverlayLight = "rgba(255, 255, 255, 0.15)",
        };

        // NOTE: Typography configuration is not supported in MudBlazor 6.20.0
        // Font customization should be done via CSS instead
        // Typography will use MudBlazor defaults for version 6.x

        // Layout Properties - Responsive Accredia
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "4px",

            // AppBar
            AppbarHeight = "64px",

            // Drawer - Larghezza standard per Accredia
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "260px",
            DrawerMiniWidthLeft = "56px",
            DrawerMiniWidthRight = "56px",
        };

        // Shadows - Material Design elevation
        Shadows = new Shadow
        {
            Elevation = new[]
            {
                "none",
                "0px 2px 1px -1px rgba(0,0,0,0.2),0px 1px 1px 0px rgba(0,0,0,0.14),0px 1px 3px 0px rgba(0,0,0,0.12)",
                "0px 3px 1px -2px rgba(0,0,0,0.2),0px 2px 2px 0px rgba(0,0,0,0.14),0px 1px 5px 0px rgba(0,0,0,0.12)",
                "0px 3px 3px -2px rgba(0,0,0,0.2),0px 3px 4px 0px rgba(0,0,0,0.14),0px 1px 8px 0px rgba(0,0,0,0.12)",
                "0px 2px 4px -1px rgba(0,0,0,0.2),0px 4px 5px 0px rgba(0,0,0,0.14),0px 1px 10px 0px rgba(0,0,0,0.12)",
                "0px 3px 5px -1px rgba(0,0,0,0.2),0px 5px 8px 0px rgba(0,0,0,0.14),0px 1px 14px 0px rgba(0,0,0,0.12)",
                "0px 3px 5px -1px rgba(0,0,0,0.2),0px 6px 10px 0px rgba(0,0,0,0.14),0px 1px 18px 0px rgba(0,0,0,0.12)",
                "0px 4px 5px -2px rgba(0,0,0,0.2),0px 7px 10px 1px rgba(0,0,0,0.14),0px 2px 16px 1px rgba(0,0,0,0.12)",
                "0px 5px 5px -3px rgba(0,0,0,0.2),0px 8px 10px 1px rgba(0,0,0,0.14),0px 3px 14px 2px rgba(0,0,0,0.12)",
                "0px 5px 6px -3px rgba(0,0,0,0.2),0px 9px 12px 1px rgba(0,0,0,0.14),0px 3px 16px 2px rgba(0,0,0,0.12)",
                "0px 6px 6px -3px rgba(0,0,0,0.2),0px 10px 14px 1px rgba(0,0,0,0.14),0px 4px 18px 3px rgba(0,0,0,0.12)",
                "0px 6px 7px -4px rgba(0,0,0,0.2),0px 11px 15px 1px rgba(0,0,0,0.14),0px 4px 20px 3px rgba(0,0,0,0.12)",
                "0px 7px 8px -4px rgba(0,0,0,0.2),0px 12px 17px 2px rgba(0,0,0,0.14),0px 5px 22px 4px rgba(0,0,0,0.12)",
                "0px 7px 8px -4px rgba(0,0,0,0.2),0px 13px 19px 2px rgba(0,0,0,0.14),0px 5px 24px 4px rgba(0,0,0,0.12)",
                "0px 7px 9px -4px rgba(0,0,0,0.2),0px 14px 21px 2px rgba(0,0,0,0.14),0px 5px 26px 4px rgba(0,0,0,0.12)",
                "0px 8px 9px -5px rgba(0,0,0,0.2),0px 15px 22px 2px rgba(0,0,0,0.14),0px 6px 28px 5px rgba(0,0,0,0.12)",
                "0px 8px 10px -5px rgba(0,0,0,0.2),0px 16px 24px 2px rgba(0,0,0,0.14),0px 6px 30px 5px rgba(0,0,0,0.12)",
                "0px 8px 11px -5px rgba(0,0,0,0.2),0px 17px 26px 2px rgba(0,0,0,0.14),0px 6px 32px 5px rgba(0,0,0,0.12)",
                "0px 9px 11px -5px rgba(0,0,0,0.2),0px 18px 28px 2px rgba(0,0,0,0.14),0px 7px 34px 6px rgba(0,0,0,0.12)",
                "0px 9px 12px -6px rgba(0,0,0,0.2),0px 19px 29px 2px rgba(0,0,0,0.14),0px 7px 36px 6px rgba(0,0,0,0.12)",
                "0px 10px 13px -6px rgba(0,0,0,0.2),0px 20px 31px 3px rgba(0,0,0,0.14),0px 8px 38px 7px rgba(0,0,0,0.12)",
                "0px 10px 13px -6px rgba(0,0,0,0.2),0px 21px 33px 3px rgba(0,0,0,0.14),0px 8px 40px 7px rgba(0,0,0,0.12)",
                "0px 10px 14px -6px rgba(0,0,0,0.2),0px 22px 35px 3px rgba(0,0,0,0.14),0px 8px 42px 7px rgba(0,0,0,0.12)",
                "0px 11px 14px -7px rgba(0,0,0,0.2),0px 23px 36px 3px rgba(0,0,0,0.14),0px 9px 44px 8px rgba(0,0,0,0.12)",
                "0px 11px 15px -7px rgba(0,0,0,0.2),0px 24px 38px 3px rgba(0,0,0,0.14),0px 9px 46px 8px rgba(0,0,0,0.12)",
            }
        };

        // Z-Index layers
        ZIndex = new ZIndex
        {
            Drawer = 1100,
            AppBar = 1200,      // AppBar con B maiuscola!
            Dialog = 1300,
            Popover = 1400,
            Snackbar = 1500,
            Tooltip = 1600
        };
    }
}
