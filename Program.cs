
using HtmlAgilityPack;

Console.WriteLine("Digite o nome do filme ou seriado:");
var searchTerm = Console.ReadLine();

Console.WriteLine("Digite o número da temporada (caso seja um seriado):");
var seasonNumber = Console.ReadLine();

// Obter os links das legendas
GetSubtitleLinks(searchTerm, seasonNumber);

// Resto do código...


static void GetSubtitleLinks(string? searchTerm, string? seasonNumber)
{

    if (searchTerm == null)
        throw new Exception("Termo de busca não pode ser nulo");

    searchTerm = !string.IsNullOrWhiteSpace(seasonNumber) ? searchTerm + "-s" + seasonNumber : searchTerm;
    string searchUrl = $"https://www.opensubtitles.org/en/search2/sublanguageid-eng/moviename-{Uri.EscapeDataString(searchTerm)}";


    var web = new HtmlWeb();
    var document = web.Load(searchUrl);


    var movieTitles = document.DocumentNode.SelectNodes("//a[@class='bnone']");





}


// /offset-40