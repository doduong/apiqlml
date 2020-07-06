using APITest.Extensions;
using APITest.Helper;
using APITest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace APITest.Controllers
{
    public class KhachHangController : ApiController
    {
        //-----------DS chi nhanh------------
        [HttpGet]
        [Route("api/khachhang/getchinhanh")]
        public JsonResult<List<ChiNhanh>> GetChiNhanh()
        {
            string ms_bd = HttpContext.Current.Request.QueryString.Get("ms_bd");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select ms_chinhanh, ten_chinhanh, ghi_chu from chi_nhanh");
            List<ChiNhanh> lsp = DataTableToListExtensions.DataTableToList<ChiNhanh>(dt);
            return Json(lsp);
        }

        //-----------DS Tổ Quản Lý------------
        [HttpGet]
        [Route("api/khachhang/gettoquanly")]
        public JsonResult<List<ToQuanLy>> GetToQuanLy()
        {
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select distinct(ms_tql), ten_tql from locdulieuthanhtra where ms_userthanhtra = " + ms_userthanhtra);
            List<ToQuanLy> lsp = DataTableToListExtensions.DataTableToList<ToQuanLy>(dt);
            return Json(lsp);
        }
        //----------DS Tuyến --------

        [HttpGet]
        [Route("api/khachhang/gettuyendoc")]
        public JsonResult<List<Tuyen>> GetTuyenDoc()
        {
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select distinct(ms_tuyen) from locdulieuthanhtra where ms_userthanhtra = " + ms_userthanhtra);
            List<Tuyen> lsp = DataTableToListExtensions.DataTableToList<Tuyen>(dt);
            return Json(lsp);
        }

        [HttpGet]
        [Route("api/khachhang/gettdktt")]
        public JsonResult<List<DieuKienThanhTra>> GetDieuKienTT()
        {
            //string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select ms_dk_ks, ten_dk_ks from dk_kiemsoat");
            List<DieuKienThanhTra> lsp = DataTableToListExtensions.DataTableToList<DieuKienThanhTra>(dt);
            return Json(lsp);
        }

        //----------THOI_KY--------------
        [HttpGet]
        [Route("api/khachhang/getthoiky")]
        public JsonResult<DataTable> GetThoiKy()
        {
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"select ms_tk, thoi_ky from thoi_ky where ms_tk = (select max(ms_tk) from thoi_ky)");
            return Json(dt);
        }

        //----------CHECK LOGIN--------------
        [HttpGet]
        [Route("api/khachhang/checklogin")]
        public JsonResult<DataTable> CheckLogin()
        {
            string ten_userthanhtra = HttpContext.Current.Request.QueryString.Get("ten_userthanhtra");
            string mk_userthanhtra = HttpContext.Current.Request.QueryString.Get("mk_userthanhtra");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"select ms_userthanhtra, ten_userthanhtra , mk_userthanhtra from userthanhtra where ten_userthanhtra = '" + ten_userthanhtra + "' and mk_userthanhtra = '" + mk_userthanhtra + "' and tinhtranguser = 1 " );
            return Json(dt);
        }

        //----------DSKH cần kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/gettoanbobanglocdulieu")]
        public JsonResult<DataTable> GetListCustomerChuaKiemSoat()
        {
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            //string ms_tuyen = HttpContext.Current.Request.QueryString.Get("ms_tuyen");

            SqlHelper _db = new SqlHelper();
            string sql = " select ms_locdulieu,ms_dk_ks ,ten_dk_ks ,ms_chinhanh ,ten_chinhanh ,ms_tql ,ten_tql ,ms_tt_dd ,mo_ta_tt_dd ,ms_ttrang ,mo_ta_ttrang ,ms_tuyen ," +
                "mo_ta_tuyen ,ms_cky ,ms_dh_mn ,ms_tk ,stt_lo_trinh ,so_seri ,ms_mnoi ,ms_kh ,nguoi_thue ,diachi ,dien_thoai ,so_ho , ms_md_chinh ,gia_chinh ," +
                "ms_gia_vuot ,gia_vuot ,ngay_thuc_te ,ngay_doc_cu ,ngay_doc_moi ,chi_so_cu , chi_so_moi, STT1 ,STT2 ,STT3 ,STT4 ,ms_userthanhtra ," +
                "ttkiemsoat , url_image ,ghi_chu, mucdichsudung, ngay_loc from locdulieuthanhtra where ms_tql = " + ms_tql + " and ms_userthanhtra = " + ms_userthanhtra +
                         " order by ms_tuyen , stt_lo_trinh ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }

        //----------DSKH cần kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/getdskhtheotql")]
        public JsonResult<DataTable> GetListCustomer()
        {
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            //string ms_tuyen = HttpContext.Current.Request.QueryString.Get("ms_tuyen");

            SqlHelper _db = new SqlHelper();
            string sql = " select ms_locdulieu,ms_dk_ks ,ten_dk_ks ,ms_chinhanh ,ten_chinhanh ,ms_tql ,ten_tql ,ms_tt_dd ,mo_ta_tt_dd ,ms_ttrang ,mo_ta_ttrang ,ms_tuyen ," +
                "mo_ta_tuyen ,ms_cky ,ms_dh_mn ,ms_tk ,stt_lo_trinh ,so_seri ,ms_mnoi ,ms_kh ,nguoi_thue ,diachi ,dien_thoai ,so_ho , ms_md_chinh ,gia_chinh ," +
                "ms_gia_vuot ,gia_vuot ,ngay_thuc_te ,ngay_doc_cu ,ngay_doc_moi ,chi_so_cu , chi_so_moi, STT1 ,STT2 ,STT3 ,STT4 ,ms_userthanhtra ," +
                "ttkiemsoat , url_image ,ghi_chu, mucdichsudung, ngay_loc from locdulieuthanhtra where ms_tql = " + ms_tql + " and ttkiemsoat = 0 and ms_userthanhtra = " + ms_userthanhtra +
                         " order by ms_tuyen , stt_lo_trinh ";        
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }

        //----------DSKH đã kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/getdskhdakstheotql")]
        public JsonResult<DataTable> GetListCustomerControled()
        {
            //string ms_chinhanh = HttpContext.Current.Request.QueryString.Get("ms_chinhanh");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            //string ms_tuyen = HttpContext.Current.Request.QueryString.Get("ms_tuyen");

            SqlHelper _db = new SqlHelper();
            string sql = " select ms_locdulieu,ms_dk_ks ,ten_dk_ks ,ms_chinhanh ,ten_chinhanh ,ms_tql ,ten_tql ,ms_tt_dd ,mo_ta_tt_dd ,ms_ttrang ,mo_ta_ttrang ,ms_tuyen ," +
                "mo_ta_tuyen ,ms_cky ,ms_dh_mn ,ms_tk ,stt_lo_trinh ,so_seri ,ms_mnoi ,ms_kh ,nguoi_thue ,diachi ,dien_thoai ,so_ho , ms_md_chinh ,gia_chinh ," +
                "ms_gia_vuot ,gia_vuot ,ngay_thuc_te ,ngay_doc_cu ,ngay_doc_moi ,chi_so_cu , chi_so_moi, STT1 ,STT2 ,STT3 ,STT4 ,ms_userthanhtra ," +
                "ttkiemsoat , url_image ,ghi_chu from locdulieuthanhtra where ms_tql = " + ms_tql + " and ttkiemsoat = 1 " +
                         " order by ms_tuyen , stt_lo_trinh ";

            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }


        [HttpGet]
        [Route("api/khachhang/getlichsudongho")]
        public JsonResult<DataTable> GetLichSuDongHo()
        {
            string ms_mnoi = HttpContext.Current.Request.QueryString.Get("ms_mnoi");
            SqlHelper _db = new SqlHelper();
            string sql = " select * from lich_su_dh ls where ms_mnoi = " + ms_mnoi + 
            " and ls.ms_ls_dh = (select max(lsbd.ms_ls_dh) from lich_su_dh lsbd where ms_mnoi = "+ms_mnoi+" ) ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<LichSuDongHo> lsp = DataTableToListExtensions.DataTableToList<LichSuDongHo>(dt);
            return Json(dt);
        }


        //----------DSKH cần kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/timkiemdskhtheotql")]
        public JsonResult<DataTable> GetListCustomerSearch()
        {
            //string ms_chinhanh = HttpContext.Current.Request.QueryString.Get("ms_chinhanh");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            //string ms_tuyen = HttpContext.Current.Request.QueryString.Get("ms_tuyen");

            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("ms_mnoi"))
            {
                strwhere = " and ms_mnoi like '%" + keyWord + "%'";
            }
            else if (conditionField.Equals("nguoi_thue"))
            {
                strwhere = " and nguoi_thue like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("so_seri"))
            {
                strwhere = " and so_seri like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and diachi like N'%" + keyWord + "%'";
            }

            SqlHelper _db = new SqlHelper();
            string sql = " select ms_locdulieu,ms_dk_ks ,ten_dk_ks ,ms_chinhanh ,ten_chinhanh ,ms_tql ,ten_tql ,ms_tt_dd ,mo_ta_tt_dd ,ms_ttrang ,mo_ta_ttrang ,ms_tuyen ," +
                "mo_ta_tuyen ,ms_cky ,ms_dh_mn ,ms_tk ,stt_lo_trinh ,so_seri ,ms_mnoi ,ms_kh ,nguoi_thue ,diachi ,dien_thoai ,so_ho , ms_md_chinh ,gia_chinh ," +
                "ms_gia_vuot ,gia_vuot ,ngay_thuc_te ,ngay_doc_cu ,ngay_doc_moi ,chi_so_cu , chi_so_moi, STT1 ,STT2 ,STT3 ,STT4 ,ms_userthanhtra ," +
                "ttkiemsoat , url_image ,ghi_chu, mucdichsudung, ngay_loc from locdulieuthanhtra where ms_tql = " + ms_tql + " and ttkiemsoat = 0 " + strwhere +
                         " order by ms_tuyen , stt_lo_trinh ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }

        
        //----------DSKH đã kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/getkhachhangkiemsoat")]
        public JsonResult<DataTable> GetKhachHangKiemSoat()
        {
            string ms_mnoi = HttpContext.Current.Request.QueryString.Get("ms_mnoi");
            //string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            string ms_tk = HttpContext.Current.Request.QueryString.Get("ms_tk");
            string sqlwhere = " where ms_mnoi = " + ms_mnoi
                            + " and ms_userthanhtra = " + ms_userthanhtra
                            + " and ms_tk = " + ms_tk;

            SqlHelper _db = new SqlHelper();
            string sql = " select * from kiem_soat " + sqlwhere;
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }

        //----------DSKH cần kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/timkiemdskhdakstheotql")]
        public JsonResult<DataTable> GetListCustomerControledSearch()
        {
            //string ms_chinhanh = HttpContext.Current.Request.QueryString.Get("ms_chinhanh");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            string ms_tk = HttpContext.Current.Request.QueryString.Get("ms_tk");
            int ms_tk1 = Int32.Parse(ms_tk);

            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("ms_mnoi"))
            {
                strwhere = " and ms_mnoi like '%" + keyWord + "%'";
            }
            else if (conditionField.Equals("nguoi_thue"))
            {
                strwhere = " and nguoi_thue like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("so_seri"))
            {
                strwhere = " and so_seri like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and diachi like N'%" + keyWord + "%'";
            }

            SqlHelper _db = new SqlHelper();
            string sql = " WITH La AS " +
                     "  (SELECT ms_dh_mn, dhn.ms_tk, dhn.ms_mnoi, dhn.ngay_doc_moi, " +
                     "          dhn.ngay_doc_cu, dhn.chi_so_cu, dhn.chi_so_moi, so_tthu AS STT4, url_image,  " +
                      "         ghi_chu, ms_ttrang, ms_dh, ks.ms_dk_ks, dkks.ten_dk_ks  " +
                      "  FROM dh_noi dhn inner join kiem_soat ks on dhn.ms_mnoi = ks.ms_mnoi  " +
                     "   inner join dk_kiemsoat dkks on ks.ms_dk_ks = dkks.ms_dk_ks " +
                     "   where ks.ms_userthanhtra = " + ms_userthanhtra + " and ks.ms_tk = "+ms_tk1+" ), " +
                     "     L0 AS " +
                      " (SELECT cn.ms_chinhanh, cn.ten_chinhanh, tql.ms_tql, tql.ten_tql, " +
                     "          td.ms_tuyen, td.mo_ta_tuyen, ttdd.mo_ta AS mo_ta_tt_dd, ttdd.ms_tt_dd, " +
                     "          ttddh.mo_ta AS mo_ta_ttrang, ttddh.ms_ttrang, td.ms_cky, " +
                      "         La.ms_dh_mn, La.ms_tk, mn.stt_lo_trinh, dh.so_seri, " +
                      "         La.ms_mnoi, kh.ms_kh, mn.nguoi_thue, ('' + mn.dia_chi_mnoi + ', ' + duong.ten_duong) AS diachi, " +
                      "         kh.dien_thoai, mn.so_ho, mdsd.ms_md_chinh, dhd.mo_ta_dong AS gia_chinh, mdsd.ms_gia_vuot, " +
                       "        dhd1.mo_ta_dong AS gia_vuot, ckbd.ngay_thuc_te, La.ngay_doc_moi, " +
                      "         La.ngay_doc_cu, La.chi_so_cu, La.chi_so_moi, La.STT4, " +
                     "          La.url_image, La.ghi_chu, La.ms_dk_ks, La.ten_dk_ks " +
                     "   FROM La " +
                     "   LEFT JOIN moi_noi AS mn ON La.ms_mnoi = mn.ms_mnoi " +
                     "   LEFT JOIN khach_hang AS kh ON mn.ms_kh = kh.ms_kh " +
                      "  LEFT JOIN tt_doc_dh AS ttddh ON La.ms_ttrang = ttddh.ms_ttrang " +
                     "   LEFT JOIN ttrang_dd AS ttdd ON mn.ms_tt_dd = ttdd.ms_tt_dd " +
                     "   LEFT JOIN muc_dich_sd AS mdsd ON mn.ms_mnoi = mdsd.ms_mnoi " +
                    "    LEFT JOIN dong_hdon AS dhd ON mdsd.ms_md_chinh = dhd.ms_dong " +
                     "   LEFT JOIN dong_hdon AS dhd1 ON mdsd.ms_gia_vuot = dhd1.ms_dong " +
                     "   LEFT JOIN tuyen_doc AS td ON mn.ms_tuyen = td.ms_tuyen " +
                     "   LEFT JOIN cky_bien_doc AS ckbd ON td.ms_cky = ckbd.ms_cky " +
                     "   LEFT JOIN to_quan_ly AS tql ON td.ms_tql = tql.ms_tql " +
                     "   LEFT JOIN chi_nhanh AS cn ON tql.ms_chinhanh = cn.ms_chinhanh " +
                    "    LEFT JOIN duong ON mn.ms_duong = duong.ms_duong " +
                     "   LEFT JOIN dong_ho AS dh ON La.ms_dh = dh.ms_dh), " +
                     "     L1 AS " +
                     "  (SELECT L0.*, " +
                     "          hd.so_tieu_thu AS STT3  " +
                     "   FROM L0  " +
                     "   LEFT JOIN  " +
                     "    (SELECT * " +
                     "      FROM hoa_don  " +
                     "      WHERE hoa_don.ms_tk = " + (ms_tk1 - 1) + ") AS hd ON L0.ms_mnoi = hd.ms_mnoi),  " +
                     "     L2 AS " +
                     "  (SELECT L1.*, " +
                     "          hd.so_tieu_thu AS STT2 " +
                     "   FROM L1 " +
                      "  LEFT JOIN " +
                      "    (SELECT * " +
                     "      FROM hoa_don " +
                     "      WHERE hoa_don.ms_tk = " + (ms_tk1 - 2) + ") AS hd ON L1.ms_mnoi = hd.ms_mnoi), " +
                     "     L3 AS " +
                     "  (SELECT L2.*, " +
                      "         hd.so_tieu_thu AS STT1 " +
                      "  FROM L2 " +
                     "   LEFT JOIN " +
                     "     (SELECT * " +
                      "     FROM hoa_don " +
                      "     WHERE hoa_don.ms_tk = " + (ms_tk1 - 3) + ") AS hd ON L2.ms_mnoi = hd.ms_mnoi) " +
                    " SELECT ms_chinhanh, ten_chinhanh, ms_tql, ten_tql, ms_tuyen, " +
                     "       mo_ta_tuyen, ms_tt_dd, mo_ta_tt_dd, ms_ttrang, mo_ta_ttrang, ms_cky, ms_dh_mn, " +
                     "       ms_tk, stt_lo_trinh, so_seri, ms_mnoi, ms_kh, nguoi_thue, " +
                     "       diachi, dien_thoai, so_ho, ms_md_chinh, gia_chinh, ms_gia_vuot, " +
                    "        gia_vuot, ngay_thuc_te, ngay_doc_cu, ngay_doc_moi, chi_so_cu, " +
                    "        chi_so_moi, STT1, STT2, STT3, STT4, url_image, ghi_chu, ms_dk_ks, ten_dk_ks  " +
                    "        FROM L3 " +
                    " WHERE ms_tql = " + ms_tql + strwhere + " ORDER BY ms_tuyen, stt_lo_trinh ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }



        //----------DSKH cần kiểm soát theo TQL ------
        [HttpGet]
        [Route("api/khachhang/getquetmavach")]
        public JsonResult<DataTable> GetQuetMaVach()
        {
            //string ms_chinhanh = HttpContext.Current.Request.QueryString.Get("ms_chinhanh");
            string ms_mnoi = HttpContext.Current.Request.QueryString.Get("ms_mnoi");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            string ms_tk = HttpContext.Current.Request.QueryString.Get("ms_tk");
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            int ms_tk1 = Int32.Parse(ms_tk);

            SqlHelper _db = new SqlHelper();
            string sql = " With La AS " +
                "(select ms_dh_mn,ms_tk,ms_mnoi, dhn.ngay_doc_moi, dhn.ngay_doc_cu,chi_so_cu,chi_so_moi," +
                "so_tthu as STT4,url_image,ghi_chu,ms_ttrang,ms_dh from dh_noi dhn " + 
                " where ms_mnoi= '"+ms_mnoi+ "' and ms_tk = " + ms_tk1 +" ), " +
                " L0 AS " +
                "(select cn.ms_chinhanh,cn.ten_chinhanh,tql.ms_tql,tql.ten_tql,td.ms_tuyen," +
                "td.mo_ta_tuyen,ttdd.mo_ta as mo_ta_tt_dd, ttdd.ms_tt_dd," +
                "ttddh.mo_ta as mo_ta_ttrang,ttddh.ms_ttrang,td.ms_cky,La.ms_dh_mn," +
                "La.ms_tk,mn.stt_lo_trinh, dh.so_seri,La.ms_mnoi,kh.ms_kh,mn.nguoi_thue," +
                "(''+ mn.dia_chi_mnoi + ', '+ duong.ten_duong) as diachi, kh.dien_thoai," +
                "mn.so_ho,mdsd.ms_md_chinh,dhd.mo_ta_dong as gia_chinh,mdsd.ms_gia_vuot, " +
                "dhd1.mo_ta_dong as gia_vuot,ckbd.ngay_thuc_te,La.ngay_doc_moi,La.ngay_doc_cu," +
                " La.chi_so_cu,La.chi_so_moi,La.STT4,La.url_image," +
                "La.ghi_chu from La left join moi_noi as mn on La.ms_mnoi= mn.ms_mnoi " +
                "left join khach_hang as kh on mn.ms_kh = kh.ms_kh left join tt_doc_dh as ttddh " +
                "on La.ms_ttrang = ttddh.ms_ttrang left join ttrang_dd as ttdd on mn.ms_tt_dd = ttdd.ms_tt_dd" +
                " left join muc_dich_sd as mdsd on mn.ms_mnoi = mdsd.ms_mnoi left join dong_hdon as dhd " +
                "on mdsd.ms_md_chinh = dhd.ms_dong left join dong_hdon as dhd1 on mdsd.ms_gia_vuot = dhd1.ms_dong " +
                "left join tuyen_doc as td on mn.ms_tuyen = td.ms_tuyen left join cky_bien_doc as ckbd on td.ms_cky = ckbd.ms_cky" +
                " left join to_quan_ly as tql on td.ms_tql = tql.ms_tql left join chi_nhanh as cn on tql.ms_chinhanh = cn.ms_chinhanh " +
                "left join duong on mn.ms_duong = duong.ms_duong left join dong_ho as dh on La.ms_dh = dh.ms_dh " +
                "where  not exists(select * from locdulieuthanhtra where (La.ms_tk = locdulieuthanhtra.ms_tk and La.ms_mnoi = locdulieuthanhtra.ms_mnoi)" +
                " and locdulieuthanhtra.ms_userthanhtra = "+ms_userthanhtra+")), L1 AS (select L0.*, hd.so_tieu_thu as STT3 from L0 " +
                "left join (select * from hoa_don where hoa_don.ms_tk = "+ (ms_tk1 -1) + " ) as hd on L0.ms_mnoi=hd.ms_mnoi), L2 AS (select L1.*," +
                " hd.so_tieu_thu as STT2 from L1 left join (select * from hoa_don where hoa_don.ms_tk = "+(ms_tk1 -2) +" ) as hd on L1.ms_mnoi=hd.ms_mnoi)," +
                " L3 AS (select L2.*, hd.so_tieu_thu as STT1 from L2 left join (select * from hoa_don where hoa_don.ms_tk = "+(ms_tk1 -3)+" ) " +
                "as hd on L2.ms_mnoi=hd.ms_mnoi) select ms_chinhanh,ten_chinhanh,ms_tql,ten_tql,ms_tuyen,mo_ta_tuyen,ms_tt_dd," +
                "mo_ta_tt_dd,ms_ttrang,mo_ta_ttrang, ms_cky,ms_dh_mn,ms_tk,stt_lo_trinh,so_seri,ms_mnoi,ms_kh,nguoi_thue," +
                "diachi,dien_thoai,so_ho, ms_md_chinh,gia_chinh,ms_gia_vuot,gia_vuot, ngay_thuc_te,ngay_doc_cu,ngay_doc_moi," +
                "chi_so_cu,chi_so_moi,STT1,STT2,STT3,STT4,url_image,ghi_chu from L3 where ms_tql = "+ms_tql+" order by ms_tuyen,stt_lo_trinh  ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }

        //----------DSKH bất thường ------
        [HttpGet]
        [Route("api/khachhang/getksbatthuong")]
        public JsonResult<DataTable> GetDanhSachBatThuong()
        {
            //string ms_chinhanh = HttpContext.Current.Request.QueryString.Get("ms_chinhanh");
            //string ms_mnoi = HttpContext.Current.Request.QueryString.Get("ms_mnoi");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            string ms_tk = HttpContext.Current.Request.QueryString.Get("ms_tk");
            string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            int ms_tk1 = Int32.Parse(ms_tk);

            SqlHelper _db = new SqlHelper();
            string sql = " WITH La AS " +
                         "  (SELECT ms_dh_mn, dhn.ms_tk, ks.ms_mnoi, dhn.ngay_doc_moi, " +
                         "        dhn.ngay_doc_cu, dhn.chi_so_cu, dhn.chi_so_moi, so_tthu AS STT4, url_image,  " +
                          "         ghi_chu, ms_ttrang, ms_dh, ks.ms_dk_ks, dkks.ten_dk_ks  " +
                          "  FROM dh_noi dhn right join kiem_soat ks on dhn.ms_mnoi = ks.ms_mnoi  " +
                         "   inner join dk_kiemsoat dkks on ks.ms_dk_ks = dkks.ms_dk_ks " +
                         "   where ks.ms_userthanhtra = "+ms_userthanhtra+" and ks.ms_tk = "+ms_tk1+"), " +
                         "     L0 AS " +
                          " (SELECT cn.ms_chinhanh, cn.ten_chinhanh, tql.ms_tql, tql.ten_tql, " +
                         "          td.ms_tuyen, td.mo_ta_tuyen, ttdd.mo_ta AS mo_ta_tt_dd, ttdd.ms_tt_dd, " +
                         "          ttddh.mo_ta AS mo_ta_ttrang, ttddh.ms_ttrang, td.ms_cky, " +
                          "         La.ms_dh_mn, La.ms_tk, mn.stt_lo_trinh, dh.so_seri, " +
                          "         La.ms_mnoi, kh.ms_kh, mn.nguoi_thue, ('' + mn.dia_chi_mnoi + ', ' + duong.ten_duong) AS diachi, " +
                          "         kh.dien_thoai, mn.so_ho, mdsd.ms_md_chinh, dhd.mo_ta_dong AS gia_chinh, mdsd.ms_gia_vuot, " +
                           "        dhd1.mo_ta_dong AS gia_vuot, ckbd.ngay_thuc_te, La.ngay_doc_moi, " +
                          "         La.ngay_doc_cu, La.chi_so_cu, La.chi_so_moi, La.STT4, " +
                         "          La.url_image, La.ghi_chu, La.ms_dk_ks, La.ten_dk_ks " +
                         "   FROM La " +
                         "   LEFT JOIN moi_noi AS mn ON La.ms_mnoi = mn.ms_mnoi " +
                         "   LEFT JOIN khach_hang AS kh ON mn.ms_kh = kh.ms_kh " +
                          "  LEFT JOIN tt_doc_dh AS ttddh ON La.ms_ttrang = ttddh.ms_ttrang " +
                         "   LEFT JOIN ttrang_dd AS ttdd ON mn.ms_tt_dd = ttdd.ms_tt_dd " +
                         "   LEFT JOIN muc_dich_sd AS mdsd ON mn.ms_mnoi = mdsd.ms_mnoi " +
                        "    LEFT JOIN dong_hdon AS dhd ON mdsd.ms_md_chinh = dhd.ms_dong " +
                         "   LEFT JOIN dong_hdon AS dhd1 ON mdsd.ms_gia_vuot = dhd1.ms_dong " +
                         "   LEFT JOIN tuyen_doc AS td ON mn.ms_tuyen = td.ms_tuyen " +
                         "   LEFT JOIN cky_bien_doc AS ckbd ON td.ms_cky = ckbd.ms_cky " +
                         "   LEFT JOIN to_quan_ly AS tql ON td.ms_tql = tql.ms_tql " +
                         "   LEFT JOIN chi_nhanh AS cn ON tql.ms_chinhanh = cn.ms_chinhanh " +
                        "    LEFT JOIN duong ON mn.ms_duong = duong.ms_duong " +
                         "   LEFT JOIN dong_ho AS dh ON La.ms_dh = dh.ms_dh), " +
                         "     L1 AS " +
                         "  (SELECT L0.*, " +
                         "          hd.so_tieu_thu AS STT3  " +
                         "   FROM L0  " +
                         "   LEFT JOIN  " +
                         "    (SELECT * " +
                         "      FROM hoa_don  " +
                         "      WHERE hoa_don.ms_tk = "+(ms_tk1 - 1)+") AS hd ON L0.ms_mnoi = hd.ms_mnoi),  " +
                         "     L2 AS " +
                         "  (SELECT L1.*, " +
                         "          hd.so_tieu_thu AS STT2 " +
                         "   FROM L1 " +
                          "  LEFT JOIN " +
                          "    (SELECT * " +
                         "      FROM hoa_don " +
                         "      WHERE hoa_don.ms_tk = " + (ms_tk1 - 2) + ") AS hd ON L1.ms_mnoi = hd.ms_mnoi), " +
                         "     L3 AS " +
                         "  (SELECT L2.*, " +
                          "         hd.so_tieu_thu AS STT1 " +
                          "  FROM L2 " +
                         "   LEFT JOIN " +
                         "     (SELECT * " +
                          "     FROM hoa_don " +
                          "     WHERE hoa_don.ms_tk = " + (ms_tk1 - 3) + ") AS hd ON L2.ms_mnoi = hd.ms_mnoi) " +
                        " SELECT ms_chinhanh, ten_chinhanh, ms_tql, ten_tql, ms_tuyen, " +
                         "       mo_ta_tuyen, ms_tt_dd, mo_ta_tt_dd, ms_ttrang, mo_ta_ttrang, ms_cky, ms_dh_mn, " +
                         "       ms_tk, stt_lo_trinh, so_seri, ms_mnoi, ms_kh, nguoi_thue, " +
                         "       diachi, dien_thoai, so_ho, ms_md_chinh, gia_chinh, ms_gia_vuot, " +
                        "        gia_vuot, ngay_thuc_te, ngay_doc_cu, ngay_doc_moi, chi_so_cu, " +
                        "        chi_so_moi, STT1, STT2, STT3, STT4, url_image, ghi_chu, ms_dk_ks, ten_dk_ks  " +
                        "        FROM L3 " +
                        " WHERE ms_tql = "+ms_tql+" ORDER BY ms_tuyen, stt_lo_trinh ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }



        //----------DS khach hang tieu thu bang 0------
        [HttpGet]
        [Route("api/khachhang/getdskh")]
        public JsonResult<List<ThongTinKhachHang>> Getgettieuthu0()
        {
            string ms_chinhanh = HttpContext.Current.Request.QueryString.Get("ms_chinhanh");
            string ms_tql = HttpContext.Current.Request.QueryString.Get("ms_tql");
            string ms_tuyen = HttpContext.Current.Request.QueryString.Get("ms_tuyen");

            SqlHelper _db = new SqlHelper();
            string sql =
                 "select dhn.ms_mnoi, mn.stt_lo_trinh, mn.nguoi_thue, mn.dia_chi_mnoi, dh.ms_dh, dh.so_seri, "
                + " dhn.chi_so_cu, dhn.chi_so_moi, dhn.so_tthu, dhn.ngay_doc_moi, dhn.ngay_doc_cu, "
                + " mn.so_ho, kh.dien_thoai, td.ms_tuyen, dhn.ms_ttrang ms_ttrang_doc, mn.ms_tt_dd, "
                + " dhn.ghi_chu, dhn.url_image "
                + " from dh_noi dhn , moi_noi mn, khach_hang kh, dong_ho dh, tuyen_doc td, to_quan_ly tql, chi_nhanh cn "
                + " where dhn.ms_mnoi = mn.ms_mnoi "
                + " and dhn.ms_dh = dh.ms_dh "
                + " and mn.ms_kh = kh.ms_kh "
                + " and mn.ms_tuyen = td.ms_tuyen "
                + " and td.ms_tql = tql.ms_tql "
                + " and tql.ms_chinhanh = cn.ms_chinhanh "
                + " and td.ms_tuyen > = 5000 "
                + " and dhn.ms_tk = (select max(ms_tk) from thoi_ky) ";
                //+ " and dhn.so_tthu = 0 ";
            if (!ms_chinhanh.Equals("")) {
                sql += " and cn.ms_chinhanh = " + ms_chinhanh + " ";
            }

            if (!ms_tql.Equals(""))
            {
                sql+=  " and tql.ms_tql =  " + ms_tql;
            }

            if (!ms_tuyen.Equals(""))
            {
                sql += " and td.ms_tuyen =  " + ms_tuyen;
            }
            DataTable dt = _db.ExecuteSQLDataTable(sql) ;
            Console.Write(sql);
            List<ThongTinKhachHang> lsp = DataTableToListExtensions.DataTableToList<ThongTinKhachHang>(dt);
            return Json(lsp);
        }

        [HttpPut]
        [Route("api/khachhang/savecontrol/{id}")]
        public void luuKiemSoat(int id, [FromBody]DoiTuongKiemSoat dtks)
        {
            DateTime time = DateTime.Now;
            //string format = "yyyy-MM-dd HH:mm:ss";
            string format1 = "yyyyMMddHHmmss";// modify the format depending upon input required in the column in database 
            String url_path = "/ImageStorage/";
            String ImgName = id + "_" +dtks.ms_tk+"_"+dtks.ms_ks+"_"+ time.ToString(format1);
            String url = url_path + ImgName + ".jpg";
            //String sql = "Update dh_khoi set url_image = N'" + url + ", ngay_doc_moi2 = N'" + time.ToString(format) + "' where ms_dhk = " + id;
            SqlHelper _db = new SqlHelper();
            SaveImage(dtks.base_64_image, ImgName, dtks);
        }
        [HttpPost]
        [Route("api/khachhang/luuKiemSoat")]
        public void Post([FromBody]NoiDungKiemSoat ndks)
        {
            int ms_mnoi = ndks.ms_mnoi;
            int ms_userthanhtra = ndks.ms_userthanhtra;
            int ms_tk = ndks.ms_tk;
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            int chi_so_cu = ndks.chi_so_cu;
            int chi_so_moi = ndks.chi_so_moi;
            int chi_so_ks = ndks.chi_so_ks;
            int so_tthu_ks = ndks.so_tthu_ks;
            int ms_dk_ks = ndks.ms_dk_ks;
            int ms_tt_ks = ndks.ms_tt_ks;
            int co_chi_so = ndks.co_chi_so;

            int ms_tql = ndks.ms_tql;
            int co_chi_so_moi = ndks.co_chi_so_moi;
            String ghi_chu_ks = ndks.ghi_chu_ks;
            String thu_muc = ndks.thu_muc;

            string str_chi_so_ks = "null";
            string str_so_tthu_ks = "null";
            string str_chi_so_moi = "null";

            if (co_chi_so == 1) {
                str_chi_so_ks = chi_so_ks.ToString();
                str_so_tthu_ks = so_tthu_ks.ToString();
            }
            if (co_chi_so_moi == 1)
            {
                str_chi_so_moi = chi_so_moi.ToString();
            }
          
            string ngay_doc_cu = "null";
            string ngay_doc_moi = "null";
            if (null != ndks.ngay_doc_cu) {
                ngay_doc_cu =  "N'"+ndks.ngay_doc_cu.ToString(format)+"'";
            }

            if (DateTime.MinValue != ndks.ngay_doc_moi)
            {
                ngay_doc_moi = "N'"+ndks.ngay_doc_moi.ToString(format)+"'";
            }



            SqlHelper _db = new SqlHelper();
            String sqlInsert = $"Insert into kiem_soat ( ms_mnoi, ms_userthanhtra, ms_tk, ngay_kiem_soat, chi_so_cu, chi_so_moi, chi_so_ks, so_tthu_ks, ms_dk_ks, ms_tt_ks, ghi_chu_ks, thu_muc, ms_tql, ngay_doc_cu , ngay_doc_moi ) " +
                 $" values (" + ms_mnoi + " , " + ms_userthanhtra + ", " + ms_tk + ", " + "N'" + time.ToString(format) + "' , " + chi_so_cu + " , " + str_chi_so_moi + " , " + str_chi_so_ks + " , " + str_so_tthu_ks + " , "
                  + ms_dk_ks + " , " + ms_tt_ks + " , N'" + ghi_chu_ks + "' , N'" + thu_muc + "', "+ms_tql+" , "+ ngay_doc_cu +" ,"+ ngay_doc_moi + " )";

            /*String sqlInsert1 = $"Insert into kiem_soat ( ms_mnoi, ms_userthanhtra, ms_tk, ngay_kiem_soat, chi_so_cu, chi_so_moi, chi_so_ks, so_tthu_ks, ms_dk_ks, ms_tt_ks, ghi_chu_ks, thu_muc, ms_tql) " +
                 $" values (" + ms_mnoi + " , " + ms_userthanhtra + ", " + ms_tk + ", " + "N'" + time.ToString(format) + "' , " + chi_so_cu + " , " + chi_so_moi + " ,null , null , "
                  + ms_dk_ks + " , " + ms_tt_ks + " , N'" + ghi_chu_ks + "' , N'" + thu_muc + "', "+ms_tql+" )";

            String sqlInsert2 = $"Insert into kiem_soat ( ms_mnoi, ms_userthanhtra, ms_tk, ngay_kiem_soat, chi_so_cu, chi_so_moi, chi_so_ks, so_tthu_ks, ms_dk_ks, ms_tt_ks, ghi_chu_ks, thu_muc, ms_tql) " +
                $" values (" + ms_mnoi + " , " + ms_userthanhtra + ", " + ms_tk + ", " + "N'" + time.ToString(format) + "' , " + chi_so_cu + " , null ,null , null , "
                 + ms_dk_ks + " , " + ms_tt_ks + " , N'" + ghi_chu_ks + "' , N'" + thu_muc + "', " + ms_tql + " )";
                 */
            DataTable dt = _db.ExecuteSQLDataTable(sqlInsert);
            /*if (co_chi_so == 1) { 
                DataTable dt = _db.ExecuteSQLDataTable(sqlInsert);
            }else
            {
                if (co_chi_so_moi == 0)
                {
                    DataTable dt = _db.ExecuteSQLDataTable(sqlInsert2);
                }else if(co_chi_so_moi ==1)
                {
                    DataTable dt = _db.ExecuteSQLDataTable(sqlInsert1);
                }
            }*/

            String sqlUpdate = "Update locdulieuthanhtra set ttkiemsoat = 1 where ms_mnoi  = " + ms_mnoi + " and ms_userthanhtra = " + ms_userthanhtra + " and ms_tk = " + ms_tk;
            DataTable dt1 = _db.ExecuteSQLDataTable(sqlUpdate);

        }
        [HttpPut]
        [Route("api/khachhang/changepwd/{id}")]
        public bool ChangePwd(String id, [FromBody]UpdateUserPwd userchange)
        {
            SqlHelper _db = new SqlHelper();
            try
            {
                int rowEffect = Convert.ToInt32(_db.ExecuteSQLNonQuery($"Update userthanhtra set mk_userthanhtra = '" + userchange.NewPwd + "'  where ten_userthanhtra = '" + id + "' and mk_userthanhtra = '" + userchange.OldPwd + "'"));
                if (rowEffect == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;

            }

        }

        /*[HttpPost]
        [Route("api/dhkhoi/insertlsds")]
        public void GetOne([FromBody]MeterResetHis his)
        {
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd";
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"Insert into lich_su_dao_so_dh (ms_dh, ms_mnoi, ngay_thuc_hien, chi_so_dh, so_lan_dao_so) values (N'" + his.ms_dh + "', " + his.ms_mnoi + ", N'" + time.ToString(format) + "' , "+ his.so_lan_dao_so+ ")");

        }*/

        public bool SaveImage(string ImgStr, string ImgName, DoiTuongKiemSoat dtks)
        {
            ///WaterMeterImgs/2170028_636957629673844114.jpg
            
            String path = HttpContext.Current.Server.MapPath("~/ImageStorageCustomer"+"/"+ dtks.ms_tk+"/"+dtks.ms_userthanhtra+"/"+dtks.ms_mnoi); //Path  
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = ImgName + ".jpg";
            String url_image = "/ImageStorageCustomer" + "/" + dtks.ms_tk + "/" + dtks.ms_userthanhtra + "/" + dtks.ms_mnoi+"/"+imageName;
            SqlHelper _db = new SqlHelper();
            String sqlUpdate = "Update kiem_soat set url_image_ks = '" + url_image + "' where ms_mnoi = " + dtks.ms_mnoi + " and ms_userthanhtra = '" + dtks.ms_userthanhtra + "' and ms_tk = " + dtks.ms_tk;
            DataTable dt = _db.ExecuteSQLDataTable(sqlUpdate);
            //set the image path

            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }


        /*public bool SaveImage(string[] listImgStr, string folder)
        {
            String path = HttpContext.Current.Server.MapPath("~/ImageStorageControl"); //Path
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            for(int i = 0; i< listImgStr.Length; i++)
            {
                string imageName = folder+"_"+ i + ".jpg";
                string imgPath = Path.Combine(path, imageName);
                byte[] imageBytes = Convert.FromBase64String(listImgStr[i]);
                File.WriteAllBytes(imgPath, imageBytes);
            }
            //string imageName = ImgName + ".jpg";

            //set the image path
            

            

            

            return true;
        }*/



    }
}
