using System.Globalization;
using LojaGamesRetro.Forms;

namespace LojaGamesRetro;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        var cultura = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;
        Thread.CurrentThread.CurrentCulture = cultura;
        Thread.CurrentThread.CurrentUICulture = cultura;

        ApplicationConfiguration.Initialize();

        using (var splash = new SplashForm())
        {
            splash.ShowDialog();
        }

        Application.Run(new Form { Text = "Retro Games Store", Width = 800, Height = 600 });
    }
}
