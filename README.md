# Đề 6 - Quản lý đăng ký học phần & điểm (WinForms)

Ứng dụng desktop C# Windows Forms quản lý sinh viên, lớp, môn học, đăng ký học phần và điểm.
Dữ liệu lưu trong các tệp văn bản `.txt` (UTF-8, mỗi dòng một bản ghi, các trường ngăn cách bằng `;`),
không dùng SQL/cơ sở dữ liệu.

**Trạng thái:** đã hoàn thiện 3 phân hệ SV1 – SV3, 3 chức năng nâng cao và toàn bộ yêu cầu chung.
`dotnet build` hiện đạt **0 Warning, 0 Error**.

## Công nghệ và ràng buộc

- C# Windows Forms, .NET 10 (`net10.0-windows`), mở bằng Visual Studio 2022+.
- Mỗi đối tượng nghiệp vụ là một lớp trong `Models/` có `ToString()`, `TryParse()`, `ToDataLine()`;
  dữ liệu được quản lý trong bộ nhớ bằng `List<T>` trong `DataStore`.
- Đọc/ghi tệp bằng `StreamReader`/`StreamWriter` (`Services/FileHelpers.cs`).
- Cấu hình chung trong `App.config`; tùy chọn người dùng lưu trong Registry `HKCU\Software\BCWF`.

## Cấu trúc thư mục

```text
BC-WinForm/
├── README.md
└── BCWF/
    ├── BCWF.slnx                     # Solution
    └── BCWF/                         # Project WinForms
        ├── BCWF.csproj
        ├── App.config                # DataFolder, MinTC, MaxTC, HocKyHienTai, DiemCanhBao
        ├── Program.cs                # Nạp dữ liệu -> đăng nhập -> mở FrmMain
        ├── Common/
        │   ├── AppConfig.cs          # Đọc <appSettings> trong App.config
        │   └── TuyChonNguoiDung.cs   # Ghi nhớ đăng nhập, vị trí form, màu/font trong Registry
        ├── Models/                   # Khoa, Lop, SinhVien, MonHoc, DangKy, LichHoc, User
        ├── Services/
        │   ├── FileHelpers.cs        # Đọc/ghi .txt và .csv, ghi log.txt
        │   ├── DataStore.cs          # Kho dữ liệu dùng chung + LoadAll/SaveAll
        │   ├── SinhVienService.cs    # SV1
        │   ├── DangKyService.cs      # SV2 + tiên quyết + trùng lịch
        │   ├── DiemService.cs        # SV3 + cảnh báo học vụ
        │   └── BackupService.cs      # Sao lưu / khôi phục tệp .txt và ảnh thẻ
        ├── Forms/
        │   ├── FrmDangNhap.cs        # Đăng nhập, khóa sau 3 lần sai, ghi nhớ đăng nhập
        │   ├── FrmDoiMatKhau.cs      # Đổi mật khẩu, ghi lại users.txt
        │   ├── FrmMain.cs            # Form MDI: menu Hệ thống / Quản lý / Báo cáo / Cửa sổ
        │   ├── FrmTrangChu.cs        # Trang chủ: tài khoản, học kỳ, thống kê, nhắc việc
        │   ├── FrmSinhVien.cs        # SV1 - TreeView Khoa -> Lớp, ListView, ảnh thẻ, nhập CSV
        │   ├── FrmDangKy.cs          # SV2 - quản lý môn học + đăng ký bằng 2 ListBox
        │   ├── FrmNhapDiem.cs        # SV3 - sửa điểm trực tiếp trên ListView, xuất CSV
        │   └── FrmCanhBaoHocVu.cs    # Nâng cao - danh sách cảnh báo học vụ
        └── Data/
            ├── users.txt
            ├── khoa.txt
            ├── lop.txt
            ├── sinhvien.txt
            ├── monhoc.txt
            ├── dangky.txt
            ├── lichhoc.txt           # Bổ sung cho kiểm tra trùng lịch
            └── Images/               # Ảnh thẻ do người dùng thêm khi chạy
```

Thư mục `Data` và các tệp dữ liệu được tự tạo (kèm dòng tiêu đề) nếu chưa có.
Tệp `log.txt` trong `Data` ghi lỗi đọc/ghi tệp và các dòng dữ liệu sai định dạng bị bỏ qua.

## Tài khoản mẫu

| Tên đăng nhập | Mật khẩu | Vai trò | Quyền |
| --- | --- | --- | --- |
| `admin` | `12345` | Admin | Đầy đủ, có menu Báo cáo và Sao lưu/Khôi phục dữ liệu |
| `giaoVu` | `12345` | GiaoVu | Các chức năng nghiệp vụ, không có Báo cáo/Sao lưu |

Nhập sai 3 lần liên tiếp sẽ khóa tài khoản trong phiên làm việc hiện tại.
Đổi mật khẩu bằng link ở form đăng nhập hoặc menu **Hệ thống → Đổi mật khẩu...** sau khi đăng nhập.

## Đối chiếu yêu cầu chung

| # | Yêu cầu | Nơi cài đặt |
| --- | --- | --- |
| 1 | C# WinForms, mỗi đối tượng là class có `ToString()`, quản lý bằng `List<T>`, không SQL | `Models/`, `Services/DataStore.cs` |
| 2 | Tệp `.txt` UTF-8, `;`, `StreamReader`/`StreamWriter`, ≥ 4 tệp trong `Data`, tự tạo tệp, tệp chính ≥ 20 dòng mẫu | `Services/FileHelpers.cs`, `Services/DataStore.cs`, `Data/` (183 sinh viên) |
| 3 | Kiểm tra rỗng/đúng kiểu bằng `TryParse`/không trùng mã, báo lỗi và đưa con trỏ về ô sai | `Forms/FrmSinhVien.cs`, `Forms/FrmDangKy.cs`, `Forms/FrmNhapDiem.cs` |
| 4 | `ListView` (Details) + ≥ 3 control Chương 3 | TreeView, ComboBox, ListBox, PictureBox, ContextMenuStrip trong các form |
| 5 | Form chính MDI + `MenuStrip`, form con không mở trùng, menu Cửa sổ sắp xếp form con | `Forms/FrmMain.cs` |
| 6 | `MessageBox` xác nhận xóa/thoát; `OpenFileDialog`/`SaveFileDialog`; `ColorDialog`/`FontDialog` | `FrmSinhVien`, `FrmNhapDiem`, `FrmCanhBaoHocVu`, `FrmMain` |
| 7 | `try-catch` cho thao tác tệp, bỏ qua dòng sai định dạng, ghi `log.txt` | `FileHelpers`, `DataStore`, các service |
| 8 | `App.config` cho cấu hình chung; Registry lưu ghi nhớ đăng nhập, vị trí/kích thước form, màu/font | `Common/AppConfig.cs`, `Common/TuyChonNguoiDung.cs`, `FrmMain`, `FrmDangNhap` |
| 9 | Đăng nhập từ `users.txt`, ≥ 2 vai trò với menu khác nhau, khóa sau 3 lần sai | `Forms/FrmDangNhap.cs`, `Forms/FrmMain.cs` |
| 10 | Sao lưu/khôi phục thư mục dữ liệu; xuất báo cáo TXT/CSV | `Services/BackupService.cs`, `DiemService.XuatBangDiemCsv`, `FrmCanhBaoHocVu` |

## Chức năng theo Đề 6

### SV1 - Sinh viên & lớp (`FrmSinhVien`, `SinhVienService`)

- Cây Khoa → Lớp (TreeView); chọn khoa/lớp thì ListView hiển thị sinh viên tương ứng.
- Thêm/sửa/xóa sinh viên; MSSV dạng `SV` + số, không trùng, có kiểm tra rỗng/định dạng và đưa con trỏ về ô sai.
- Sửa theo 3 trạng thái Xem – Thêm – Sửa: nút **Hủy** (hoặc `Esc`) khôi phục đúng dữ liệu gốc, Thêm mới dùng biểu mẫu trắng; khi có thay đổi chưa lưu, form hỏi lại trước khi đóng.
- Ảnh thẻ: `OpenFileDialog` + `PictureBox`, ảnh được chép vào `Data/Images`.
- Nhập danh sách sinh viên từ CSV; dòng thiếu cột, sai MSSV hoặc trùng được bỏ qua và ghi vào `log.txt`.

### SV2 - Đăng ký học phần (`FrmDangKy`, `DangKyService`)

- Tab quản lý môn học: thêm/sửa/xóa môn, số tín chỉ, môn tiên quyết.
- Đăng ký bằng cách chuyển môn giữa 2 ListBox (môn chưa đăng ký / môn đã chọn), hỗ trợ nhấp đúp.
- Mở tab là nạp sẵn lớp/sinh viên đầu tiên; đổi lớp, sinh viên hoặc học kỳ sẽ nạp lại danh sách môn và hỏi trước khi bỏ các thay đổi chưa lưu.
- Tổng tín chỉ phải nằm trong `MinTC` – `MaxTC` đọc từ `App.config` (mặc định 12 – 25).
- Chặn đăng ký trùng môn trong cùng học kỳ và chặn đăng ký lại môn đã đạt (tổng kết ≥ 4).

### SV3 - Điểm (`FrmNhapDiem`, `DiemService`)

- Chọn lớp, môn, học kỳ để nạp bảng điểm; đổi môn/học kỳ tự nạp lại bảng điểm tương ứng; sửa trực tiếp điểm QT/GK/CK ngay trên dòng ListView đang chọn.
- Điểm tổng kết = 30% quá trình + 30% giữa kỳ + 40% cuối kỳ; xếp loại A/B/C/D/F.
- Tô đỏ môn rớt (tổng kết < 4), hiển thị ĐTB tích lũy theo tín chỉ.
- Xem bảng điểm cá nhân, xuất bảng điểm lớp và bảng điểm cá nhân ra CSV (`SaveFileDialog`).

### Chức năng nâng cao

- Môn tiên quyết: chưa đạt môn trước thì không cho đăng ký (`DangKyService.KiemTraTienQuyet`).
- Kiểm tra trùng lịch học theo thứ/tiết từ `Data/lichhoc.txt` (`DangKyService.KiemTraTrungLich`).
- Danh sách sinh viên bị cảnh báo học vụ theo ngưỡng `DiemCanhBao` trong `App.config`, xuất CSV.

## Định dạng tệp dữ liệu

| Tệp | Định dạng |
| --- | --- |
| `users.txt` | `TenDangNhap;MatKhau;HoTen;VaiTro` |
| `khoa.txt` | `MaKhoa;TenKhoa` |
| `lop.txt` | `MaLop;TenLop;MaKhoa` |
| `sinhvien.txt` | `MSSV;HoTen;NgaySinh;GioiTinh;MaLop;Anh` |
| `monhoc.txt` | `MaMon;TenMon;SoTC;MaMonTienQuyet` |
| `dangky.txt` | `MSSV;MaMon;HocKy;DiemQT;DiemGK;DiemCK` |
| `lichhoc.txt` | `MaMon;Thu;TietBatDau;SoTiet` |

Ghi chú: so với gợi ý trong đề, có thêm cột `Anh` (tên tệp ảnh thẻ), cột `MaMonTienQuyet`
và tệp `lichhoc.txt` để phục vụ chức năng nâng cao. Ngày sinh dùng `dd/MM/yyyy`;
điểm để trống khi chưa nhập. Dòng tiêu đề của mỗi tệp được bỏ qua khi nạp dữ liệu.

## Chạy chương trình

```bash
dotnet build BCWF/BCWF/BCWF.csproj
dotnet run --project BCWF/BCWF/BCWF.csproj
```

Hoặc mở `BCWF/BCWF.slnx` bằng Visual Studio, chọn project `BCWF` và nhấn F5.

## Ghi chú

- Trang chủ dạng dashboard chỉ hiển thị thông tin cần thiết: tài khoản, học kỳ, các thẻ thống kê
  và nhắc việc; không còn phần lối tắt chức năng hay đường dẫn thư mục dữ liệu.
- Màu nền/phông chữ trong **Hệ Thống → Tùy chỉnh giao diện** được áp dụng cho cửa sổ chính
  và các form đang mở, không chỉ thanh menu.
- Báo cáo Word và slide thuyết trình là sản phẩm nộp kèm, không nằm trong repository này.
- Ảnh thẻ mẫu không được đưa lên Git (thư mục `Data/Images` chỉ giữ `.gitkeep`);
  người dùng chọn ảnh trong form SV1 để thêm ảnh.
