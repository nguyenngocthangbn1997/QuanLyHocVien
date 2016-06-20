﻿// Quản lý Học viên Trung tâm Anh ngữ
// Copyright © 2016, VP2T
// File "frmQuanLyDiem.cs"
// Writing by Nguyễn Lê Hoàng Tuấn (nguyentuanit96@gmail.com)

using System;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;

namespace QuanLyHocVien
{
    public partial class frmQuanLyDiem : Form
    {
        private LopHoc busLopHoc = new LopHoc();
        private BangDiem busBangDiem = new BangDiem();

        public frmQuanLyDiem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Nạp bảng điểm lên giao diện
        /// </summary>
        /// <param name="maHV">Mã học viên</param>
        /// <param name="maLop">Mã lớp</param>
        public void LoadPanelDiem(string maHV, string maLop)
        {
            var d = busBangDiem.SelectDetail(maHV, maLop);
            lblMaLop.Text = d.MaLop;
            lblTenLop.Text = d.TenLop;
            lblKhoa.Text = d.TenKH;
            lblMaHV.Text = d.MaHV;
            lblTenHV.Text = d.TenHV;
            numDiemNghe.Value = d.DiemNghe;
            numDiemNoi.Value = d.DiemNoi;
            numDiemDoc.Value = d.DiemDoc;
            numDiemViet.Value = d.DiemViet;
        }

        /// <summary>
        /// Nạp giao diện xuống bảng điểm
        /// </summary>
        /// <returns></returns>
        public BANGDIEM LoadDiem()
        {
            return new BANGDIEM()
            {
                MaHV = lblMaHV.Text,
                MaLop = lblMaLop.Text,
                DiemNghe = (int)numDiemNghe.Value,
                DiemNoi = (int)numDiemNoi.Value,
                DiemDoc = (int)numDiemDoc.Value,
                DiemViet = (int)numDiemViet.Value,
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHienTatCa_Click(object sender, EventArgs e)
        {
            gridLop.DataSource = busLopHoc.SelectAll();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            gridLop.DataSource = busLopHoc.Select(txtMaLop.Text);
        }

        private void btnDatLai_Click(object sender, EventArgs e)
        {
            txtMaLop.Text = string.Empty;
        }

        private void gridLop_Click(object sender, EventArgs e)
        {
            try
            {
                gridDSHV.DataSource = busBangDiem.SelectDSHV(gridLop.SelectedRows[0].Cells["clmMaLop"].Value.ToString());
            }
            catch { }
        }

        private void gridDSHV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTongCong.Text = string.Format("Tổng cộng: {0} học viên", gridDSHV.Rows.Count);
        }

        private void gridDSHV_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTongCong.Text = string.Format("Tổng cộng: {0} học viên", gridDSHV.Rows.Count);
        }

        private void gridDSHV_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPanelDiem(gridDSHV.SelectedRows[0].Cells["clmMaHV"].Value.ToString(),
                gridLop.SelectedRows[0].Cells["clmMaLop"].Value.ToString());
            }
            catch { }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            gridDSHV_Click(sender, e);
        }

        private void frmQuanLyDiem_Load(object sender, EventArgs e)
        {
            lblMaLop.Text = string.Empty;
            lblTenLop.Text = string.Empty;
            lblKhoa.Text = string.Empty;
            lblMaHV.Text = string.Empty;
            lblTenHV.Text = string.Empty;
            numDiemNghe.Value = 0;
            numDiemNoi.Value = 0;
            numDiemDoc.Value = 0;
            numDiemViet.Value = 0;

            gridDSHV.AutoGenerateColumns = false;
            gridLop.AutoGenerateColumns = false;

            btnHienTatCa_Click(sender, e);
            gridLop_Click(sender, e);
            gridDSHV_Click(sender, e);
        }

        private void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            try
            {
                busBangDiem.Update(LoadDiem());

                MessageBox.Show("Cập nhật bảng điểm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
