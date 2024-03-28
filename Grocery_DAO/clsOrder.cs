using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery_DAO
{
    public class clsOrder
    {

        public static order FetchOrder(int? OrderId)
        {
            using (var ctx = new Entities())
            {
                var ordr = ctx.orders.Include(e => e.user).Include(f => f.product).Where(a => a.orderId == OrderId).SingleOrDefault();
                return ordr;
            }
        }

        public static int AddOrder(order ord)
        {
            using (var ctx = new Entities())
            {
                ctx.orders.Add(ord);
                ctx.SaveChanges();
                return ord.orderId;
            }
        }


        // display based on sellerId when seller is logged in and
        public static List<order> ViewOrder(int userId,string userType)
        {
            using (var ctx = new Entities())
            {
                if (userType.Equals("customer"))
                {
                    var ordr = ctx.orders.Include(e => e.user).Include(f => f.product).Where(e => e.userId == userId).ToList();
                    return ordr;
                }
                else 
                {
                    var ordr = ctx.orders.Include(e => e.user).Include(f => f.product).Where(e => e.product.userId == userId).ToList();
                    return ordr;
                }
            }
        }

        public static List<user> GetCustomer1()
        {
            using (var ctx = new Entities())
            {
                return ctx.users.Where(user=>user.userType.Equals("customer")).ToList();
            }
        }

        public static List<product> GetProduct()
        {
            using (var ctx = new Entities())
            {
                return ctx.products.ToList();
            }
        }

        // display list of user feature not desided to add my project till now
        public static bool DeleteOrder(int ordId)
        {

            using (var ctx = new Entities())
            {
                var ord = ctx.orders.Where(a => a.orderId.Equals(ordId)).SingleOrDefault();
                ctx.orders.Remove(ord);
                ctx.SaveChanges();
                return true;
            }

        }
    }

    public class OrderConfiguration1 : EntityTypeConfiguration<order>
    {
        public OrderConfiguration1()
        {
            // Primary Key
            this.HasKey(e => e.orderId);

            // Properties
            this.Property(e => e.userId)
                .IsRequired();

            this.Property(e => e.productId)
                .IsRequired();

            this.Property(e => e.quantity)
                .IsRequired();
                

            this.Property(e => e.total)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("order"); // Name of your table in the database
            this.Property(e => e.orderId).HasColumnName("orderId");
            this.Property(e => e.userId).HasColumnName("userId");
            this.Property(e => e.productId).HasColumnName("productId");
            this.Property(e => e.quantity).HasColumnName("quantity");
            this.Property(e => e.total).HasColumnName("total");
        }

    }
}
