using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NxAutoAio.WinUI
{
    /// <summary>
    /// Quản lý vòng đời ứng dụng và tạo cửa sổ chính khi ứng dụng được mở.
    /// </summary>
    public partial class App : Application
    {
        // Giữ tham chiếu để cửa sổ chính tồn tại trong suốt vòng đời ứng dụng.
        private Window? _window;

        /// <summary>
        /// Khởi tạo tài nguyên XAML của ứng dụng trước khi xử lý sự kiện khởi chạy.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tạo và hiển thị cửa sổ chính khi ứng dụng khởi chạy.
        /// </summary>
        /// <param name="args">Thông tin về yêu cầu khởi chạy ứng dụng.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
