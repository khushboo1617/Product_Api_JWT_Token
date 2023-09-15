using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Product_Api_JWT_Token.Data;
using Product_Api_JWT_Token.Models;
using System.IO;

namespace Product_Api_JWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly IWebHostEnvironment environment;
        public ProductController(UserContext userContext, IWebHostEnvironment environment)
        {
            _userContext = userContext;
            this.environment = environment;
        }

        [Authorize]
        [HttpGet]
        [Route("GetProducts")]
        public List<Product> GetProducts()
        {
            return _userContext.products.ToList();
        }

        [Authorize]
        [HttpGet]
        [Route("GetProduct")]
        public Product GetProduct(int id)
        {
            return _userContext.products.Where(x => x.Id == id).FirstOrDefault();
        }

        [Authorize]
        [HttpPost]
        [Route("AddProduct")]
        public string AddProduct(Product product) //
        {
            string response = string.Empty;
            Product pro = new Product();
            pro = _userContext.products.Where(x => x.Name == product.Name).FirstOrDefault();
            if (pro == null)
            {
                _userContext.products.Add(product);
                _userContext.SaveChanges();
                return "Product added successfully.";
            }
            else
            {
                return "Product with the same name already exists in system.";
            }

        }

        [Authorize]
        [HttpPut]
        [Route("UpdateProduct")]
        public string UpdateProduct(Product product)
        {
            Product pro = new Product();
            pro = _userContext.products.Where(x => x.Name == product.Name).FirstOrDefault();
            if (pro == null)
            {
                _userContext.Entry(product).State = EntityState.Modified;
                _userContext.SaveChanges();
                return "Product updated successfully.";
            }
            else
            {
                return "Product with the same name already exists in system.";
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteProduct")]
        public string DeleteProduct(int id)
        {
            Product product = _userContext.products.Where(x => x.Id == id).FirstOrDefault();
            if (product != null)
            {
                _userContext.products.Remove(product);
                _userContext.SaveChanges();
                return "Product deleted successfully.";
            }
            else
            {
                return "No such product found in system";
            }
        }

        [Authorize]
        [HttpPut("UploadMultipleImages")]
        public string UploadMultipleImages(IFormFileCollection filecollection, int id)
        {
            int pass = 0; int fail = 0;
            string response = string.Empty;
            string ImageUrl = string.Empty;
            string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            //string productName = _userContext.products.Where(x => x.Id == id).Select(y => y.Name).ToString();
            Product product = _userContext.products.Where(x => x.Id == id).FirstOrDefault();
            string productName = product.Name;
            try
            {
                string FilePath = this.environment.WebRootPath + "\\Upload\\product\\" + productName;
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                foreach (var file in filecollection)
                {
                    string imagepath = FilePath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        file.CopyToAsync(stream);
                        ImageUrl = hostUrl + "/Upload/product/" + productName + "/" + file.FileName;// + ".png";
                        product.Images = ImageUrl;
                        _userContext.Entry(product).State = EntityState.Modified;
                        _userContext.SaveChanges();
                        pass++;
                        response = "Images uploaded succesfully";
                    }
                }
            }
            catch (Exception ex)
            {
                fail++;
                response = "Failed to upload images.";

            }
            return response;
        }
    }

}
