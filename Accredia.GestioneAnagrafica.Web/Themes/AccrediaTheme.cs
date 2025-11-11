using MudBlazor;

namespace Accredia.GestioneAnagrafica.Web.Themes;

public class AccrediaTheme
{
    public const string Grafite = "#1a1a2e";
    public const string Ocra = "#d4a574";
    public const string Écru = "#f8f7f5";
    public const string Bianco = "#ffffff";

    public const string SuccessGreen = "#2e7d32";
    public const string WarningOrange = "#f57c00";
    public const string ErrorRed = "#c62828";
    public const string InfoBlue = "#0277bd";

    public const string TextPrimary = "#1a1a2e";
    public const string TextSecondary = "#666666";
    public const string TextDisabled = "#bdbdbd";

    public static MudTheme GetLightTheme()
    {
        return new MudTheme
        {
            Palette = new PaletteLight
            {
                Primary = Grafite,
                PrimaryContrastText = Bianco,
                Secondary = Ocra,
                SecondaryContrastText = Bianco,
                Success = SuccessGreen,
                SuccessContrastText = Bianco,
                Warning = WarningOrange,
                WarningContrastText = Bianco,
                Error = ErrorRed,
                ErrorContrastText = Bianco,
                Info = InfoBlue,
                InfoContrastText = Bianco,
                Background = Écru,
                BackgroundGrey = "#faf9f7",
                Surface = Bianco,
                TextPrimary = TextPrimary,
                TextSecondary = TextSecondary,
                TextDisabled = TextDisabled,
                Divider = "#e8e5e0",
                DividerLight = "#f5f3f0",
                ActionDefault = TextPrimary,
                ActionDisabled = TextDisabled,
                ActionDisabledBackground = "#faf9f7",
                AppbarBackground = Grafite,
                AppbarText = Bianco,
                DrawerBackground = Bianco,
                DrawerText = TextPrimary,
            },
            Typography = new Typography
            {
                Default = new Default
                {
                    FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = 400,
                    LineHeight = 1.5,
                    LetterSpacing = "0.03125em"
                },
                H1 = new H1 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "2.5rem", FontWeight = 700, LineHeight = 1.2, LetterSpacing = "-0.015625em" },
                H2 = new H2 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "2rem", FontWeight = 700, LineHeight = 1.3, LetterSpacing = "0em" },
                H3 = new H3 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1.75rem", FontWeight = 700, LineHeight = 1.33, LetterSpacing = "0.0125em" },
                H4 = new H4 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1.5rem", FontWeight = 700, LineHeight = 1.4, LetterSpacing = "0.0125em" },
                H5 = new H5 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1.25rem", FontWeight = 600, LineHeight = 1.5, LetterSpacing = "0em" },
                H6 = new H6 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1rem", FontWeight = 600, LineHeight = 1.6, LetterSpacing = "0.0125em" },
                Subtitle1 = new Subtitle1 { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1rem", FontWeight = 500, LineHeight = 1.75, LetterSpacing = "0.009375em" },
                Subtitle2 = new Subtitle2 { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.875rem", FontWeight = 500, LineHeight = 1.57, LetterSpacing = "0.0071428571em" },
                Body1 = new Body1 { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1rem", FontWeight = 400, LineHeight = 1.5, LetterSpacing = "0.03125em" },
                Body2 = new Body2 { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.875rem", FontWeight = 400, LineHeight = 1.43, LetterSpacing = "0.0125em" },
                Button = new Button { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.875rem", FontWeight = 600, LineHeight = 1.75, LetterSpacing = "0.0892857143em", TextTransform = "uppercase" },
                Caption = new Caption { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.75rem", FontWeight = 400, LineHeight = 1.66, LetterSpacing = "0.0333333333em" },
                Overline = new Overline { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.75rem", FontWeight = 600, LineHeight = 2.66, LetterSpacing = "0.1666666667em", TextTransform = "uppercase" }
            }
        };
    }

    public static MudTheme GetDarkTheme()
    {
        return new MudTheme
        {
            Palette = new PaletteLight
            {
                Primary = "#e8d0b0",
                PrimaryContrastText = Grafite,
                Secondary = "#d0d0e8",
                SecondaryContrastText = Grafite,
                Success = "#66bb6a",
                SuccessContrastText = Grafite,
                Warning = "#ffb74d",
                WarningContrastText = Grafite,
                Error = "#ef5350",
                ErrorContrastText = Bianco,
                Info = "#64b5f6",
                InfoContrastText = Grafite,
                Background = "#0f0f1e",
                BackgroundGrey = "#1a1a2e",
                Surface = "#1a1a2e",
                TextPrimary = "#f0f0f0",
                TextSecondary = "#b0b0b0",
                TextDisabled = "#606060",
                Divider = "#2a2a3e",
                DividerLight = "#252536",
                ActionDefault = "#f0f0f0",
                ActionDisabled = "#606060",
                ActionDisabledBackground = "#252536",
                AppbarBackground = Grafite,
                AppbarText = Bianco,
                DrawerBackground = "#1a1a2e",
                DrawerText = "#f0f0f0",
            },
            Typography = new Typography
            {
                Default = new Default { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1rem", FontWeight = 400, LineHeight = 1.5, LetterSpacing = "0.03125em" },
                H1 = new H1 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "2.5rem", FontWeight = 700, LineHeight = 1.2, LetterSpacing = "-0.015625em" },
                H2 = new H2 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "2rem", FontWeight = 700, LineHeight = 1.3, LetterSpacing = "0em" },
                H3 = new H3 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1.75rem", FontWeight = 700, LineHeight = 1.33, LetterSpacing = "0.0125em" },
                H4 = new H4 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1.5rem", FontWeight = 700, LineHeight = 1.4, LetterSpacing = "0.0125em" },
                H5 = new H5 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1.25rem", FontWeight = 600, LineHeight = 1.5, LetterSpacing = "0em" },
                H6 = new H6 { FontFamily = new[] { "Montserrat", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1rem", FontWeight = 600, LineHeight = 1.6, LetterSpacing = "0.0125em" },
                Body1 = new Body1 { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "1rem", FontWeight = 400, LineHeight = 1.5, LetterSpacing = "0.03125em" },
                Body2 = new Body2 { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.875rem", FontWeight = 400, LineHeight = 1.43, LetterSpacing = "0.0125em" },
                Caption = new Caption { FontFamily = new[] { "Open Sans", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, FontSize = "0.75rem", FontWeight = 400, LineHeight = 1.66, LetterSpacing = "0.0333333333em" }
            }
        };
    }
}
