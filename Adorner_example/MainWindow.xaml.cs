using ResizingAdomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Adorner_example
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
   
    public partial class MainWindow : Window
    {

        private AdornerLayer adornerLayer;
        public Image testImage;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.MainInkCanvas);


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MainInkCanvas.EditingMode = InkCanvasEditingMode.None;

            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.MainInkCanvas);


            testImage = new Image();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit(); //表示BitmapImage初始化開始
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;//加載時，將整個圖像緩存到RAM中。
            bitmapImage.UriSource = new Uri(@"video-game.png", UriKind.Relative);//獲取或設置BitmapImage的Uri源
            bitmapImage.EndInit();//表示BitmapImage初始化結束

            testImage.Source = bitmapImage;
            testImage.Height = bitmapImage.Height; //double.NAN
            testImage.Width = bitmapImage.Width;
            testImage.Loaded += new RoutedEventHandler(Image_Loaded);
            testImage.Margin = new Thickness(60, 60, -60, -60);
            testImage.Stretch = Stretch.Fill;
            MainInkCanvas.Children.Add(testImage);
        }
        private void Image_Loaded(object sender, RoutedEventArgs e)
        {

            //Console.WriteLine(e.Source);

            this.adornerLayer.Add(new ResizingAdorner((UIElement)e.Source));
        }
    }
}
