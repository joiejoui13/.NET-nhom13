namespace AssignmentApp;
using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using AssignmentApp.DAL.Repositories.Main;
using AssignmentApp.BLL.Services.Main;
using AssignmentApp.GUI;
using AssignmentApp.GUI.Forms;

static class Program
{
    // Biến static toàn cục quản lý ServiceProvider để sử dụng khi cần thiết
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 1. Tạo tập hợp đăng ký Service
        var services = new ServiceCollection();
        ConfigureServices(services);

        // 2. Build ServiceProvider quản lý tự động
        var serviceProvider = services.BuildServiceProvider();
        ServiceProvider = serviceProvider;
        using (serviceProvider)
        {
            // Trích xuất frmAuth từ container. Hệ thống sẽ tự tạo AuthRepository -> tiêm vào AuthService -> tiêm vào frmAuth!
            var loginForm = ServiceProvider.GetRequiredService<frmAuth>();
            Application.Run(loginForm);
        }
    }    

    private static void ConfigureServices(IServiceCollection services)
    {
        // Đăng ký Repository (DAL) dưới dạng Transient (tạo mới mỗi lần yêu cầu để an toàn thread)
        services.AddTransient<IAuthRepository, AuthRepository>();
        services.AddTransient<IMainRepository, MainRepository>();

        // Đăng ký Service nghiệp vụ (BLL)
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IMainService, MainService>();

        // Đăng ký Form Giao diện (GUI)
        services.AddTransient<frmAuth>();
        services.AddTransient<frmMain>();
    }
}