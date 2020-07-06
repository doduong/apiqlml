using APITest.Extensions;
using APITest.Helper;
using APITest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.IO;

namespace APITest.Controllers
{
    public class HoKyThuatController : ApiController
    {

        //--DS users ---------------------------
        [HttpGet]
        [Route("api/hkt/getusers")]
        public JsonResult<DataTable> getUsers()
        {
            //string ms_bd = HttpContext.Current.Request.QueryString.Get("ms_bd");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select users.Id, users.UserType_Id, users.Name, users.Pass, users.FullName, users.Status, type.UserType_Name from Users users, UserType type where users.UserType_Id = type.UserType_Id ");
            List<User> lsp = DataTableToListExtensions.DataTableToList<User>(dt);
            return Json(dt);
        }

        //-----------get user theo id------------
        [HttpGet]
        [Route("api/khachhang/getuserbyid")]
        public JsonResult<DataTable> GetUserById()
        {
            string id = HttpContext.Current.Request.QueryString.Get("id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select users.Id, users.UserType_Id, users.Name, users.Pass, users.FullName, users.Status, type.UserType_Name from Users users, UserType type where users.UserType_Id = type.UserType_Id where users.Status =1 and users.Id = '" + id + "'");
            List<ToQuanLy> lsp = DataTableToListExtensions.DataTableToList<ToQuanLy>(dt);
            return Json(dt);
        }

        //-----------DS Hố Kỹ Thuật------------
        [HttpGet]
        [Route("api/hkt/getholes")]
        public JsonResult<DataTable> GetHoles()
        {
            //string ms_userthanhtra = HttpContext.Current.Request.QueryString.Get("ms_userthanhtra");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                "select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Name,"
                +" (select u.FullName from Users u where u.Id = Hole.Maintain_User_Id) Maintain_Name, "
                + " (select u.FullName from Users u where u.Id = Hole.Inspect_User_Id) Inspect_Name "
                + " from Hole hole, HoleType holetype, HoleSize holesize, "
                + "  HoleStatus holestatus, Street street "
                + "  where hole.HoleType_Id = holetype.HoleType_Id "
                + "  and hole.HoleSize_Id = holesize.HoleSize_Id "
                + "  and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + "  and hole.Street_Id = street.Street_Id "
                + "  and hole.HoleStatus_Id = 1 "
                + " order by  street.Street_Route, hole.Hole_Route ");
            List<Hole> lsp = DataTableToListExtensions.DataTableToList<Hole>(dt);
            return Json(dt);
        }

        /*----------Tìm kiếm hố ký thuật  ------
             */
        [HttpGet]
        [Route("api/hkt/searchholes")]
        public JsonResult<DataTable> searchholes()
        {

            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord ;
            }

            SqlHelper _db = new SqlHelper();
            string sql = "select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Name,"
                + " (select u.FullName from Users u where u.Id = Hole.Maintain_User_Id) Maintain_Name, "
                + " (select u.FullName from Users u where u.Id = Hole.Inspect_User_Id) Inspect_Name "
                + " from Hole hole, HoleType holetype, HoleSize holesize, "
                + "  HoleStatus holestatus, Street street "
                + "  where hole.HoleType_Id = holetype.HoleType_Id "
                + "  and hole.HoleSize_Id = holesize.HoleSize_Id "
                + "  and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + "  and hole.Street_Id = street.Street_Id "
                + "  and hole.HoleStatus_Id = 1 "
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            List<Hole> lsp = DataTableToListExtensions.DataTableToList<Hole>(dt);
            return Json(dt);
        }

        //-----------DS Hố Kỹ Thuật theo Id------------
        [HttpGet]
        [Route("api/hkt/getholebyid")]
        public JsonResult<DataTable> GetHolesById()
        {
            string hole_id = HttpContext.Current.Request.QueryString.Get("hole_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                      "select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Name,"
                + " (select u.FullName from Users u where u.Id = Hole.Maintain_User_Id) Maintain_Name, "
                + " (select u.FullName from Users u where u.Id = Hole.Inspect_User_Id) Inspect_Name "
                + " from Hole hole, HoleType holetype, HoleSize holesize, "
                + "  HoleStatus holestatus, Street street "
                + "  where hole.HoleType_Id = holetype.HoleType_Id "
                + "  and hole.HoleSize_Id = holesize.HoleSize_Id "
                + "  and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + "  and hole.Street_Id = street.Street_Id "
                + "  and hole.HoleStatus_Id = 1 and hole.Hole_Id = " + hole_id);
            List<Hole> lsp = DataTableToListExtensions.DataTableToList<Hole>(dt);
            return Json(dt);
        }

        //--DS street  ---------------------------
        [HttpGet]
        [Route("api/hkt/getstreets")]
        public JsonResult<DataTable> getStreets()
        {
            //string ms_bd = HttpContext.Current.Request.QueryString.Get("ms_bd");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select Street_Id, Street_Route, Street_Name, Description from Street");
            List<Street> lsp = DataTableToListExtensions.DataTableToList<Street>(dt);
            return Json(dt);


        }
        
        //------------DS Hole Data -----------------
        [HttpGet]
        [Route("api/hkt/getholedata")]
        public JsonResult<DataTable> getHoleData()
        {
            //string ms_bd = HttpContext.Current.Request.QueryString.Get("ms_bd");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("select * from HoleData");
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }

        //------------DS Hole Data Full-----------------
        [HttpGet]
        [Route("api/hkt/getholedatafull")]
        public JsonResult<DataTable> getHoleDataFull()
        {
            //string ms_bd = HttpContext.Current.Request.QueryString.Get("ms_bd");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " order by  street.Street_Route,hole.Hole_Route ");
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }

        //------------DS Hole Data By Id-----------------
        [HttpGet]
        [Route("api/hkt/getholedatabyid")]
        public JsonResult<DataTable> getHoleDataById()
        {
            string hole_id = HttpContext.Current.Request.QueryString.Get("hole_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.hole_id = " + hole_id
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }


        /*------------DS Hole Data Chua Kiem Soat-----------------
         Thuoc tinh: Inspect_Status = 0;
         */
        [HttpGet]
        [Route("api/hkt/getholedatainspectnotyet")]
        public JsonResult<DataTable> getHoleDataNotInspect()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status = 0 "
                + " and hole.Inspect_User_Id = " + user_id 
                + " order by  street.Street_Route,hole.Hole_Route "
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }
        /*----------Tìm kiếm hố ký thuật chưa kiểm soát ------
         Điều kiện: holedata.Inspect_Status = 0
             */
        [HttpGet]
        [Route("api/hkt/timkiemholedata")]
        public JsonResult<DataTable> GetListCustomerControledSearch()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord ;
            }

            SqlHelper _db = new SqlHelper();
            string sql = " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status = 0"
                + " and hole.Inspect_User_Id = " + user_id
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            //List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }


        /*------------DS Hole Data Da Kiem Soat-----------------
         Thuoc tinh: Inspect_Status = 0;
         */
        [HttpGet]
        [Route("api/hkt/getholedatawereinspected")]
        public JsonResult<DataTable> getHoleDataWereInspected()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name,street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status > 0"
                + " and holedata.Ok_Status = 1 "
                + " and hole.Inspect_User_Id = " + user_id
               + " order by  street.Street_Route,hole.Hole_Route "
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }

        /*----------Tìm kiếm hố ký thuật đã kiểm soát ------
         Điều kiện: holedata.Inspect_Status = 1
             */
        [HttpGet]
        [Route("api/hkt/searchholedatawereinspected")]
        public JsonResult<DataTable> searchholedatawereinspected()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord;
            }

            SqlHelper _db = new SqlHelper();
            string sql = " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status > 0"
                + " and holedata.Ok_Status = 1 "
                + " and hole.Inspect_User_Id = " + user_id
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            return Json(dt);
        }


        /*------------DS Hole Data Chua Bao Duong-----------------
         Thuoc tinh: Inspect_Status = 0;
         */
        [HttpGet]
        [Route("api/hkt/getholedatamaintainnotyet")]
        public JsonResult<DataTable> getHoleDataNotTetMaintain()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Maintain_Status = 0"
                + " and hole.Maintain_User_Id =" + user_id
               + " order by  street.Street_Route,hole.Hole_Route "
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }

        /*----------Tìm kiếm hố ký thuật chưa bảo dưỡng ------
        Điều kiện: holedata.Maintain_Status = 0
            */
        [HttpGet]
        [Route("api/hkt/searchholedatamaintainnotyet")]
        public JsonResult<DataTable> searchholedatamaintainnotyet()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");

            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord;
            }

            SqlHelper _db = new SqlHelper();
            string sql = " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Maintain_Status = 0"
                + " and hole.Maintain_User_Id =" + user_id
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            return Json(dt);
        }


        /*------------DS Hole Data Da Bảo Dưỡng-----------------
         Thuoc tinh: Inspect_Status = 0;
         */
        [HttpGet]
        [Route("api/hkt/getholedataweremaintained")]
        public JsonResult<DataTable> getHoleDataWereMaintained()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Maintain_Status > 0"
                + " and hole.Maintain_User_Id =" + user_id
                + " order by  street.Street_Route,hole.Hole_Route "
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }


        /*----------Tìm kiếm hố ký thuật đã bảo dưỡng ------
        Điều kiện: holedata.Maintain_Status = 0
            */
        [HttpGet]
        [Route("api/hkt/searchholedataweremaintained")]
        public JsonResult<DataTable> searchholedataweremaintained()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord;
            }
            SqlHelper _db = new SqlHelper();
            string sql = " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Maintain_Status > 0"
                + " and hole.Maintain_User_Id =" + user_id
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            return Json(dt);
        }


        /*------------DS Hole Data Da Kiem Soat Nhung Chua Dat Cua User kiem soat-----------------
         Thuoc tinh: Inspect_Status = 1;
                    Ok_Status =0
         */
        [HttpGet]
        [Route("api/hkt/getholedatawereinspectednotok")]
        public JsonResult<DataTable> getHoleDataWereInspectedNotOk()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status > 0"
                + " and holedata.Ok_Status = 0 "
                + " and hole.Inspect_User_Id = " + user_id
                + " order by  street.Street_Route,hole.Hole_Route "
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }
        /*----------Tìm kiếm hố ký thuật đã kiểm soát nhung chua dat ------
         Điều kiện: holedata.Inspect_Status = 1
                    holedata.Ok_Status = 0
             */
        [HttpGet]
        [Route("api/hkt/searchholedatawereinspectednotok")]
        public JsonResult<DataTable> searchholedatawereinspectednotok()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord;
            }
            SqlHelper _db = new SqlHelper();
            string sql = " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status > 0"
                + " and holedata.Ok_Status = 0 "
                 + " and hole.Inspect_User_Id = " + user_id
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            return Json(dt);
        }

        /*------------DS Hole Data Da Kiem Soat Nhung Chua Dat Cua User kiem soat-----------------
         Thuoc tinh: Inspect_Status = 1;
                    Ok_Status =0
         */
        [HttpGet]
        [Route("api/hkt/getholedatawereinspectednotokofmaintain")]
        public JsonResult<DataTable> getholedatawereinspectednotokofmaintain()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable(
                " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status > 0"
                + " and holedata.Ok_Status = 0 "
                 + " and hole.Maintain_User_Id =" + user_id
                + " order by  street.Street_Route,hole.Hole_Route "
                );
            List<HoleData> lsp = DataTableToListExtensions.DataTableToList<HoleData>(dt);
            return Json(dt);


        }

        [HttpGet]
        [Route("api/hkt/searchholedatawereinspectednotokofmaintain")]
        public JsonResult<DataTable> searchholedatawereinspectednotokofmaintain()
        {
            string user_id = HttpContext.Current.Request.QueryString.Get("user_id");
            string conditionField = HttpContext.Current.Request.QueryString.Get("conditionField");
            string keyWord = HttpContext.Current.Request.QueryString.Get("keyWord");
            String strwhere = "";
            if (conditionField.Equals("pho"))
            {
                strwhere = " and street.Street_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("ma"))
            {
                strwhere = " and hole.Hole_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("diachi"))
            {
                strwhere = " and hole.Hole_Address like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("loai"))
            {
                strwhere = " and holetype.HoleType_Name like N'%" + keyWord + "%'";
            }
            else if (conditionField.Equals("hole_id"))
            {

                strwhere = " and hole.Hole_Id = " + keyWord;
            }

            SqlHelper _db = new SqlHelper();
            string sql = " select hole.*, holetype.HoleType_Name, holesize.HoleSize_Name, holestatus.HoleStatus_Name, street.Street_Route, street.Street_Name, "
                + " holedata.Period_Id, holedata.Maintain_Day, holedata.Inspect_Day, holedata.Maintain_Pic, holedata.Id,  "
                + " holedata.Inspect_Pic, holedata.Maintain_Status, holedata.Inspect_Status, holedata.Ok_Status, "
                + " holedata.Inspect_Count, holedata.Description as description_holedata "
                + " from HoleData holedata, Hole hole, HoleType holetype, HoleSize holesize, "
                + " HoleStatus holestatus, Street street "
                + " where "
                + "  holedata.Hole_Id = hole.Hole_Id  "
                + " and hole.HoleType_Id = holetype.HoleType_Id  "
                + " and hole.HoleSize_Id = holesize.HoleSize_Id "
                + " and hole.HoleStatus_Id = holestatus.HoleStatus_Id "
                + " and hole.Street_Id = street.Street_Id "
                + " and hole.HoleStatus_Id = 1 "
                + " and holedata.Period_Id = (select max(period_id) from Period)"
                + " and holedata.Inspect_Status > 0"
                + " and holedata.Ok_Status = 0 "
                + " and hole.Maintain_User_Id =" + user_id
                + " " + strwhere + " order by  street.Street_Route,hole.Hole_Route ";
            DataTable dt = _db.ExecuteSQLDataTable(sql);
            Console.Write(sql);
            //List<DuLieuKhachHang> lsp = DataTableToListExtensions.DataTableToList<DuLieuKhachHang>(dt);
            return Json(dt);
        }



        //------------DS Max Period -----------------
        [HttpGet]
        [Route("api/hkt/getmaxperiod")]
        public JsonResult<DataTable> getMaxPeriod()
        {
            //string ms_bd = HttpContext.Current.Request.QueryString.Get("ms_bd");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"select period_id, period_date from Period where period_id = (select max(period_id) from Period)");
            List<Period> lsp = DataTableToListExtensions.DataTableToList<Period>(dt);
            return Json(dt);

        }

        //----------CHECK LOGIN--------------
        [HttpGet]
        [Route("api/hkt/checklogin")]
        public JsonResult<DataTable> CheckLogin()
        {
            string name = HttpContext.Current.Request.QueryString.Get("name");
            string pass = HttpContext.Current.Request.QueryString.Get("pass");
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"select users.Id, users.UserType_Id, users.Name, users.Pass, users.FullName, users.Status, type.UserType_Name "+
            " from Users users, UserType type  where users.UserType_Id = type.UserType_Id and users.Name = '" + name + "' and users.Pass = '" + pass + "' and users.Status = 1 ");
            return Json(dt);
        }

        [HttpPost]
        [Route("api/hkt/luuKiemSoat")]
        public void Post([FromBody]KiemSoatHoleData ndks)
        {
            int hole_id = ndks.Hole_Id;
            string Inspect_Pic = ndks.Inspect_Pic;
            int Inspect_Status = ndks.Inspect_Status;
            int Ok_Status = ndks.Ok_Status;
            string Description = ndks.Description;
            int Inspect_Count = ndks.Inspect_Count;
            int isks = ndks.isks;
            string Hole_Name = ndks.Hole_Name;
            int Period_Id = ndks.Period_Id;
            string base_64_image = ndks.base_64_image;

            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            SqlHelper _db = new SqlHelper();

            if (!base_64_image.Equals("") && base_64_image != null) { 

                //string format = "yyyy-MM-dd HH:mm:ss";
                string format1 = "yyyyMMddHHmmss";// modify the format depending upon input required in the column in database 
                String url_path = "/ImageStorageHole/";
                String ImgName = Period_Id + "_" + hole_id + "_"  + time.ToString(format1);
                String url = url_path + ImgName + ".jpg";
                //String sql = "Update dh_khoi set url_image = N'" + url + ", ngay_doc_moi2 = N'" + time.ToString(format) + "' where ms_dhk = " + id;
     
                String path = HttpContext.Current.Server.MapPath("~/ImageStorageHole" + "/" + Period_Id + "/" + hole_id); //Path  
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                }

                string imageName = ImgName + ".jpg";
                String url_image = "/ImageStorageHole" + "/" + Period_Id +"/" + hole_id + "/" + imageName;
                //set the image path

                string imgPath = Path.Combine(path, imageName);

                byte[] imageBytes = Convert.FromBase64String(base_64_image);
                File.WriteAllBytes(imgPath, imageBytes);

                String sqlUpdate = $"Update HoleData set Inspect_Day = N'" + time.ToString(format) + "' , Inspect_Pic = N'"+url_image+"' , Inspect_Status = "+Inspect_Status+
                    ", Ok_Status = "+ Ok_Status + ", Inspect_Count = "+ Inspect_Count + ", Description = N'"+Description+"' where Hole_Id = " + hole_id + " and Period_Id = " + Period_Id;
                DataTable dt = _db.ExecuteSQLDataTable(sqlUpdate);
            }
            else
            {
                String sqlUpdate = $"Update HoleData set Inspect_Day = N'" + time.ToString(format) + "' , Inspect_Pic = '" + null + "' , Inspect_Status = " + Inspect_Status +
                   ", Ok_Status = " + Ok_Status + ", Inspect_Count = " + Inspect_Count + ", Description = '" + Description + "' where Hole_Id = " + hole_id + " and Period_Id = " + Period_Id;
                DataTable dt = _db.ExecuteSQLDataTable(sqlUpdate);

            }  



        }

        [HttpPost]
        [Route("api/hkt/luuBaoDuong")]
        public void Post([FromBody] BaoDuongHoleData ndks)
        {
            int hole_id = ndks.Hole_Id;
            string Maintain_Pic = ndks.Maintian_Pic;
            int Maintain_Status = ndks.Maintain_Status;
            string Description = ndks.Description;
            string Hole_Name = ndks.Hole_Name;
            int Period_Id = ndks.Period_Id;
            string base_64_image = ndks.base_64_image;

            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            SqlHelper _db = new SqlHelper();

            if (!base_64_image.Equals("") && base_64_image != null)
            {

                //string format = "yyyy-MM-dd HH:mm:ss";
                string format1 = "yyyyMMddHHmmss";// modify the format depending upon input required in the column in database 
                String url_path = "/ImageStorageHole/";
                String ImgName = Period_Id + "_" + hole_id + "_" + time.ToString(format1);
                String url = url_path + ImgName + ".jpg";
                //String sql = "Update dh_khoi set url_image = N'" + url + ", ngay_doc_moi2 = N'" + time.ToString(format) + "' where ms_dhk = " + id;

                String path = HttpContext.Current.Server.MapPath("~/ImageStorageHole" + "/" + Period_Id + "/" + hole_id); //Path  
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                }

                string imageName = ImgName + ".jpg";
                String url_image = "/ImageStorageHole" + "/" + Period_Id + "/" + hole_id + "/" + imageName;
                //set the image path

                string imgPath = Path.Combine(path, imageName);

                byte[] imageBytes = Convert.FromBase64String(base_64_image);
                File.WriteAllBytes(imgPath, imageBytes);

                String sqlUpdate = $"Update HoleData set Maintain_Day = N'" + time.ToString(format) + "' , Maintain_Pic = N'" + url_image + "' , Maintain_Status = " + Maintain_Status +
                    ", Description = N'" + Description + "' where Hole_Id = " + hole_id + " and Period_Id = " + Period_Id;
                DataTable dt = _db.ExecuteSQLDataTable(sqlUpdate);
            }
            else
            {
                String sqlUpdate = $"Update HoleData set Maintain_Day = N'" + time.ToString(format) + "' , Maintain_Pic = '" + null + "' , Maintain_Status = " + Maintain_Status +
                   ", Description = '" + Description + "' where Hole_Id = " + hole_id + " and Period_Id = " + Period_Id;
                DataTable dt = _db.ExecuteSQLDataTable(sqlUpdate);

            }



        }
        //Change password 
        [HttpPut]
        [Route("api/hkt/changepwd/{id}")]
        public bool ChangePwd(String id, [FromBody]UpdateUserPwd userchange)
        {
            SqlHelper _db = new SqlHelper();
            try
            {
                int rowEffect = Convert.ToInt32(_db.ExecuteSQLNonQuery($"Update users set pass = '" + userchange.NewPwd + "'  where name = '" + id + "' and Pass = '" + userchange.OldPwd + "'"));
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

        //Change password 
        [HttpPut]
        [Route("api/hkt/updatetoado/{id}")]
        public bool updatetoado(String id, [FromBody]UpdateToaDo toaDo)
        {
            SqlHelper _db = new SqlHelper();
            try
            {
                int rowEffect = Convert.ToInt32(_db.ExecuteSQLNonQuery($"Update Hole set Hole_Latitude = '" + toaDo.Hole_Latitude + "', Hole_Longitude = '"+toaDo.Hole_Longitude +"'  where Hole_Id = '" + toaDo.Hole_Id + "'"));
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

        /*public bool SaveImage(string ImgStr, string ImgName, DoiTuongKiemSoat dtks)
        {
            ///WaterMeterImgs/2170028_636957629673844114.jpg

            String path = HttpContext.Current.Server.MapPath("~/ImageStorageHole" + "/" + dtks.ms_tk + "/" + dtks.ms_userthanhtra + "/" + dtks.ms_mnoi); //Path  
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = ImgName + ".jpg";
            String url_image = "/ImageStorageHole" + "/" + dtks.ms_tk + "/" + dtks.ms_userthanhtra + "/" + dtks.ms_mnoi + "/" + imageName;
            SqlHelper _db = new SqlHelper();
            String sqlUpdate = "Update kiem_soat set url_image_ks = '" + url_image + "' where ms_mnoi = " + dtks.ms_mnoi + " and ms_userthanhtra = '" + dtks.ms_userthanhtra + "' and ms_tk = " + dtks.ms_tk;
            DataTable dt = _db.ExecuteSQLDataTable(sqlUpdate);
            //set the image path

            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }*/



    }
}
