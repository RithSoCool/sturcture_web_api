using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_36_Entities : DbContext
    {
        public BCRM_36_Entities()
        {
        }

        public BCRM_36_Entities(DbContextOptions<BCRM_36_Entities> options)
            : base(options)
        {
        }

        public virtual DbSet<BCRM_Customer> BCRM_Customers { get; set; }
        public virtual DbSet<BCRM_Customer_Coupon_Code> BCRM_Customer_Coupon_Codes { get; set; }
        public virtual DbSet<BCRM_Dutchmill_Member> BCRM_Dutchmill_Members { get; set; }
        public virtual DbSet<BCRM_Dutchmill_Member_V1> BCRM_Dutchmill_Member_V1s { get; set; }
        public virtual DbSet<BCRM_Line_Flex_Template> BCRM_Line_Flex_Templates { get; set; }
        public virtual DbSet<BCRM_Line_Flex_Template_Replace> BCRM_Line_Flex_Template_Replaces { get; set; }
        public virtual DbSet<BCRM_Line_Info> BCRM_Line_Infos { get; set; }
        public virtual DbSet<BCRM_Login_State> BCRM_Login_States { get; set; }
        public virtual DbSet<CRM_Config> CRM_Configs { get; set; }
        public virtual DbSet<CRM_Customer> CRM_Customers { get; set; }
        public virtual DbSet<CRM_Customer_Address> CRM_Customer_Addresses { get; set; }
        public virtual DbSet<CRM_Point_Transaction> CRM_Point_Transactions { get; set; }
        public virtual DbSet<CRM_Privilege> CRM_Privileges { get; set; }
        public virtual DbSet<CRM_Privilege_Category> CRM_Privilege_Categories { get; set; }
        public virtual DbSet<CRM_Privilege_Category_Mapping> CRM_Privilege_Category_Mappings { get; set; }
        public virtual DbSet<CRM_Privilege_Group_Image> CRM_Privilege_Group_Images { get; set; }
        public virtual DbSet<CRM_Privilege_Image> CRM_Privilege_Images { get; set; }
        public virtual DbSet<CRM_Privilege_Transaction> CRM_Privilege_Transactions { get; set; }
        public virtual DbSet<Document_DN_Form_Value> Document_DN_Form_Values { get; set; }
        public virtual DbSet<Document_Document> Document_Documents { get; set; }
        public virtual DbSet<Document_Status_Log> Document_Status_Logs { get; set; }
        public virtual DbSet<Document_Store> Document_Stores { get; set; }
        public virtual DbSet<Document_Tx_Ref> Document_Tx_Refs { get; set; }
        public virtual DbSet<FFM_Ticket> FFM_Tickets { get; set; }
        public virtual DbSet<IM_Inv_Entry> IM_Inv_Entries { get; set; }
        public virtual DbSet<IM_Inventory> IM_Inventories { get; set; }
        public virtual DbSet<WL_Wallet> WL_Wallets { get; set; }
        public virtual DbSet<WL_Wallet_Ledger> WL_Wallet_Ledgers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BCRM_Customer>(entity =>
            {
                entity.ToTable("BCRM_Customer");

                entity.Property(e => e.ImageProfileUrl).HasMaxLength(500);

                entity.Property(e => e.Line_UserId).HasMaxLength(255);

                entity.Property(e => e.Point_Balance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Total_Spending).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Wallet_Alt_Ref).HasMaxLength(50);
            });

            modelBuilder.Entity<BCRM_Customer_Coupon_Code>(entity =>
            {
                entity.HasKey(e => e.CouponCodeId)
                    .HasName("PK__BCRM_Cus__BB806E5E79E64A2A");

                entity.ToTable("BCRM_Customer_Coupon_Code");

                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.CouponCode).HasMaxLength(255);

                entity.Property(e => e.Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.Privilege_Name).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(255);

                entity.Property(e => e.Status_Desc).HasMaxLength(5);

                entity.Property(e => e.Type_Desc).HasMaxLength(25);
            });

            modelBuilder.Entity<BCRM_Dutchmill_Member>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("BCRM_Dutchmill_Member");

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.DinaShippingAddress).HasMaxLength(2000);

                entity.Property(e => e.DinaShippingCountry).HasMaxLength(500);

                entity.Property(e => e.DinaShippingDistrict).HasMaxLength(500);

                entity.Property(e => e.DinaShippingFirstName).HasMaxLength(100);

                entity.Property(e => e.DinaShippingLastName).HasMaxLength(100);

                entity.Property(e => e.DinaShippingMobileNo).HasMaxLength(10);

                entity.Property(e => e.DinaShippingPostalCode).HasMaxLength(50);

                entity.Property(e => e.DinaShippingProvince).HasMaxLength(500);

                entity.Property(e => e.DinaShippingSubDistrict).HasMaxLength(500);

                entity.Property(e => e.District).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingAddress).HasMaxLength(2000);

                entity.Property(e => e.DutchmillShippingCountry).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingDistrict).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingFirstName).HasMaxLength(100);

                entity.Property(e => e.DutchmillShippingLastName).HasMaxLength(100);

                entity.Property(e => e.DutchmillShippingMobileNo).HasMaxLength(10);

                entity.Property(e => e.DutchmillShippingPostalCode).HasMaxLength(50);

                entity.Property(e => e.DutchmillShippingProvince).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingSubDistrict).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Gender).HasMaxLength(5);

                entity.Property(e => e.ImageProfileUrl).HasMaxLength(500);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LineChannelId)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LineID)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LineName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LinePictureUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.Property(e => e.Province).HasMaxLength(500);

                entity.Property(e => e.SubDistrict).HasMaxLength(500);
            });

            modelBuilder.Entity<BCRM_Dutchmill_Member_V1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BCRM_Dutchmill_Member_V1");

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.DinaShippingAddress).HasMaxLength(2000);

                entity.Property(e => e.DinaShippingCountry).HasMaxLength(500);

                entity.Property(e => e.DinaShippingDistrict).HasMaxLength(500);

                entity.Property(e => e.DinaShippingFirstName).HasMaxLength(100);

                entity.Property(e => e.DinaShippingLastName).HasMaxLength(100);

                entity.Property(e => e.DinaShippingMobileNo).HasMaxLength(10);

                entity.Property(e => e.DinaShippingPostalCode).HasMaxLength(50);

                entity.Property(e => e.DinaShippingProvince).HasMaxLength(500);

                entity.Property(e => e.DinaShippingSubDistrict).HasMaxLength(500);

                entity.Property(e => e.District).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingAddress).HasMaxLength(2000);

                entity.Property(e => e.DutchmillShippingCountry).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingDistrict).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingFirstName).HasMaxLength(100);

                entity.Property(e => e.DutchmillShippingLastName).HasMaxLength(100);

                entity.Property(e => e.DutchmillShippingMobileNo).HasMaxLength(10);

                entity.Property(e => e.DutchmillShippingPostalCode).HasMaxLength(50);

                entity.Property(e => e.DutchmillShippingProvince).HasMaxLength(500);

                entity.Property(e => e.DutchmillShippingSubDistrict).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Gender).HasMaxLength(5);

                entity.Property(e => e.ImageProfileUrl).HasMaxLength(500);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LineChannelId)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LineID)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LineName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LinePictureUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.MemberId).ValueGeneratedOnAdd();

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.Property(e => e.Province).HasMaxLength(500);

                entity.Property(e => e.SubDistrict).HasMaxLength(500);
            });

            modelBuilder.Entity<BCRM_Line_Flex_Template>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BCRM_Line_Flex_Template");

                entity.Property(e => e.JsonTemplate).IsRequired();

                entity.Property(e => e.SP_Mapped).HasMaxLength(255);

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Titile).HasMaxLength(255);
            });

            modelBuilder.Entity<BCRM_Line_Flex_Template_Replace>(entity =>
            {
                entity.HasKey(e => e.ReplaceId)
                    .HasName("PK__BCRM_Lin__68591851435DED9A");

                entity.ToTable("BCRM_Line_Flex_Template_Replace");

                entity.Property(e => e.Key).IsRequired();
            });

            modelBuilder.Entity<BCRM_Line_Info>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__BCRM_Lin__349DA5A63AA8D3A8");

                entity.ToTable("BCRM_Line_Info");

                entity.Property(e => e.Access_Token).HasMaxLength(1000);

                entity.Property(e => e.IAM_OAuth_TX_Ref).HasMaxLength(250);

                entity.Property(e => e.Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.Line_OAuth_State).HasMaxLength(250);

                entity.Property(e => e.Line_UserId).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.OAuth_Ref).HasMaxLength(250);

                entity.Property(e => e.Payload).HasMaxLength(1000);

                entity.Property(e => e.Picture_Url).HasMaxLength(500);

                entity.Property(e => e.Remart).HasMaxLength(100);
            });

            modelBuilder.Entity<BCRM_Login_State>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PK__BCRM_Log__4DDA2818A9CBB922");

                entity.ToTable("BCRM_Login_State");

                entity.Property(e => e.Access_Token).HasMaxLength(1000);

                entity.Property(e => e.Brand).HasMaxLength(100);

                entity.Property(e => e.IAM_OAuth_TX_Ref).HasMaxLength(250);

                entity.Property(e => e.Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.Line_OAuth_State).HasMaxLength(250);

                entity.Property(e => e.Payload).HasMaxLength(1000);
            });

            modelBuilder.Entity<CRM_Config>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.ToTable("CRM_Config");

                entity.Property(e => e.ConfigId).ValueGeneratedNever();

                entity.Property(e => e.Point_Asset_ARef).HasMaxLength(20);

                entity.Property(e => e.Point_Conversion_Rate).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<CRM_Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("CRM_Customer");

                entity.Property(e => e.Avg_Spending).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CRM_Wallet_Alt_Ref).HasMaxLength(50);

                entity.Property(e => e.Customer_Ref)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.First_Name_En).HasMaxLength(250);

                entity.Property(e => e.First_Name_Th).HasMaxLength(250);

                entity.Property(e => e.Gov_Id_Ref).HasMaxLength(250);

                entity.Property(e => e.Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Last_Name_En).HasMaxLength(250);

                entity.Property(e => e.Last_Name_Th).HasMaxLength(250);

                entity.Property(e => e.MBS_Tier_En).HasMaxLength(250);

                entity.Property(e => e.MBS_Tier_Th).HasMaxLength(250);

                entity.Property(e => e.Middle_Name_En).HasMaxLength(250);

                entity.Property(e => e.Middle_Name_Th).HasMaxLength(250);

                entity.Property(e => e.MobileNo).HasMaxLength(20);

                entity.Property(e => e.Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Registered_Channel).HasMaxLength(50);

                entity.Property(e => e.Spending).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title_En).HasMaxLength(250);

                entity.Property(e => e.Title_Th).HasMaxLength(250);
            });

            modelBuilder.Entity<CRM_Customer_Address>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("PK_CRM_Customer_Adress");

                entity.ToTable("CRM_Customer_Address");

                entity.Property(e => e.Addr_Label).HasMaxLength(1000);

                entity.Property(e => e.Addr_Remark).HasMaxLength(1000);

                entity.Property(e => e.Addr_Type_Desc).HasMaxLength(250);

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Building).HasMaxLength(250);

                entity.Property(e => e.ContactNo).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.District).HasMaxLength(250);

                entity.Property(e => e.First_Name).HasMaxLength(250);

                entity.Property(e => e.HouseNo).HasMaxLength(50);

                entity.Property(e => e.Label).HasMaxLength(500);

                entity.Property(e => e.Lane).HasMaxLength(250);

                entity.Property(e => e.Lang_Code).HasMaxLength(5);

                entity.Property(e => e.Last_Name).HasMaxLength(250);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Province).HasMaxLength(250);

                entity.Property(e => e.Street).HasMaxLength(250);

                entity.Property(e => e.SubDistrict).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.VillageNo).HasMaxLength(250);
            });

            modelBuilder.Entity<CRM_Point_Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.CV_Spending_Left).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Ext_TransactionId).HasMaxLength(50);

                entity.Property(e => e.ITF_Ref).HasMaxLength(250);

                entity.Property(e => e.ITF_Ref_2).HasMaxLength(250);

                entity.Property(e => e.Point_Conversion_Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Reference).HasMaxLength(250);

                entity.Property(e => e.Spending).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Status_Desc)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.TX_Type_Desc)
                    .IsRequired()
                    .HasMaxLength(2);
            });

            modelBuilder.Entity<CRM_Privilege>(entity =>
            {
                entity.HasKey(e => e.PrivilegeId)
                    .HasName("PK__CRM_Priv__B3E77E5C25F0CC25");

                entity.ToTable("CRM_Privilege");

                entity.Property(e => e.Alt_Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CPC_Pattern).HasMaxLength(1000);

                entity.Property(e => e.Exp_Mode_Desc)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Extra_Ref).HasMaxLength(250);

                entity.Property(e => e.Extra_Ref_2).HasMaxLength(250);

                entity.Property(e => e.Name_En).HasMaxLength(500);

                entity.Property(e => e.Name_Th).HasMaxLength(500);

                entity.Property(e => e.Privilege_BCI_Ref).HasMaxLength(250);

                entity.Property(e => e.Privilege_Image_Url).HasMaxLength(1000);

                entity.Property(e => e.Progress_Status_Desc)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.Req_Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.Status_Desc)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.SubName_En).HasMaxLength(500);

                entity.Property(e => e.SubName_Th).HasMaxLength(500);

                entity.Property(e => e.Value_Ref).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<CRM_Privilege_Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__CRM_Priv__19093A0BFA2C018B");

                entity.ToTable("CRM_Privilege_Category");

                entity.Property(e => e.Alt_Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Category_BCI_Ref).HasMaxLength(250);

                entity.Property(e => e.Category_Image_Url).HasMaxLength(1000);

                entity.Property(e => e.Category_Sub_Image_BCI_Ref).HasMaxLength(250);

                entity.Property(e => e.Category_Sub_Image_Url).HasMaxLength(1000);

                entity.Property(e => e.Name_En)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name_Th)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.Req_Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status_Desc)
                    .IsRequired()
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<CRM_Privilege_Category_Mapping>(entity =>
            {
                entity.HasKey(e => e.MappingId)
                    .HasName("PK__CRM_Priv__8B57819D076311F6");

                entity.ToTable("CRM_Privilege_Category_Mapping");

                entity.Property(e => e.Req_Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CRM_Privilege_Group_Image>(entity =>
            {
                entity.HasKey(e => e.Group_ImageId)
                    .HasName("PK__CRM_Priv__8B57819D1FEF1F8F");

                entity.ToTable("CRM_Privilege_Group_Image");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Req_Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type_Desc)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<CRM_Privilege_Image>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__CRM_Priv__7516F70C40870EA1");

                entity.ToTable("CRM_Privilege_Image");

                entity.Property(e => e.BCI_Ref).HasMaxLength(250);

                entity.Property(e => e.Image_Url).HasMaxLength(1000);

                entity.Property(e => e.Req_Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CRM_Privilege_Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__CRM_Priv__410680B12AD57565");

                entity.Property(e => e.Ext_TransactionId).HasMaxLength(50);

                entity.Property(e => e.ITF_Ref).HasMaxLength(250);

                entity.Property(e => e.ITF_Ref_2).HasMaxLength(250);

                entity.Property(e => e.Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Point_PerAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Point_Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.Req_Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.Status_Desc)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.TX_Ref).HasMaxLength(500);

                entity.Property(e => e.TX_Type_Desc)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.Void_Ref).HasMaxLength(500);

                entity.Property(e => e.Void_Remark).HasMaxLength(500);
            });

            modelBuilder.Entity<Document_DN_Form_Value>(entity =>
            {
                entity.HasKey(e => e.ValueId);

                entity.ToTable("Document_DN_Form_Value");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(1000);
            });

            modelBuilder.Entity<Document_Document>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.ToTable("Document_Document");

                entity.Property(e => e.Approve_By).HasMaxLength(50);

                entity.Property(e => e.DocumentNo).HasMaxLength(250);

                entity.Property(e => e.ITF_ExtraData).HasMaxLength(2500);

                entity.Property(e => e.ITF_Ref).HasMaxLength(250);

                entity.Property(e => e.ITF_Ref_2).HasMaxLength(250);

                entity.Property(e => e.Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.Int_Channel).HasMaxLength(25);

                entity.Property(e => e.MobileNo).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.PointOfPurchase).HasMaxLength(250);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.Remark_2).HasMaxLength(2000);
            });

            modelBuilder.Entity<Document_Status_Log>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__Document__5E548648EAACB540");

                entity.ToTable("Document_Status_Log");

                entity.Property(e => e.BillingNo).HasMaxLength(50);

                entity.Property(e => e.DocumentRef).HasMaxLength(100);

                entity.Property(e => e.DocumentRef_Stack).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(255);

                entity.Property(e => e.Spending).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Document_Store>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Document_Store");

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.Titile).HasMaxLength(255);
            });

            modelBuilder.Entity<Document_Tx_Ref>(entity =>
            {
                entity.HasKey(e => e.RefId)
                    .HasName("PK__Document__2D2A2CF1783E012E");

                entity.ToTable("Document_Tx_Ref");

                entity.Property(e => e.BillingNo).HasMaxLength(100);

                entity.Property(e => e.DocumentRef_Stack).HasMaxLength(100);

                entity.Property(e => e.RejectMessage).HasMaxLength(255);

                entity.Property(e => e.Spending).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Tx_Referance).HasMaxLength(100);
            });

            modelBuilder.Entity<FFM_Ticket>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.ToTable("FFM_Ticket");

                entity.Property(e => e.Addr_Remark).HasMaxLength(1000);

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Building).HasMaxLength(250);

                entity.Property(e => e.ContactNo).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.District).HasMaxLength(250);

                entity.Property(e => e.Extra_Ref).HasMaxLength(250);

                entity.Property(e => e.Extra_Ref_2).HasMaxLength(250);

                entity.Property(e => e.FFM_Detail).HasMaxLength(1000);

                entity.Property(e => e.FFM_Reference).HasMaxLength(250);

                entity.Property(e => e.FFM_Type_Desc)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.First_Name).HasMaxLength(250);

                entity.Property(e => e.HouseNo).HasMaxLength(50);

                entity.Property(e => e.Lane).HasMaxLength(250);

                entity.Property(e => e.Lang_Code)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Last_Name).HasMaxLength(250);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Province).HasMaxLength(250);

                entity.Property(e => e.Shipping_Company).HasMaxLength(250);

                entity.Property(e => e.Shipping_TrackingUrl).HasMaxLength(500);

                entity.Property(e => e.Street).HasMaxLength(250);

                entity.Property(e => e.SubDistrict).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.TrackingNo).HasMaxLength(250);

                entity.Property(e => e.VillageNo).HasMaxLength(250);
            });

            modelBuilder.Entity<IM_Inv_Entry>(entity =>
            {
                entity.HasKey(e => e.EntryId)
                    .HasName("PK__IM_Inv_E__F57BD2F794BA8A87");

                entity.ToTable("IM_Inv_Entry");

                entity.Property(e => e.Expired_Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Fullfillment_Mode_Desc)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Minimum_Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Remaining_Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Req_Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Reserved_Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Stock_Mode_Desc)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Total_Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Type_Desc)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Withdraw_Qty).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<IM_Inventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId)
                    .HasName("PK__IM_Inven__F5FDE6B34ACC0BBE");

                entity.ToTable("IM_Inventory");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(250);

                entity.Property(e => e.Inv_Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Remark).HasMaxLength(250);

                entity.Property(e => e.Req_Identity_SRef).HasMaxLength(50);
            });

            modelBuilder.Entity<WL_Wallet>(entity =>
            {
                entity.HasKey(e => e.WalletId);

                entity.ToTable("WL_Wallet");

                entity.Property(e => e.Alt_Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CRM_Customer_Ref).HasMaxLength(50);

                entity.Property(e => e.Expire).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Extra_Ref).HasMaxLength(250);

                entity.Property(e => e.Extra_Ref_2).HasMaxLength(250);

                entity.Property(e => e.Issue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Redeem).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WL_App_Id).HasMaxLength(25);

                entity.Property(e => e.WL_Identity_SRef).HasMaxLength(50);

                entity.Property(e => e.WL_Scope_Desc)
                    .IsRequired()
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<WL_Wallet_Ledger>(entity =>
            {
                entity.HasKey(e => e.LedgerId);

                entity.ToTable("WL_Wallet_Ledger");

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Ext_TransactionId).HasMaxLength(50);

                entity.Property(e => e.Extra_Ref).HasMaxLength(250);

                entity.Property(e => e.Extra_Ref_2).HasMaxLength(250);

                entity.Property(e => e.Pre_Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Reference).HasMaxLength(250);

                entity.Property(e => e.Remaining).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Status_Desc)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.TX_Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TX_Exp_Data).HasMaxLength(500);

                entity.Property(e => e.TX_Type_Desc)
                    .IsRequired()
                    .HasMaxLength(2);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
