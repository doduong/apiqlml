using APITest.Extensions;
using APITest.Helper;
using APITest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Http;

namespace APITest.Controllers
{
    public class SanPhamController : ApiController
    {
        [HttpGet]
        [Route("api/sanpham/getall")]
        public List<SanPham> GetAll()
        {
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable("Select * from SanPham");
            List<SanPham> lsp = DataTableToListExtensions.DataTableToList<SanPham>(dt);
            return lsp;
        }

        [HttpGet]
        [Route("api/sanpham/getsingle/{id}")]
        public SanPham GetSingle(int id)
        {
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"Select * from SanPham where id = {id}");
            SanPham lsp = DataTableToListExtensions.DataTableToList<SanPham>(dt)[0];
            return lsp;
        }

        [HttpPost]
        [Route("api/sanpham/getone")]
        public void GetOne([FromBody]SanPham sp)
        {
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"Insert into SanPham values (N'"+sp.masp+"', N'"+sp.tensp+"')");
            
        }

        [HttpPut]
        [Route("api/sanpham/updatesp/{id}")]
        public void UpdateSanPham(int id, [FromBody]SanPham sp)
        {
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"Update SanPham set masp = N'" + sp.masp + "', tensp = N'" + sp.tensp + "' where id = " +id);

        }

        [HttpDelete]
        [Route("api/sanpham/deletesp/{id}")]
        public void DeleteSanPham(int id)
        {
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"Delete SanPham where id = " + id);

        }



        [HttpPost]
        [Route("api/sanpham/insertimage")]
        public void insertSanPhamImage([FromBody]SanPham sp)
        {

            String url_path = "/WaterMeterImgs/";
            String ImgName = sp.masp + "_" + "NameImage";
            SqlHelper _db = new SqlHelper();
            DataTable dt = _db.ExecuteSQLDataTable($"Insert into SanPham values (N'" + sp.masp + "',N'"+sp.tensp+"', N'" + (url_path + ImgName )+"')");
            SaveImage(sp.url_image, ImgName);


        }


       
        public bool SaveImage(string ImgStr, string ImgName)
        {
            ///WaterMeterImgs/2170028_636957629673844114.jpg
            String path = HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = ImgName + ".jpg";

            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }

        /*public static Image Base64ToImage(string base64String)
       {
           // Convert base 64 string to byte[]
           byte[] imageBytes = Convert.FromBase64String(base64String);
           // Convert byte[] to Image
           using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
           {
               Image image = Image.FromStream(ms, true);
               return image;
           }
       }*/



    }
}
