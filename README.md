# 🏢 Quản Lý Cửa Hàng Bán Lẻ - Assignment .NET

Đây là bài tập lớn môn lập trình .NET, xây dựng ứng dụng quản lý cửa hàng sử dụng **Windows Forms** và **SQL Server**.

## 🛠️ Công nghệ sử dụng

* **Ngôn ngữ**: C#
* **Framework**: .NET 8.0 (Windows Forms)
* **Database**: SQL Server
* **Thư viện UI**:
    * Material Skin 2
    * SunnyUI
    * ReaLTaiizor

## 📂 Cấu trúc dự án

```
.NET-nhom13/
├── AssignmentApp/              # Ứng dụng chính
│   ├── Page/                 # Các trang giao diện
│   │   ├── RoleAdmin/        # Giao diện Admin
│   │   └── RolePos/          # Giao diện POS
│   ├── Components/           # Các thành phần tùy chỉnh
│   │   └── MyCustomCard.cs   # Card tùy chỉnh
│   ├── Class/                # Các lớp hỗ trợ
│   │   ├── Functions.cs      # Hàm kết nối và xử lý DB chung
│   │   ├── FunctionsAdmin.cs # Hàm xử lý Admin
│   │   └── FunctionsPos.cs   # Hàm xử lý POS
│   └── Form1.cs              # Form đăng nhập (AuthController)
├── AssignmentDBScript.sql      # File backup Database
└── README.md                   # File hướng dẫn này
```

## 🚀 Hướng dẫn cài đặt và chạy

### Bước 1: Khôi phục Database

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối đến SQL Server của bạn
3. Chạy file SQL: `AssignmentDBScript.sql`

### Bước 2: Cập nhật chuỗi kết nối

1. Mở file: `AssignmentApp/Class/Functions.cs`
2. Chỉnh sửa biến `connstring` cho phù hợp với kết nối của bạn:

```csharp
public static string connstring = @"
    Data Source=LAPTOP-TEEPQA0B\\SQLEXPRESS; 
    Initial Catalog=TestChamnetCK; 
    Integrated Security=True; 
    TrustServerCertificate=True"
```

Thay `LAPTOP-TEEPQA0B\\SQLEXPRESS` bằng tên server SQL của bạn.

### Bước 3: Mở và chạy dự án

1. Mở file solution:

```
.NET-nhom13/AssignmentApp.sln
```

2. Chạy dự án (nhấn **F5**)
3. Form đăng nhập sẽ hiện ra:
    * **Tài khoản**: `000000001`
    * **Mật khẩu**: `123456`

## 📋 Tài khoản mẫu

| Tài khoản (CCCD) | Mật khẩu | Vai trò |
|-------------------|----------|--------|
| `000000001`       | `123456` | POS    |
| `000000002`       | `123456` | Admin  |
| `000000003`       | `123456` | User   |

## 👥 Thành viên nhóm

* [Your Name 1]
* [Your Name 2]
* [Your Name 3]
* [Your Name 4]

---