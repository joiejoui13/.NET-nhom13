using System;
using System.Windows.Forms;

namespace AssignmentApp.Class;

/// <summary>
/// Lớp chứa tất cả các hàm xử lý logic, tính toán, và gọi dữ liệu chung cho toàn bộ ứng dụng
/// </summary>
public static class Functions
{
    // Ví dụ 1: Hàm hiển thị thông báo dùng chung
    public static void ShowMessage(string message, string title = "Thông báo")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Ví dụ 2: Hàm hỏi xác nhận người dùng
    public static bool ConfirmAction(string question)
    {
        var result = MessageBox.Show(question, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return result == DialogResult.Yes;
    }

    // Ví dụ 3: Hàm xóa trắng các ô nhập liệu trên Form
    public static void ClearAllTextBoxes(Control form)
    {
        foreach (Control control in form.Controls)
        {
            if (control is TextBox)
            {
                ((TextBox)control).Clear();
            }
        }
    }

    // Hàm Login giả định xử lý Logic xác thực
    public static bool Login(string username, string password)
    {
        // Trong thực tế, bạn sẽ viết code kiểm tra Database ở đây
        // Trả về true nếu đúng, false nếu sai
        if (username == "admin" && password == "123")
        {
            return true;
        }
        return false;
    }
}
