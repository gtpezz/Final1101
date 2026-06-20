using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TradeTerminal.Desktop;

public class ImagePathConverter : IValueConverter
{
    private readonly string _imagesPath;
    private readonly string _defaultImageName = "picture.png";

    public ImagePathConverter()
    {
        _imagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return GetDefaultImage();
        }

        try
        {
            var fileName = value.ToString();
            var filePath = Path.Combine(_imagesPath, fileName);

            if (File.Exists(filePath))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }
        catch
        { }

        return GetDefaultImage();
    }

    private BitmapImage GetDefaultImage()
    {
        try
        {
            var defaultPath = Path.Combine(_imagesPath, _defaultImageName);
            if (File.Exists(defaultPath))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(defaultPath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }
        catch { }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}