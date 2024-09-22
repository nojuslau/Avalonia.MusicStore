using Avalonia.Media.Imaging;
using Avalonia.MusicStore.Models;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.ViewModels;
public class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;
    public string Artist => _album.Artist;
    public string Title => _album.Title;
    private Bitmap? _cover;
    public AlbumViewModel(Album album)
    {
        _album = album;
    }
    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }
    public async Task LoadCover()
    {
        await using (var imageStream = await _album.LoadCoverBitmapAsync())
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            Console.WriteLine(Cover);
        }
    }
}
