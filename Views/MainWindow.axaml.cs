using Avalonia.Controls;
using SvcSystems.UI.Terminal;

namespace MarkeratorFast.Views;

public partial class MainWindow : Window
{
    private readonly TerminalControlModel _terminal = new();
    
    public MainWindow()
    {
        InitializeComponent();
        TerminalView.Model = _terminal;
        
        _terminal.Feed(@$"./markerator -t Bengineering -i index.md --favicon true -op About.md,Contact.md --posts true -pt News -u https://bengineeri.ng -rss true -ri true -pp 10 -c Themes/BlueDark.css\r\n" +
                       "Created directory output//home/flecko/Projects/Markerator/bin/Debug/net10.0/output/css to store CSS files.\r\n" +
                       "Copied /home/flecko/Projects/Markerator/bin/Debug/net10.0/input/Themes/BlueDark.css -> /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/css/BlueDark.css\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/index.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/BigUpdates.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/LongTime.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/RssLinksAreHere.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/RssHasLanded.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/2024.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/MyBirthday.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/Activity.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News/Welcome.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/News.xml\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/About.html\r\n" +
                       "Generated /home/flecko/Projects/Markerator/bin/Debug/net10.0/output/Contact.html\r\n" +
                       "Site generated successfully.\n\r\n");
    }
}