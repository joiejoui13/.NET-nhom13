# 🏢 Hệ Thống Quản Lý Cửa Hàng Bán Lẻ - Kiến Trúc 3 Lớp Chuẩn

Dự án này được xây dựng theo mô hình **N-Tier (3 Lớp)** chuyên nghiệp, đảm bảo tính bảo mật cao, giao diện đồng nhất và khả năng làm việc nhóm song song hiệu quả.

---

## 📂 1. Cấu trúc Thư mục Thực tế & Chú thích

Dự án được tổ chức theo nguyên tắc **Module hóa (Feature-based)**. Mỗi chức năng được tách biệt hoàn toàn ở cả 3 tầng (12-12-12).

```
AssignmentApp/
├── 📂 GUI (Presentation Layer)
│   ├── 📂 Base/                --> ucBase, frmBase (Ép chuẩn 1000x700, màu nền F0F2F5)
│   ├── 📂 Forms/               --> frmMain (Điều hướng), frmAuth (Đăng nhập)
│   └── 📂 UserControls/        --> Chia theo Role: Admin, Sales, Warehouse (12 màn hình)
├── 📂 BLL (Business Logic Layer)
│   ├── 📂 Services/
│   │   ├── 📂 Security/        --> AuthService.cs (Xử lý Đăng nhập)
│   │   ├── 📂 Admin/           --> Promotion, Employee, Report Services
│   │   ├── 📂 Sales/           --> POS, Order, Delivery, Return, Customer Services
│   │   └── 📂 Warehouse/       --> Product, Category, StockIn, Inventory Services
│   ├── 📂 Security/            --> PasswordHasher.cs (Mã hóa BCrypt)
│   └── 📂 Session/             --> UserSession.cs (Lưu thông tin người dùng đang đăng nhập)
├── 📂 DAL (Data Access Layer)
│   ├── 📂 Core/                --> DbContext.cs (Kết nối SQL Server bằng Dapper)
│   ├── 📂 Repositories/
│   │   ├── 📂 Security/        --> AuthRepository.cs (Truy vấn đăng nhập)
│   │   ├── 📂 Admin/           --> UserRepository, PromotionRepository...
│   │   ├── 📂 Sales/           --> POSRepository, OrderRepository...
│   │   └── 📂 Warehouse/       --> ProductRepository (Mẫu chuẩn CRUD), CategoryRepository...
│   └── 📂 Scripts/             --> Database.sql (Kịch bản tạo bảng & tài khoản Test)
└── 📂 DTO (Data Transfer Objects)
    └── 📂 Models/              --> Các thực thể: User, Product, Order, Customer...
```

---

## 🛠️ 2. Hướng dẫn Quy trình làm việc (Workflow)

Để hoàn thiện một chức năng (ví dụ: Quản lý Sản phẩm), thành viên thực hiện theo 3 bước:

1.  **Thiết kế UI**: Mở `ucProductList.Designer.cs`. Kéo thả linh kiện Guna2 vào. Lưu ý: Mọi UC đều kế thừa `ucBase` nên đã có sẵn kích thước chuẩn.
2.  **Viết DAL**: Mở `ProductRepository.cs`. Viết các câu lệnh SQL bằng Dapper.
    *   *Dấu hiệu*: `string sql = "SELECT * FROM tblHangHoa WHERE MaHang = @MaHang";`
3.  **Viết BLL**: Mở `ProductService.cs`. Gọi hàm từ Repository và xử lý logic (nếu có).
4.  **Kết nối GUI**: Tại `ucProductList.cs`, gọi hàm từ `ProductService` để hiển thị dữ liệu lên GridView.

---

## 🏆 3. Các Tiêu chí Đạt điểm cao & Dấu hiệu nhận biết trong Code

| Tiêu chí | Giải thích khái niệm | Dấu hiệu nhận biết trong bài |
| :--- | :--- | :--- |
| **Kiến trúc 3 lớp** | Tách biệt GUI (Giao diện), BLL (Logic), DAL (Dữ liệu). | Thư mục được chia rõ: **GUI, BLL, DAL**. |
| **Repository Pattern** | Quản lý SQL tập trung, không viết SQL trong Form. | Các file kết thúc bằng **`Repository.cs`** trong tầng DAL. |
| **Dapper (Micro-ORM)** | Kết nối DB tốc độ cao, ánh xạ dữ liệu tự động. | Thấy `using Dapper;` và lệnh `.QueryAsync<T>` trong DAL. |
| **Bảo mật BCrypt** | Mã hóa mật khẩu kèm Salt ngẫu nhiên, chống bẻ khóa. | Xem `PasswordHasher.cs`. Sử dụng thư viện `BCrypt.Net`. |
| **Chống SQL Injection** | Sử dụng tham số hóa (Parameters) cho mọi câu lệnh SQL. | Câu SQL có dấu **`@`** (Ví dụ: `@CCCD`, `@MaHang`). |
| **Consistent UI** | Giao diện luôn đồng bộ kích thước và trải nghiệm. | Các Form/UC đều kế thừa từ **`Base.frmBase`** hoặc **`Base.ucBase`**. |
| **Modern Charting** | Biểu đồ hiện đại hỗ trợ .NET 8. | Sử dụng **`LiveChartsCore.SkiaSharpView`**. |

---

## 🤖 4. Prompt dành cho Thành viên mới (Clone bài)

Hãy copy lệnh sau vào AI Agent (Antigravity/Cursor) khi bạn vừa clone bài về:

> "Tôi vừa clone dự án .NET 8 này. Hãy giúp tôi hoàn thiện môi trường:
> 1. Kiểm tra kết nối SQL trong `DAL/Core/DbContext.cs` và cập nhật đúng Server của tôi.
> 2. Chạy tệp `DAL/Scripts/Database.sql` để tạo 3 tài khoản Test và bảng mẫu.
> 3. Chạy `dotnet restore` để nạp các thư viện: Guna2, Dapper, BCrypt, LiveChartsCore.
> 4. Hướng dẫn tôi cách sử dụng file `ProductRepository.cs` làm mẫu để tôi tự làm module của mình."

---

## 🔑 5. Tài khoản Test & Phân quyền

| Vai trò | Tài khoản (CCCD) | Mật khẩu | Chức năng hiển thị |
| :--- | :--- | :--- | :--- |
| **Quản lý (ADMIN)** | `ADMIN` | `123` | Toàn quyền tất cả chức năng. |
| **Bán hàng (SALES)** | `SALES` | `123` | POS, Đơn hàng, Giao hàng, Trả hàng, Khách hàng. |
| **Kho (WAREHOUSE)** | `WAREHOUSE` | `123` | Sản phẩm, Danh mục, Nhập kho, Tồn kho. |

*(Lưu ý: Hệ thống đã tích hợp PasswordHasher, các mật khẩu trên sẽ được Hash khi lưu chính thức).*