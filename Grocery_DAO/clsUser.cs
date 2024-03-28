using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery_DAO
{
    public class clsUser
    {

        public static user FetchUser(int? userId)
        {
            using (var ctx = new Entities())
            {
                var usr = ctx.users.Where(a => a.userId == userId).SingleOrDefault();
                return usr;
            }
        }

        public static user FetchUserByName(String Name)
        {
            using (var ctx = new Entities())
            {
                var usr = ctx.users.Where(a => a.userName == Name).SingleOrDefault();
                return usr;
            }
        }
        public static int AddUser(user usr)
        {
            using (var ctx = new Entities())
            {
                ctx.users.Add(usr);
                ctx.SaveChanges();
                return usr.userId;
            }
        }


        // display list of user feature not desided to add my project till now
        public static List<user> ViewUser()
        {
            using (var ctx = new Entities())
            {
                //var user = ctx.users.ToList();
                var user = ctx.users.ToList();
                return user;
            }
        }

        // display list of user feature not desided to add my project till now
        public static bool UpdateUser(int uid, string uname, string pswd, string userType, string firstName, string lastName, string contactNumber, string email)
        {

            using (var ctx = new Entities())
            {
                var user = ctx.users.Where(a => a.userId == uid).SingleOrDefault();
                user.userName = uname;
                user.password = pswd;
                user.userType = userType;
                user.firstName = firstName;
                user.lastName = lastName;
                user.contactNumber = contactNumber;
                user.email = email;
                
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
                return true;
            }
        }

        public static bool AuthenticateUser(string userName,string password)
        {
            using (var ctx = new Entities())
            {
                var usr = ctx.users.Where(a => a.userName.Equals(userName)).SingleOrDefault();
                if(usr != null && usr.password.Equals(password))
                {
                    return true;
                }
                return false;
            }
        }

        // display list of user feature not desided to add my project till now
        public static bool DeleteUser(int usrId)
        {
            using (var ctx = new Entities())
            {
                var usr = ctx.users.Where(a => a.userId.Equals(usrId)).SingleOrDefault();
                ctx.users.Remove(usr);

                ctx.SaveChanges();
                return true;
            }
        }
    }

    public class UserConfiguration : EntityTypeConfiguration<user>
    {
        public UserConfiguration()
        {
            // Primary Key
            this.HasKey(e => e.userId);

            // Properties
            this.Property(e => e.userName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.userType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.firstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.lastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.contactNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(e => e.email)
                .IsRequired()
                .HasMaxLength(50);




            // Table & Column Mappings
            this.ToTable("user"); // Name of your table in the database
            this.Property(e => e.userId).HasColumnName("userId");
            this.Property(e => e.userName).HasColumnName("userName");
            this.Property(e => e.password).HasColumnName("password");
            this.Property(e => e.userType).HasColumnName("userType");
            this.Property(e => e.firstName).HasColumnName("firstName");
            this.Property(e => e.lastName).HasColumnName("lastName");
            this.Property(e => e.contactNumber).HasColumnName("contactNumber");
            this.Property(e => e.email).HasColumnName("email");

        }

    }
}

