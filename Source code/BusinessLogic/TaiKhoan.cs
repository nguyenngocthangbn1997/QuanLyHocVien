﻿// Quản lý Học viên Trung tâm Anh ngữ
// Copyright © 2016, VP2T
// File "TaiKhoan.cs"
// Writing by Nguyễn Lê Hoàng Tuấn (nguyentuanit96@gmail.com)

using System.Linq;
using DataAccess;
using static BusinessLogic.GlobalSettings;

namespace BusinessLogic
{
    public class TaiKhoan
    {
        /// <summary>
        /// Chọn một tài khoản
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập</param>
        /// <returns></returns>
        public TAIKHOAN Select(string tenDangNhap)
        {
            return (from p in Database.TAIKHOANs
                    where p.TenDangNhap == tenDangNhap
                    select p).Single();
        }

        /// <summary>
        /// Lấy danh sách tài khoản
        /// </summary>
        /// <param name="tenDangNhap">Theo tên đăng nhập</param>
        /// <param name="loaiTK">Theo loại tài khoản (0: Nhân viên, 1: Học viên, 2: Giảng viên)</param>
        /// <returns></returns>
        public object SelectAll(string tenDangNhap, int? loaiTK)
        {
            switch (loaiTK)
            {
                case null:
                    return (from p in Database.TAIKHOANs
                            where (tenDangNhap == null ? true : p.TenDangNhap.Contains(tenDangNhap))
                            select p).ToList();
                case 0:
                    return (from p in Database.NHANVIENs
                            where (tenDangNhap == null ? true : p.TenDangNhap.Contains(tenDangNhap))
                            select new
                            {
                                TenDangNhap = p.TenDangNhap,
                                MatKhau = p.TAIKHOAN.MatKhau
                            }).ToList();
                case 1:
                    return (from p in Database.HOCVIENs
                            where p.TenDangNhap != null &&
                                  (tenDangNhap == null ? true : p.TenDangNhap.Contains(tenDangNhap))
                            select new
                            {
                                TenDangNhap = p.TenDangNhap,
                                MatKhau = p.TAIKHOAN.MatKhau
                            }).ToList();
                case 2:
                    return (from p in Database.GIANGVIENs
                            where (tenDangNhap == null ? true : p.TenDangNhap.Contains(tenDangNhap))
                            select new
                            {
                                TenDangNhap = p.TenDangNhap,
                                MatKhau = p.TAIKHOAN.MatKhau
                            }).ToList();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="tenDangNhap"></param>
        public void Delete(string tenDangNhap)
        {
            var temp = (from p in Database.TAIKHOANs
                        where p.TenDangNhap == tenDangNhap
                        select p).Single();

            Database.TAIKHOANs.DeleteOnSubmit(temp);
            Database.SubmitChanges();
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="tk"></param>
        public void Update(TAIKHOAN tk)
        {
            var temp = (from p in Database.TAIKHOANs
                        where p.TenDangNhap == tk.TenDangNhap
                        select p).Single();

            temp.MatKhau = tk.MatKhau;
            Database.SubmitChanges();
        }

        /// <summary>
        /// Trả về tên người dùng của tên đăng nhập
        /// </summary>
        /// <param name="tk"></param>
        public string FullUserName(TAIKHOAN tk)
        {
            var a = (from p in Database.NHANVIENs
                     where p.TenDangNhap == tk.TenDangNhap
                     select p).SingleOrDefault();
            if (a != null)
                return a.TenNV;

            var b = (from p in Database.HOCVIENs
                     where p.TenDangNhap == tk.TenDangNhap
                     select p).SingleOrDefault();
            if (b != null)
                return b.TenHV;

            var c = (from p in Database.GIANGVIENs
                     where p.TenDangNhap == tk.TenDangNhap
                     select p).SingleOrDefault();
            if (c != null)
                return c.TenGV;
            return null;
        }
    }
}
