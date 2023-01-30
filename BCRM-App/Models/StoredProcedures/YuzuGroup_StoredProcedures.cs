using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_36_Entities : DbContext
    {
        public DbSet<sp_BCRM_DeleteAccount> sp_BCRM_DeleteAccount { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<sp_BCRM_DeleteAccount>(entity => entity.HasNoKey());
        }
    }

    public class sp_BCRM_DeleteAccount
    {
        //public string First_Name_Th { get; set; }
        //public string Last_Name_Th { get; set; }
        public string MobileNo { get; set; }
    }
}
