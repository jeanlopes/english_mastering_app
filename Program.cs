
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
    


    var web = new HtmlWeb();
    

    HtmlNodeCollection? movieTitles;
    var movieTitleCollection = new HtmlNodeCollection();
    var offset = 0
    do
    {
        var pagination = offset > 0 ? "/offset-" + offset : "";
        var searchUrl = $"https://www.opensubtitles.org/en/search2/sublanguageid-eng/moviename-{Uri.EscapeDataString(searchTerm)}{ pagination }";
        var document = web.Load(searchUrl);
        movieTitles = document.DocumentNode.SelectNodes("//a[@class='bnone']");
        if (movieTitles != null)
        {
            foreach (var title in movieTitles)
            {
                movieTitleCollection.Add(title);
            }
            break;
        }
        offset += 40;

    } while (true)
    


    Console.WriteLine(movieTitleCollection.Count);




}


// /offset-40