using System.Collections.Generic;
using Alithea2.Models;

namespace Alithea2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Alithea2.Models.MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Alithea2.Models.MyDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //list category
//            var listCategory = new List<Category>();
//
//            string[] NameCategory = {"Quần áo thời trang Nam", "Quần áo thời trang Nữ", "Áo khoác, áo lạnh", "Giày dép Nam",
//                "Giày dép Nữ", "Đồ thời trang khác"};
//            string[] DescriptionCategory = {"Dành cho Nam", "Dành cho Nữ", "Áo ấm", "Dành cho Nam",
//                "Dành cho Nữ", "Phụ kiện"};
//            string[] ImageCategory =
//            {
//                    "https://4men.com.vn/images/thumbs/2019/05/ao-so-mi-trang-asm1266-10790-slide-products-5cedf46614ee3.png",
//                    "https://choquanao.vn/upload/images/1915179_L.jpg",
//                    "https://ae01.alicdn.com/kf/HTB1TCapQgHqK1RjSZFEq6AGMXXap/2019-Ng-i-n-ng-Qu-n-o-o-Kho-c-Qu-n-i-m-y.jpg_640x640.jpg",
//                    "https://vn-live-01.slatic.net/original/4e29f0a1804e3ddc742622f915810cf5.jpg",
//                    "https://my-test-11.slatic.net/original/70fbea987a0d5aa4c79cb1ea72aff2ff.jpg",
//                    "http://zenspa.com.vn/wp-content/uploads/2018/07/bo-my-pham-trang-diem-ca-nhan-co-ban-1.jpg"
//                };
//
//            for (int i = 0; i < 6; i++)
//            {
//                listCategory.Add(new Category()
//                {
//                    RoleNumber = (1000 + i).ToString(),
//                    CategoryName = NameCategory[i],
//                    CategoryDescription = DescriptionCategory[i],
//                    CreatedAt = Convert.ToDateTime("01/02/2005"),
//                    DeletedAt = null,
//                    UpdatedAt = Convert.ToDateTime("01/02/2005"),
//                    CategoryImage = ImageCategory[i],
//                    Status = Category.CategoryStatus.Active
//                });
//            }
//            context.Categories.AddRange(listCategory);
//            context.SaveChanges();
//
//            //list product
//            var listProduct = new List<Product>();
//
//            string[] NameProduct =
//            {
//                    "Tops Nam Dây Kéo Lá Thư Màu Đen Giải Trí", "Tops Nam Dây Kéo Lá Thư Màu Đen Giải Trí", "Tops Nam Bộ Lạc Trắng Giải Trí", "SHEIN Tops Nam Khối Màu Slogan Màu Xanh Lam Thể Thao", "SHEIN Tops Nam Hình Học Màu Đen Casual",
//                    "Đầm Ruched Hoa Nhiều Màu Boho", "SHEIN Đầm Pleated Hoa Nhiều Màu Boho", "SHEIN Jumpsuits Belted Polka Dot Màu Xanh Lam Giải Trí", "Đầm Nghề Thêu Bộ Lạc Trắng Thanh Lịch", "SHEIN Đầm Hoa Màu Vàng Thanh Lịch",
//                    "Áo Khoác POM Pom Bộ Lạc Màu Xanh Lá Giải Trí", "Áo Khoác Dây Kéo Màu Trơn Khaki Phong Cách Campus", "Áo Khoác Nam Dây Kéo Màu Trơn Hải Quân Giải Trí", "Áo Khoác Nam Tape Sọc Trắng Giải Trí", "SHEIN Áo Khoác Nam Dây Kéo Lá Thư Màu Đen Giải Trí",
//                    "Giày Thể Thao Nam Lá Thư Màu Đen Dễ Chịu", "Giày Boot Nam Màu Trơn Màu Be Dễ Chịu", "Giày Thể Thao Nam Khối Màu Nhiều Màu Dễ Chịu", "Giày Thể Thao Nam Màu Trơn Trắng Dễ Chịu", "Giày Thể Thao Nam Có Dây Buộc Phía Trước",
//                    "Giày Dép Nude Glamorous", "Bow Trang Trí Dép Đi Trong Nhà", "Giày Gót Trắng Tao Nhã", "Giày Gót Màu Đen Công Sở", "Giày Trang Trí Bằng Kim Loại",
//                    "Kính Râm Nam Top Top Shield", "Lưới Sọc 1 Cặp Rỗng Ra Vớ Dài", "Túi Trang Điểm Net", "20 Cái Quạt Hình Gradient Gradient Xử Lý Makeup Brush Set", "Curler Curash & Acne Pin Set 5pack"
//            };
//
//            string[] ImageProduct =
//            {
//                   
//                        "https://img.ltwebstatic.com/images3_pi/2019/12/04/1575439118de68686c9706281628cb805aa43560a5_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/05/1572949359e92cccec97ddef2745489165f8f2cd1a_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/10/16/1571209263a539785739688d01eb36a2a5ee685431_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images2_pi/2019/03/21/15531536483041337996_thumbnail_900x1199.webp",
//                        "https://img.ltwebstatic.com/images2_pi/2019/09/05/1567669884639096522_thumbnail_900x1199.webp",
//                    
//                        "https://img.ltwebstatic.com/images3_pi/2019/12/09/1575859696833602a70ff04222727f456207d27afc_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/20/1574238080d0245ad162c069573d32cad6b31a8626_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/10/25/15719921164a6c67091d2e9d858f80148a9a37c50f_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/27/1574824574ee43f904ba44b40b232b55f443832440_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/26/157477058998bc828dec1d0cca4ae397f54b7f9106_thumbnail_900x.webp",
//                  
//                        "https://img.ltwebstatic.com/images3_pi/2019/10/29/15723363175de450d22b65b9f4aa5872efcf803af1_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images/pi/201710/ca/15087542705427563284_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/01/15725784901dcfc8e53ae7a5eb6603905efd79c40e_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/25/15746583325757cdbb7e3c5f55c0d7abca3fbb7e92_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/10/18/1571393073a242756aee250dc13bc08c36c52a5308_thumbnail_900x.webp",
//                    
//                        "https://img.ltwebstatic.com/images3_pi/2019/10/29/157233530944f9a33cc2a9b30b770ce394ee3ac8dd_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/09/29/1569746391b89db665428a098dcdd36d9678e5944e_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/20/1574234418696b2b5f09500f85a27138d57df8ee1d_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/15/1573804711522b82bde9ae7adb7a7b6c8d0f9ec27c_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/08/157318596678866f6506d75a07e77fd4447c75e148_thumbnail_900x.webp",
//                   
//                        "https://img.ltwebstatic.com/images3_pi/2019/10/08/15705170232b63e2db177f2a026a856a30760d1188_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/05/1572942302403d79ab1a7b8dab4218310a9e78ac21_thumbnail_900x.webp",
//                        "https://img.shein.com/images/shein.com/201703/a5/14889382338933561200_thumbnail_900x1199.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/12/1573569710f935bfe7e32731f42269530e605e4507_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/06/1573029272260e9281497cdf9dbd7a4a26813bf1d6_thumbnail_900x.webp",
//                   
//                        "https://img.ltwebstatic.com/images2_pi/2019/08/02/1564733953245441013_thumbnail_900x1199.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/04/157284967427744b41e2ebee6ffaf4328594141e03_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images2_pi/2019/06/04/1559618859890919016_thumbnail_900x1199.webp",
//                        "https://img.ltwebstatic.com/images3_pi/2019/11/15/157379303381dbe00be013132dfcc32106bdf8d43e_thumbnail_900x.webp",
//                        "https://img.ltwebstatic.com/images2_pi/2019/04/02/15541681263674380832_thumbnail_900x1199.webp",
//                };
//
//            Random unitPriceRandom = new Random();
//            for (int i = 0; i < 30; i++)
//            {
//                listProduct.Add(new Product()
//                    {
//                        RoleNumber = (1000 + i).ToString() + (1000 + i).ToString(),
//                        ProductName = NameProduct[i],
//                        ProductDescription = "San Pham",
//                        ProductImage = ImageProduct[i],
//                        CreatedAt = Convert.ToDateTime("01/02/2005"),
//                        DeletedAt = null,
//                        UpdatedAt = Convert.ToDateTime("01/02/2005"),
//                        Status = Product.ProductStatus.Active,
//                        UnitPrice = unitPriceRandom.Next(100000, 1000000)
//                    });
//            }
//            context.Products.AddRange(listProduct);
//            context.SaveChanges();

            //category of product
//            Random catID = new Random();
//            var sotruoc = 0;
//            var sosau = 0;
//            List<ProductCategory> listProducatCategories = new List<ProductCategory>();
//            for (int i = 0; i < 30; i++)
//            {
//                for (int j = 0; j < 2; j++)
//                {
//                    while (sotruoc == sosau)
//                    {
//                        sotruoc = sosau;
//                        sosau = catID.Next(1, 6);
//
//                    }
//                    listProducatCategories.Add(new ProductCategory()
//                    {
//                        ProductID = i,
//                        CategoryID = sosau
//                    });
//                }
//            }
//
//            context.ProductCategories.AddRange(listProducatCategories);
//            context.SaveChanges();
        }
    }
}
