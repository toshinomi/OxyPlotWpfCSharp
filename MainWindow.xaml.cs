using OxyPlot;
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

namespace OxyPlotWpfCSharp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] m_nHistgram = new int[256];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClickBtnFileSelect(object sender, RoutedEventArgs e)
        {
            ComOpenFileDialog openFileDlg = new ComOpenFileDialog();
            openFileDlg.Filter = "JPG|*.jpg|PNG|*.png";
            openFileDlg.Title = "Open the file";
            if (openFileDlg.ShowDialog() == true)
            {
                image.Source = null;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDlg.FileName);
                bitmap.EndInit();
                bitmap.Freeze();

                image.Source = bitmap;

                DrawHistgram(bitmap);
            }
            return;
        }

        public void DrawHistgram(BitmapImage _bitmap)
        {
            InitHistgram();

            CalHistgram(_bitmap);

            var dataList = new List<DataPoint>();
            for (int nIdx = 0; nIdx < 256; nIdx++)
            {
                var dataPoint = new DataPoint(nIdx, m_nHistgram[nIdx]);
                dataList.Add(dataPoint);
            }
            chart.ItemsSource = dataList;
        }

        public void CalHistgram(BitmapImage _bitmap)
        {
            int nWidthSize = _bitmap.PixelWidth;
            int nHeightSize = _bitmap.PixelHeight;

            WriteableBitmap wBitmap = new WriteableBitmap(_bitmap);

            int nIdxWidth;
            int nIdxHeight;

            unsafe
            {
                for (nIdxHeight = 0; nIdxHeight < nHeightSize; nIdxHeight++)
                {
                    for (nIdxWidth = 0; nIdxWidth < nWidthSize; nIdxWidth++)
                    {
                        byte* pPixel = (byte*)wBitmap.BackBuffer + nIdxHeight * wBitmap.BackBufferStride + nIdxWidth * 4;
                        byte nGrayScale = (byte)((pPixel[(int)ComInfo.Pixel.B] + pPixel[(int)ComInfo.Pixel.G] + pPixel[(int)ComInfo.Pixel.R]) / 3);

                        m_nHistgram[nGrayScale] += 1;
                    }
                }
            }
        }

        public void InitHistgram()
        {
            for (int nIdx = 0; nIdx < 256; nIdx++)
            {
                m_nHistgram[nIdx] = 0;
            }
        }
    }
}