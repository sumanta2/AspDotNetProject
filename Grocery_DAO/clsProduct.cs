using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace Grocery_DAO
{
    public class clsProduct
    {

        public static product FetchProduct(int? productid)
        {
            using (var ctx = new Entities())
            {
                var prod = ctx.products.Include(e => e.user).Where(a => a.productId == productid).SingleOrDefault();
                return prod;
            }
        }
        public static int AddProduct(product prod)
        {
            using (var ctx = new Entities())
            {
                ctx.products.Add(prod);
                ctx.SaveChanges();
                return prod.productId;
            }
        }
        public static List<product> ViewProduct()
        {
            
            using (var ctx = new Entities())
            {
                var prod = ctx.products.Include(e => e.user).ToList();
                return prod;
            }
        }

        public static List<product> ViewProductById(int id)
        {

           using (var ctx = new Entities())
            {
                var prod = ctx.products.Include(e => e.user).Where(f => f.userId == id).ToList();
                return prod;
            }
        }



        public static bool UpdateProduct(int prodid, string prodname, string proddesc, string prodprice, string prodqty, byte[] image_name)
        {
            using (var ctx = new Entities())
            {
                var prod = ctx.products.Where(a => a.productId == prodid).SingleOrDefault();
                prod.productName = prodname;
                prod.price = Convert.ToInt32(prodprice);
                prod.description = proddesc;
                prod.stockQuantity = Convert.ToInt32(prodqty);
                prod.image_name = image_name;
                ctx.Entry(prod).State = EntityState.Modified;
                ctx.SaveChanges();
                return true;
            }
        }


        ///when anyone order any product the product quantity will decrease the quanity
        public static bool UpdateProductQuantity(int prodid,int prodqty)
        {
            using (var ctx = new Entities())
            {
                var prod = ctx.products.Where(a => a.productId == prodid).SingleOrDefault();
                prod.stockQuantity = prod.stockQuantity - prodqty;
                ctx.Entry(prod).State = EntityState.Modified;
                ctx.SaveChanges();
                return true;
            }
        }

        public static List<user> GetSeller()
        {
            using (var ctx = new Entities())
            {
                return ctx.users.Where(user=>user.userType.Equals("seller")).ToList();
            }
        }

        public static bool DeleteProduct(int prodid)
        {

            using (var ctx = new Entities())
            {
                var prod = ctx.products.Where(a => a.productId.Equals(prodid)).SingleOrDefault();
                ctx.products.Remove(prod);
                ctx.SaveChanges();
                return true;
            }

        }
    }

    public class ProductConfiguration : EntityTypeConfiguration<product>
    {
        public ProductConfiguration()
        {
            // Primary Key
            this.HasKey(e => e.productId);
                
            // Properties
            this.Property(e => e.userId)
                .IsRequired();

            this.Property(e => e.productName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.description)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.price)
                .IsRequired();

            this.Property(e => e.stockQuantity)
                .IsRequired();

            this.Property(e => e.orderQuantity)
                .IsRequired();

            this.Property(e => e.image_name)
                .IsRequired()
                .HasMaxLength(50);



            // Table & Column Mappings
            this.ToTable("product"); // Name of your table in the database
            this.Property(e => e.productId).HasColumnName("productId");
            this.Property(e => e.userId).HasColumnName("userId");
            this.Property(e => e.productName).HasColumnName("productName");
            this.Property(e => e.description).HasColumnName("description");
            this.Property(e => e.price).HasColumnName("price");
            this.Property(e => e.stockQuantity).HasColumnName("stockQuantity");
            this.Property(e => e.orderQuantity).HasColumnName("orderQuantity");
            this.Property(e => e.image_name).HasColumnName("image_name");
        }

    }
}
