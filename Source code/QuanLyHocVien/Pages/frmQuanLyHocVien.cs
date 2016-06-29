﻿// Quản lý Học viên Trung tâm Anh ngữ
// Copyright © 2016, VP2T
// File "frmQuanLyHocVien.cs"
// Writing by Nguyễn Lê Hoàng Tuấn (nguyentuanit96@gmail.com)

using System;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;
using QuanLyHocVien.Popups;

namespace QuanLyHocVien.Pages
{
    public partial class frmQuanLyHocVien : Form
    {
        public frmQuanLyHocVien()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            GlobalPages.QuanLyHocVien = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmHocVienEdit frm = new frmHocVienEdit(null);
            frm.ShowDialog();
            btnXemTatCa_Click(sender, e);
        }

        private void chkMaHV_CheckedChanged(object sender, EventArgs e)
        {
            txtMaHV.Enabled = chkMaHV.Checked;
        }

        private void chkTenHV_CheckedChanged(object sender, EventArgs e)
        {
            txtTenHV.Enabled = chkTenHV.Checked;
        }

        private void chkGioiTinh_CheckedChanged(object sender, EventArgs e)
        {
            cboGioiTinh.Enabled = chkGioiTinh.Checked;
        }

        private void chkNgayTiepNhan_CheckedChanged(object sender, EventArgs e)
        {
            dateTuNgay.Enabled = dateDenNgay.Enabled = chkNgayTiepNhan.Checked;
        }

        private void btnDatLai_Click(object sender, EventArgs e)
        {
            chkMaHV.Checked = true;
            chkTenHV.Checked = chkGioiTinh.Checked = chkNgayTiepNhan.Checked = false;

            txtMaHV.Text = txtTenHV.Text = string.Empty;
            btnXemTatCa_Click(sender, e);
        }

        private void frmQuanLyHocVien_Load(object sender, EventArgs e)
        {
            cboLoaiHV.DataSource = LoaiHV.SelectAll();
            cboLoaiHV.DisplayMember = "TenLoaiHV";
            cboLoaiHV.ValueMember = "MaLoaiHV";

            btnXemTatCa_Click(sender, e);
        }

        private void gridDSHV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTongCong.Text = string.Format("Tổng cộng: {0} học viên", gridDSHV.Rows.Count);
        }

        private void gridDSHV_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTongCong.Text = string.Format("Tổng cộng: {0} học viên", gridDSHV.Rows.Count);
        }

        private void btnXemTatCa_Click(object sender, EventArgs e)
        {
            gridDSHV.AutoGenerateColumns = false;
            gridDSHV.DataSource = HocVien.SelectAll(cboLoaiHV.SelectedValue.ToString());
        }

        private void cboLoaiHV_SelectedValueChanged(object sender, EventArgs e)
        {
            btnXemTatCa_Click(sender, e);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            gridDSHV.DataSource = HocVien.SelectAll(chkMaHV.Checked ? txtMaHV.Text : null,
                chkTenHV.Checked ? txtTenHV.Text : null, 
                chkGioiTinh.Checked ? cboGioiTinh.Text : null,
                chkNgayTiepNhan.Checked ? (DateTime?)dateTuNgay.Value : null,
                chkNgayTiepNhan.Checked ? (DateTime?)dateDenNgay.Value : null,
                cboLoaiHV.SelectedValue.ToString());
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            frmHocVienEdit frm = new frmHocVienEdit(HocVien.Select(gridDSHV.SelectedRows[0].Cells["clmMaHV"].Value.ToString()));
            frm.ShowDialog();
            btnXemTatCa_Click(sender, e);
        }

        private void gridDSHV_DoubleClick(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HocVien.Delete(gridDSHV.SelectedRows[0].Cells["clmMaHV"].Value.ToString());

                    MessageBox.Show("Xóa học viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnXemTatCa_Click(sender, e);
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
