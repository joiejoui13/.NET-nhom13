using System;
using System.IO;
using System.Text.RegularExpressions;

class Program {
    static void Main() {
        string path = @"d:\0Project\Chmanet\.NET-nhom13\AssignmentApp\GUI\UserControls\Warehouse\ucStockIn.cs";
        string content = File.ReadAllText(path);

        // 1. Remove SetEditState definition
        string setEditStatePattern = @"\s*// 5\.2\.6\. Thay d?i tr?ng thái khóa/m? các ô nh?p li?u\s*private void SetEditState\(bool editing\)\s*\{[^\}]*\}[^\}]*\}";
        content = Regex.Replace(content, setEditStatePattern, "");

        // 2. Add ResetTab1State and ResetTab2State right before // ======================================================== \n // TAB 1 EVENTS
        string resetStates = @"
        private void ResetTab1State()
        {
            isEditing = false;
            isAddingNew = false;
            
            txtMaPhieuNhap.Text = """";
            dtNgayNhap.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
            txtNguoiDung.Text = """";

            txtMaPhieuNhap.ReadOnly = true;
            txtMaPhieuNhap.Enabled = false; // T?t ch? d? tìm ki?m
            dtNgayNhap.Enabled = false;
            cboTrangThai.Enabled = false;

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;
            
            dgvDetails.ClearSelection();
            currentDetails.Clear();
        }

        private void ResetTab2State()
        {
            btnAddToCart.Enabled = true;
            guna2Button4.Enabled = false; // S?a
            btnRemoveFromCart.Enabled = false; // Xóa
            guna2Button3.Enabled = false; // B? qua
            
            // Nút Luu (btnBackToReceipt) sáng n?u gi? có hàng
            btnBackToReceipt.Enabled = (currentDetails.Count > 0);
            
            txtSelMaSP.Text = """";
            txtSelTenSP.Text = """";
            txtSelSoLuong.Text = """";
            txtSelGiaNhap.Text = """";
            lblTotalAmount.Text = ""T?NG TI?N T?M TÍNH: 0 d"";
            dgvCurrentDetails.ClearSelection();
        }

";
        content = content.Replace("// ========================================================\r\n        // TAB 1 EVENTS", resetStates + "// ========================================================\r\n        // TAB 1 EVENTS");

        // 3. Update ucStockIn_Load
        string loadPattern = @"// Ch?n dòng d?u tiên n?u có d? li?u\s*if \(dgvDetails\.Rows\.Count > 0\)\s*\{\s*SelectReceiptRow\(0\);\s*\}\s*// Ðua các nút và các tru?ng thông tin v? tr?ng thái khóa\s*SetEditState\(false\);";
        content = Regex.Replace(content, loadPattern, "ResetTab1State();\n            ResetTab2State();");

        // 4. Update dgvDetails_CellClick
        string cellClickPattern = @"SelectReceiptRow\(e\.RowIndex\);";
        content = Regex.Replace(content, cellClickPattern, "SelectReceiptRow(e.RowIndex);\n                \n                if (txtMaPhieuNhap.Enabled == true) { txtMaPhieuNhap.Enabled = false; btnSearch.Enabled = true; }\n\n                btnAdd.Enabled = false;\n                btnEdit.Enabled = true;\n                btnDelete.Enabled = true;\n                btnSave.Enabled = false;\n                btnCancel.Enabled = true;");

        // 5. Update btnAdd_Click
        string btnAddPattern = @"SetEditState\(true\);";
        content = Regex.Replace(content, btnAddPattern, "btnAdd.Enabled = false;\n                btnEdit.Enabled = false;\n                btnDelete.Enabled = false;\n                btnSave.Enabled = true;\n                btnCancel.Enabled = true;\n\n                txtMaPhieuNhap.ReadOnly = true;\n                dtNgayNhap.Enabled = true;\n                cboTrangThai.Enabled = true;\n\n                ResetTab2State();");

        // 6. Update btnEdit_Click
        string btnEditPattern = @"SetEditState\(true\);";
        content = Regex.Replace(content, btnEditPattern, "btnAdd.Enabled = false;\n            btnEdit.Enabled = false;\n            btnDelete.Enabled = false;\n            btnSave.Enabled = true;\n            btnCancel.Enabled = true;\n\n            txtMaPhieuNhap.ReadOnly = true;\n            dtNgayNhap.Enabled = true;\n            cboTrangThai.Enabled = true;\n\n            ResetTab2State();");

        // 7. Update btnDelete_Click
        string btnDelPattern = @"if \(dgvDetails\.Rows\.Count > 0\)\s*\{\s*SelectReceiptRow\(0\);\s*\}\s*else\s*\{\s*txtMaPhieuNhap\.Text = """";\s*txtNguoiDung\.Text = """";\s*cboTrangThai\.SelectedIndex = -1;\s*currentDetails\.Clear\(\);\s*\}\s*SetEditState\(false\);";
        content = Regex.Replace(content, btnDelPattern, "ResetTab1State();\n                    ResetTab2State();");

        // 8. Update btnSave_Click
        string saveOld = @"if \(isAddingNew\)\s*\{\s*SelectReceiptRow\(dgvDetails\.Rows\.Count - 1\);\s*\}\s*else\s*\{\s*SelectReceiptRow\(dgvDetails\.CurrentRow\.Index\);\s*\}\s*SetEditState\(false\);";
        content = Regex.Replace(content, saveOld, "ResetTab1State();\n            ResetTab2State();");

        // 9. Update btnCancel_Click
        string cancelOld = @"if \(dgvDetails\.Rows\.Count > 0\)\s*\{\s*SelectReceiptRow\(dgvDetails\.CurrentRow\.Index\);\s*\}\s*else\s*\{\s*txtMaPhieuNhap\.Text = """";\s*txtNguoiDung\.Text = """";\s*cboTrangThai\.SelectedIndex = -1;\s*currentDetails\.Clear\(\);\s*\}\s*SetEditState\(false\);";
        content = Regex.Replace(content, cancelOld, "ResetTab1State();\n            ResetTab2State();");

        // 10. Update dgvCurrentDetails_CellClick
        string dgvTab2Click = @"private void dgvCurrentDetails_CellClick\(object sender, DataGridViewCellEventArgs e\)\s*\{\s*if \(e\.RowIndex >= 0\)\s*\{\s*DataGridViewRow row = dgvCurrentDetails\.Rows\[e\.RowIndex\];\s*txtSelMaSP\.Text = row\.Cells\[0\]\.Value\.ToString\(\);\s*txtSelTenSP\.Text = row\.Cells\[1\]\.Value\.ToString\(\);\s*txtSelSoLuong\.Text = row\.Cells\[2\]\.Value\.ToString\(\);\s*txtSelGiaNhap\.Text = row\.Cells\[3\]\.Value\.ToString\(\);\s*\}";
        string dgvTab2ClickNew = @"private void dgvCurrentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCurrentDetails.Rows[e.RowIndex];
                txtSelMaSP.Text = row.Cells[0].Value.ToString();
                txtSelTenSP.Text = row.Cells[1].Value.ToString();
                txtSelSoLuong.Text = row.Cells[2].Value.ToString();
                txtSelGiaNhap.Text = row.Cells[3].Value.ToString();
                
                btnAddToCart.Enabled = false;
                guna2Button4.Enabled = true;
                btnRemoveFromCart.Enabled = true;
                guna2Button3.Enabled = true;
            }";
        content = Regex.Replace(content, dgvTab2Click, dgvTab2ClickNew);

        // 11. Add ResetTab2State to btnAddToCart_Click end
        content = content.Replace("UpdateCartGrid();\r\n            UpdateTotalAmount();", "UpdateCartGrid();\r\n            UpdateTotalAmount();\r\n            ResetTab2State();");

        // 12. Add ResetTab2State to guna2Button4_Click (S?a) end
        content = content.Replace("UpdateCartGrid();\r\n                UpdateTotalAmount();", "UpdateCartGrid();\r\n                UpdateTotalAmount();\r\n                ResetTab2State();");

        // 13. Add ResetTab2State to btnRemoveFromCart_Click end
        content = content.Replace("UpdateCartGrid();\r\n                    UpdateTotalAmount();", "UpdateCartGrid();\r\n                    UpdateTotalAmount();\r\n                    ResetTab2State();");

        // 14. guna2Button3_Click (B? qua Tab 2)
        string guna3Click = @"private void guna2Button3_Click\(object sender, EventArgs e\)\s*\{\s*txtSelMaSP\.Text = """";\s*txtSelTenSP\.Text = """";\s*txtSelSoLuong\.Text = """";\s*txtSelGiaNhap\.Text = """";\s*\}";
        content = Regex.Replace(content, guna3Click, "private void guna2Button3_Click(object sender, EventArgs e)\n        {\n            ResetTab2State();\n        }");

        // 15. Update btnSearch_Click
        string searchClickOld = @"private void btnSearch_Click\(object sender, EventArgs e\)\s*\{\s*// L?N 1: Kích ho?t ch? d? tìm ki?m\s*if \(!txtMaPhieuNhap\.ReadOnly && \!isAddingNew\)\s*\{";
        string searchClickNew = @"private void btnSearch_Click(object sender, EventArgs e)
        {
            // L?N 1: Kích ho?t ch? d? tìm ki?m
            if (txtMaPhieuNhap.Enabled == false && txtMaPhieuNhap.ReadOnly == true)
            {
                ResetTab1State();
                txtMaPhieuNhap.ReadOnly = false;
                txtMaPhieuNhap.Enabled = true;
                
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = true;
                
                MessageBox.Show(""Ch? d? tìm ki?m dã b?t! Vui lòng nh?p Mã phi?u nh?p r?i ?n nút Tìm ki?m l?n n?a."", ""Thông báo"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaPhieuNhap.Focus();
                return;
            }

            // L?N 2: Th?c hi?n tìm ki?m
            if (txtMaPhieuNhap.Enabled == true)";
        content = content.Replace("private void btnSearch_Click(object sender, EventArgs e)\r\n        {\r\n            // L?N 1: Kích ho?t ch? d? tìm ki?m\r\n            if (!txtMaPhieuNhap.ReadOnly && !isAddingNew)\r\n            {", searchClickNew);

        // 16. Update btnRefresh_Click
        string refreshOld = @"private void btnRefresh_Click\(object sender, EventArgs e\)\s*\{\s*txtMaPhieuNhap\.ReadOnly = true;\s*LoadReceiptsGrid\(\);\s*if \(dgvDetails\.Rows\.Count > 0\)\s*SelectReceiptRow\(0\);\s*SetEditState\(false\);\s*\}";
        string refreshNew = @"private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReceiptsGrid();
            ResetTab1State();
        }";
        content = Regex.Replace(content, refreshOld, refreshNew);

        // Remove SetEditState calls completely if any remained
        content = Regex.Replace(content, @"SetEditState\((true|false)\);", "");

        File.WriteAllText(path, content);
        Console.WriteLine("Success!");
    }
}
