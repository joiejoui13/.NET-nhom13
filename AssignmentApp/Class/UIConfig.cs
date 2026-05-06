using System.Drawing;
using MaterialSkin;
using MaterialSkin.Controls;
using Sunny.UI;

namespace AssignmentApp.Class;

public static class UIConfig
{
    //// Bảng màu chung - Đã chuyển sang tông Blue (Xanh nước biển)
    //public static readonly Color ColorPrimary = Color.FromArgb(25, 118, 210); // Blue 700
    //public static readonly Color ColorDark = Color.FromArgb(13, 71, 161);    // Blue 900
    //public static readonly Color ColorAccent = Color.FromArgb(3, 169, 244);   // Light Blue 500

    ////1. Cấu hình cho MaterialSkin(Dùng cho AuthForm của bạn)
    //public static void ApplyMaterialStyle(MaterialForm form)
    //{
    //    var manager = MaterialSkinManager.Instance;
    //    manager.AddFormToManage(form);
    //    manager.Theme = MaterialSkinManager.Themes.LIGHT;

    //    // Sử dụng tông màu Blue để ra màu xanh nước biển chuẩn
    //    manager.ColorScheme = new MaterialSkin.ColorScheme(
    //        MaterialSkin.Primary.Blue800,
    //        MaterialSkin.Primary.Blue900,
    //        MaterialSkin.Primary.Blue500,
    //        MaterialSkin.Accent.LightBlue200,
    //        MaterialSkin.TextShade.WHITE
    //    );
    //}

    //// 2. Cấu hình cho Sunny.UI
    //public static void ApplySunnyStyle(UIForm form)
    //{
    //    form.Style = UIStyle.Custom;
    //    form.TitleColor = ColorPrimary;
    //    form.TitleForeColor = Color.White;
    //    form.RectColor = ColorPrimary;
    //}

    //public static void ApplySunnyButtonStyle(UIButton btn)
    //{
    //    btn.Style = UIStyle.Custom;
    //    btn.FillColor = ColorPrimary;
    //    btn.RectColor = ColorPrimary;
    //    btn.FillHoverColor = ColorAccent;
    //    btn.ForeColor = Color.White;
    //}

    /* 
    // Tạm thời đóng đoạn này vì bạn chưa dùng đến và để tránh lỗi xung đột thư viện
    public static void ApplyReaLTaiizorStyle(ReaLTaiizor.Forms.MaterialForm form)
    {
        var manager = ReaLTaiizor.Manager.MaterialSkinManager.Instance;
        manager.AddFormToManage(form);
        manager.Theme = ReaLTaiizor.Manager.MaterialSkinManager.Themes.LIGHT;
        // manager.ColorScheme = ...
    }
    */
}
