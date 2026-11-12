# Đề 6 - Quản lý đăng ký học phần & điểm (WinForms)

Ứng dụng WinForms quản lý sinh viên, đăng ký học phần và điểm, dữ liệu lưu trong các tệp `.txt`
phân tách bằng dấu `;`. Repository hiện **chỉ dựng khung kiến trúc**: model, tầng dữ liệu, service
và giao diện đã có; phần xử lý nghiệp vụ còn để `TODO` cho từng thành viên.

## Cấu trúc thư mục

```
BC-WinForm/
├── BCWF.slnx                     # Solution
└── BCWF/                         # Project WinForms (net10.0-windows)
    ├── BCWF.csproj
    ├── App.config                # DataFolder, MinTC, MaxTC, HocKyHienTai, DiemCanhBao
    ├── Program.cs                # Nạp dữ liệu -> đăng nhập -> mở FrmMain
    ├── Common/
    │   └── AppConfig.cs          # Đọc <appSettings> trong App.config
    ├── Models/                   # Lớp dữ liệu + TryParse/ToDataLine
    │   ├── Khoa.cs
    │   ├── Lop.cs
    │   ├── SinhVien.cs
    │   ├── MonHoc.cs
    │   ├── DangKy.cs             # Đăng ký học phần + 3 đầu điểm
    │   ├── LichHoc.cs            # Phục vụ kiểm tra trùng lịch (nâng cao)
    │   └── User.cs               # Tài khoản đăng nhập
    ├── Services/
    │   ├── FileHelpers.cs        # Đọc/ghi tệp văn bản, CSV
    │   ├── DataStore.cs          # Kho dữ liệu dùng chung + LoadAll/SaveAll
    │   ├── SinhVienService.cs    # SV1
    │   ├── DangKyService.cs      # SV2
    │   └── DiemService.cs        # SV3 + cảnh báo học vụ
    ├── Forms/
    │   ├── FrmMain.cs            # Menu: Hệ Thống, Quản Lý (SV1/SV2/SV3), Báo Cáo
    │   ├── FrmDangNhap.cs        # Đăng nhập theo danh sách trong users.txt
    │   ├── FrmSinhVien.cs        # SV1 - TreeView Khoa->Lớp, ListView SV, ảnh thẻ, CSV
    │   ├── FrmDangKy.cs          # SV2 - Tab quản lý môn học + tab đăng ký 2 ListBox
    │   ├── FrmNhapDiem.cs        # SV3 - ListView sửa điểm trực tiếp, xuất CSV
    │   └── FrmCanhBaoHocVu.cs    # Nâng cao - danh sách cảnh báo học vụ
    └── Data/
        ├── users.txt
        ├── khoa.txt
        ├── lop.txt
        ├── sinhvien.txt
        ├── monhoc.txt
        ├── dangky.txt
        ├── lichhoc.txt           # Bổ sung cho phần nâng cao
        └── Images/               # Ảnh thẻ sinh viên
```

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
(môn tiên quyết) và tệp `lichhoc.txt` để phục vụ các chức năng nâng cao. Ngày sinh dùng
`dd/MM/yyyy`, điểm để trống khi chưa nhập.

## Đối chiếu yêu cầu - nơi cài đặt

| Yêu cầu | Nơi cài đặt |
| --- | --- |
| SV1: Cây Khoa -> Lớp, ListView sinh viên của lớp, thêm/sửa/xóa | `Forms/FrmSinhVien.cs` |
| SV1: MSSV không trùng, ảnh thẻ, nhập danh sách từ CSV | `Services/SinhVienService.cs` |
| SV2: quản lý môn học | `Forms/FrmDangKy.cs` (tab Quản lý môn học) |
| SV2: chuyển môn giữa 2 ListBox, tổng tín chỉ MinTC-MaxTC, chống trùng | `Forms/FrmDangKy.cs`, `Services/DangKyService.cs` |
| SV3: nhập điểm theo lớp & môn, sửa trực tiếp trên ListView | `Forms/FrmNhapDiem.cs` |
| SV3: điểm tổng kết 30/30/40, xếp loại, tô đỏ môn rớt, ĐTB tích lũy, xuất CSV | `Services/DiemService.cs`, `Forms/FrmNhapDiem.cs` |
| Nâng cao: môn tiên quyết, trùng lịch | `Services/DangKyService.cs`, `Data/lichhoc.txt` |
| Nâng cao: danh sách sinh viên bị cảnh báo học vụ | `Forms/FrmCanhBaoHocVu.cs`, `Services/DiemService.cs` |
| Giới hạn tín chỉ | `App.config` (`MinTC`, `MaxTC`) đọc qua `Common/AppConfig.cs` |

## Quy ước

- Đọc/ghi dữ liệu chỉ qua `DataStore.Instance`; sau khi sửa danh sách thì gọi `LuuSinhVien()`,
  `LuuMonHoc()`, `LuuDangKy()`... tương ứng.
- Service trả về `bool`/`out string thongBao` để form hiển thị lỗi nghiệp vụ.
- `FrmMain` dùng một vùng nội dung duy nhất (`panelNoiDung`): mỗi lúc chỉ hiển thị một chức năng,
  các form được giữ lại để chuyển qua lại mà không mở thêm cửa sổ và không mất trạng thái.
- Khi hiển thị lại một form đã mở, `FrmMain` gọi `ILamMoiKhiHienThi.LamMoiKhiHienThi()`
  để form nạp lại dữ liệu mới nhất.
- Các chỗ cần làm đều được đánh dấu `TODO (SV1|SV2|SV3|nâng cao)`.

## Chạy

```bash
dotnet build BCWF/BCWF/BCWF.csproj
dotnet run --project BCWF/BCWF/BCWF.csproj
```

Tài khoản mẫu trong `Data/users.txt`: `admin` / `12345` hoặc `giaoVu` / `12345`.
Thư mục `Data/Images` có sẵn ba ảnh thẻ mẫu `SV001.png`, `SV002.png`, `SV004.png`.
