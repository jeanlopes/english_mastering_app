
using System.Text.RegularExpressions;
using english_mastering_app;
using HtmlAgilityPack;

const string PATH = "D:/Documentos/workspace/english_mastering_app";

Console.WriteLine("Digite o nome do filme ou seriado:");
var searchTerm = Console.ReadLine();

Console.WriteLine("Digite o número da temporada (caso seja um seriado):");
var seasonNumber = Console.ReadLine();


var links = GetMovieNameLinks(searchTerm, seasonNumber);
Console.WriteLine("Escolha o título do filme ou seriado");

var i = 1;
links.ForEach(link =>
{
    Console.WriteLine($"{i} - {link.InnerText}");
    i++;
});

var sel = Convert.ToInt32(Console.ReadLine());
var link = links[sel - 1].Attributes["href"].Value;

Console.WriteLine("Escolha a legenda para baixar");

var subtitleLinks = GetSubtitleLinks(link);

i = 1;
subtitleLinks.ForEach(sub =>
{
    Console.WriteLine($"{i} - {sub.Title}");
    i++;
});

sel = Convert.ToInt32(Console.ReadLine());
var input = subtitleLinks[sel - 1].Link;

var pattern = @"\D"; // Remove tudo exceto os números (\D é o inverso de \d)

var result = Regex.Replace(input, pattern, "");

var subtitleUrlDownload = $"https://www.opensubtitles.org/en/subtitleserve/sub/{result}";

await DownloadFileFromUrl(subtitleUrlDownload, PATH);

Console.WriteLine("Download concluido");

static List<HtmlNode> GetMovieNameLinks(string? searchTerm, string? seasonNumber)
{
    if (searchTerm == null)
        throw new Exception("Termo de busca não pode ser nulo");

    searchTerm = !string.IsNullOrWhiteSpace(seasonNumber) ? searchTerm + "-s" + seasonNumber : searchTerm;

    var web = new HtmlWeb();
    HtmlNodeCollection? movieTitles;
    var movieTitleCollection = new List<HtmlNode>();
    var offset = 0;
    do
    {
        var pagination = offset > 0 ? "/offset-" + offset : "";
        var searchUrl = $"https://www.opensubtitles.org/en/search2/sublanguageid-eng/moviename-{Uri.EscapeDataString(searchTerm)}{pagination}";
        var document = web.Load(searchUrl);
        movieTitles = document.DocumentNode.SelectNodes("//a[@class='bnone']");
        if (movieTitles != null)
        {
            foreach (var title in movieTitles)
            {
                movieTitleCollection.Add(title);
            }
        }
        else break;
        offset += 40;

    } while (true);

    return movieTitleCollection;
}

static List<SubtitleDto> GetSubtitleLinks(string link){
    var web = new HtmlWeb();
    HtmlNodeCollection? subtitleLinks;
    HtmlNodeCollection? subtitleNames;
    var movieTitleCollection = new List<SubtitleDto>();
    var offset = 0;

    do
    {
        var pagination = offset > 0 ? "/offset-" + offset : "";
        var searchUrl = $"https://www.opensubtitles.org{link}/{pagination}";
        var document = web.Load(searchUrl);
        subtitleLinks = document.DocumentNode.SelectNodes("//a[@class='bnone']");
        subtitleNames = document.DocumentNode.SelectNodes("//a[@class='bnone']/../..");
        if (subtitleLinks != null)
        {
            var i = 0;
            foreach (var title in subtitleLinks)
            {
                var name = subtitleNames[i].InnerText;
                name = name
                    .Split("&amp;")[0]
                    .Replace("onlineDownload Subtitles Searcher", "")
                    .Replace("\n", "")
                    .Replace("\r", "");
                    
                movieTitleCollection.Add(new SubtitleDto(name, title.Attributes["href"].Value));
                i++;
            }
        }
        else break;
        offset += 40;

    } while (true);
    
    return movieTitleCollection;
}

static async Task DownloadFileFromUrl(string url, string savePath)
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var fileName = response.Content.Headers?.ContentDisposition?.FileName;
    byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
    await File.WriteAllBytesAsync(savePath+"/subtitle.zip", fileContent);
    
}