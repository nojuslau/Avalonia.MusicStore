using iTunesSearch.Library;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.Models;
public class Album
{
    private static iTunesSearchManager _searchManager = new();
    private static HttpClient _httpClient = new();
    private string _cachePath => $"./Cache/{Artist} - {Title}";
    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }
    public static async Task<IEnumerable<Album>> SearchAsync(string searchTerm)
    {
        var query = await _searchManager.GetAlbumsAsync(searchTerm).ConfigureAwait(false);

        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName,
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }
    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if (File.Exists(_cachePath + ".bmp"))
        {
            return File.OpenRead(_cachePath + ".bmp");
        }
        else
        {
            var data = await _httpClient.GetByteArrayAsync(CoverUrl);
            return new MemoryStream(data);
        }
    }
}
