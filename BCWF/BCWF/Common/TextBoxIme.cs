using System;
using System.Windows.Forms;

namespace BCWF.Common
{
    /// <summary>
    /// TextBox biết được trạng thái đang soạn văn bản của bộ gõ tiếng Việt (IME).
    /// Khi đang soạn, không được sửa hay kiểm tra nội dung ô nhập
    /// (sửa Text lúc này sẽ huỷ vùng soạn và làm mất ký tự).
    /// </summary>
    public class TextBoxIme : TextBox
    {
        private const int WM_IME_COMPOSITION = 0x0103;
        private const int WM_IME_ENDCOMPOSITION = 0x0104;

        // Các cờ trạng thái vùng soạn (GCS) của Windows.
        private const int GCS_COMPSTR = 0x00000008;
        private const int GCS_COMPREAD = 0x00000004;
        private const int GCS_RESULTSTR = 0x00000010;

        /// <summary>Đang soạn văn bản bằng bộ gõ (IME) hay không.</summary>
        public bool DangComposition { get; private set; }

        /// <summary>Bắt đầu soạn văn bản bằng bộ gõ.</summary>
        public event EventHandler? CompositionStarted;

        /// <summary>Kết thúc soạn văn bản bằng bộ gõ.</summary>
        public event EventHandler? CompositionEnded;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_IME_COMPOSITION)
            {
                int trangThai = (int)(m.LParam.ToInt64() & 0xFFFFFFFF);
                if ((trangThai & (GCS_COMPSTR | GCS_COMPREAD | GCS_RESULTSTR)) != 0 && !DangComposition)
                {
                    // Đánh dấu trước khi xử lý để không sửa Text khi vùng soạn còn đang mở.
                    DangComposition = true;
                    CompositionStarted?.Invoke(this, EventArgs.Empty);
                }

                base.WndProc(ref m);
                return;
            }

            if (m.Msg == WM_IME_ENDCOMPOSITION)
            {
                base.WndProc(ref m);

                if (DangComposition)
                {
                    DangComposition = false;
                    CompositionEnded?.Invoke(this, EventArgs.Empty);
                }

                return;
            }

            base.WndProc(ref m);
        }
    }
}
