using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace D_Chandrakant.DataModels;

public partial class TailordbContext : DbContext
{
    public TailordbContext()
    {
    }

    public TailordbContext(DbContextOptions<TailordbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }
    public virtual DbSet<TblReportsetting> TblReportsettings { get; set; }

    public virtual DbSet<Billdetail> Billdetails { get; set; }

    public virtual DbSet<Billheader> Billheaders { get; set; }

    public virtual DbSet<Billpayment> Billpayments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerOld> CustomerOlds { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Emp> Emps { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeOld> EmployeeOlds { get; set; }

    public virtual DbSet<Empolyeesalary> Empolyeesalaries { get; set; }

    public virtual DbSet<Empwork> Empworks { get; set; }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            var entity = entry.Entity;
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entity.GetType().GetProperty("RecStatus").SetValue(entity, "D");
            }
        }
        return base.SaveChanges();
    }


    public virtual DbSet<Firm> Firms { get; set; }

    public virtual DbSet<Firm2> Firm2s { get; set; }

    public virtual DbSet<Fixedmeasurement> Fixedmeasurements { get; set; }

    public virtual DbSet<Head> Heads { get; set; }

    public virtual DbSet<Headgroup> Headgroups { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemOld> ItemOlds { get; set; }

    public virtual DbSet<Languagestring> Languagestrings { get; set; }

    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<Pattern> Patterns { get; set; }

    public virtual DbSet<Posting> Postings { get; set; }

    public virtual DbSet<Rateemp> Rateemps { get; set; }

    public virtual DbSet<Remark> Remarks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tailorrate> Tailorrates { get; set; }

    public virtual DbSet<Tailorvoucher> Tailorvouchers { get; set; }

    public virtual DbSet<Tblreport> Tblreports { get; set; }

    public virtual DbSet<Userview> Userviews { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("account")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.HeadId, "HeadID_idx");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.HeadId).HasColumnName("HeadID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Head).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.HeadId)
                .HasConstraintName("HeadID");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PRIMARY");

            entity
                .ToTable("bill")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CustomerId, "CustomerID_idx");

            entity.HasIndex(e => e.ItemIdFk, "ItemID_idx");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.Cgst)
                .HasMaxLength(45)
                .HasColumnName("CGST");
            entity.Property(e => e.ClothCharge).HasMaxLength(45);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Deliverytype).HasMaxLength(45);
            entity.Property(e => e.GrandTotal).HasMaxLength(45);
            entity.Property(e => e.ItemIdFk).HasColumnName("ItemID_fk");
            entity.Property(e => e.LabourCharge).HasMaxLength(45);
            entity.Property(e => e.MemoNo).HasMaxLength(45);
            entity.Property(e => e.MemoSeries).HasMaxLength(45);
            entity.Property(e => e.PaymentDeatials).HasMaxLength(45);
            entity.Property(e => e.PaymentType).HasMaxLength(45);
            entity.Property(e => e.RecivedDate).HasColumnType("datetime");
            entity.Property(e => e.RemainingGrand).HasMaxLength(45);
            entity.Property(e => e.Sgst)
                .HasMaxLength(45)
                .HasColumnName("SGST");
            entity.Property(e => e.Total).HasMaxLength(45);
            entity.Property(e => e.TrialDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("CustomerID");

            entity.HasOne(d => d.ItemIdFkNavigation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ItemIdFk)
                .HasConstraintName("ItemID_fk");
        });

        modelBuilder.Entity<TblReportsetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_reportsetting");

            entity.Property(e => e.ChartSettingDate).HasColumnType("timestamp");
        });

        modelBuilder.Entity<Billdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("billdetails");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BillHeaderId).HasColumnName("BillHeaderID");
            entity.Property(e => e.Cgst).HasColumnName("CGST");
            entity.Property(e => e.Cgstamount).HasColumnName("CGSTAmount");
            entity.Property(e => e.DeliveryNote).HasMaxLength(100);
            entity.Property(e => e.Hsncode)
                .HasMaxLength(255)
                .HasColumnName("HSNCode");
            entity.Property(e => e.IsDelivered).HasColumnType("bit(1)");
            entity.Property(e => e.ItemDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.MemoSeries).HasMaxLength(1);
            entity.Property(e => e.Sgst).HasColumnName("SGST");
            entity.Property(e => e.Sgstamount).HasColumnName("SGSTAmount");
            entity.Property(e => e.Status).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Billheader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("billheader");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BillDate).HasMaxLength(50);
            entity.Property(e => e.Byhand)
                .HasMaxLength(255)
                .HasColumnName("byhand");
            entity.Property(e => e.CardNumber).HasMaxLength(50);
            entity.Property(e => e.Cgstamount).HasColumnName("CGSTAmount");
            entity.Property(e => e.CustomerAddress).HasMaxLength(255);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerMobile).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.DeliveryType).HasMaxLength(20);
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.MagilBalanceNarration).HasMaxLength(50);
            entity.Property(e => e.MemoDate).HasColumnType("datetime");
            entity.Property(e => e.MemoSeries).HasMaxLength(1);
            entity.Property(e => e.PayMode).HasMaxLength(20);
            entity.Property(e => e.Sgstamount).HasColumnName("SGSTAmount");
            entity.Property(e => e.Status).HasMaxLength(200);
            entity.Property(e => e.TrialDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Weight).HasMaxLength(50);
        });

        modelBuilder.Entity<Billpayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("billpayment");

            entity.Property(e => e.CardChequeInfo).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemoSeries).HasMaxLength(1);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMode).HasMaxLength(50);
            entity.Property(e => e.PaymentType).HasMaxLength(255);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("customer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adate)
                .HasColumnType("datetime")
                .HasColumnName("ADate");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Bdate)
                .HasColumnType("datetime")
                .HasColumnName("BDate");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Custom1).HasMaxLength(50);
            entity.Property(e => e.Custom2).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirmName).HasMaxLength(255);
            entity.Property(e => e.Gstin)
                .HasMaxLength(50)
                .HasColumnName("GSTIN");
            entity.Property(e => e.Mobile1).HasMaxLength(255);
            entity.Property(e => e.Mobile2).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone1).HasMaxLength(255);
            entity.Property(e => e.Phone2).HasMaxLength(255);
            entity.Property(e => e.Weight).HasMaxLength(50);
        });

        modelBuilder.Entity<CustomerOld>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("customer_old")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adate)
                .HasColumnType("datetime")
                .HasColumnName("ADate");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Bdate)
                .HasColumnType("datetime")
                .HasColumnName("BDate");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Custom1).HasMaxLength(50);
            entity.Property(e => e.Custom2).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirmName).HasMaxLength(255);
            entity.Property(e => e.Gstin)
                .HasMaxLength(50)
                .HasColumnName("GSTIN");
            entity.Property(e => e.Mobile1).HasMaxLength(255);
            entity.Property(e => e.Mobile2).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone1).HasMaxLength(255);
            entity.Property(e => e.Phone2).HasMaxLength(255);
            entity.Property(e => e.Weight).HasMaxLength(50);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("department");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("deptName");
        });

        modelBuilder.Entity<Emp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("emp");

            entity.HasIndex(e => e.DeptFk, "emp_ibfk_1_idx");

            entity.Property(e => e.AccountNo).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.DeptFk).HasColumnName("DeptFK");
            entity.Property(e => e.EmpType)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Mobile).HasMaxLength(45);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.PfNo).HasMaxLength(200);
            entity.Property(e => e.ProfileImg).HasMaxLength(200);
            entity.Property(e => e.RecStatus).HasMaxLength(5);

            entity.HasOne(d => d.DeptFkNavigation).WithMany(p => p.Emps)
                .HasForeignKey(d => d.DeptFk)
                .HasConstraintName("emp_ibfk_1");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adate)
                .HasColumnType("datetime")
                .HasColumnName("ADate");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Bdate)
                .HasColumnType("datetime")
                .HasColumnName("BDate");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Mobile1).HasMaxLength(255);
            entity.Property(e => e.Mobile2).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.Phone1).HasMaxLength(255);
            entity.Property(e => e.Phone2).HasMaxLength(255);
            entity.Property(e => e.RoleIdfk).HasColumnName("RoleIDFK");
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<EmployeeOld>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("employee_old")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.RoleIdfk, "RoleIDFK_idx");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adate)
                .HasColumnType("datetime")
                .HasColumnName("ADate");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Bdate)
                .HasColumnType("datetime")
                .HasColumnName("BDate");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Mobile1).HasMaxLength(255);
            entity.Property(e => e.Mobile2).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.Phone1).HasMaxLength(255);
            entity.Property(e => e.Phone2).HasMaxLength(255);
            entity.Property(e => e.RoleIdfk).HasColumnName("RoleIDFK");
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.RoleIdfkNavigation).WithMany(p => p.EmployeeOlds)
                .HasForeignKey(d => d.RoleIdfk)
                .HasConstraintName("RoleIDFK");
        });

        modelBuilder.Entity<Empolyeesalary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empolyeesalary");

            entity.HasIndex(e => e.EmpIdFk, "EmpIdFK_idx");

            entity.Property(e => e.AdvanceSalaryDate).HasColumnType("datetime");
            entity.Property(e => e.EmpIdFk).HasColumnName("EmpIdFK");

            entity.HasOne(d => d.EmpIdFkNavigation).WithMany(p => p.Empolyeesalaries)
                .HasForeignKey(d => d.EmpIdFk)
                .HasConstraintName("EmpIdFK");
        });

        modelBuilder.Entity<Empwork>(entity =>
        {
            entity.HasKey(e => e.SrNo).HasName("PRIMARY");

            entity.ToTable("empwork");

            entity.HasIndex(e => e.MeasurementFk, "empwork_ibfk_1_idx");

            entity.HasIndex(e => e.BillDetailFk, "empwork_ibfk_2_idx");

            entity.HasIndex(e => e.ItemFk, "empwork_ibfk_3_idx");

            entity.HasIndex(e => e.EmpIdfk, "empwork_ibfk_4_idx");

            entity.HasIndex(e => e.RateId, "empwork_ibfk_5_idx");

            entity.Property(e => e.BillDetailFk).HasColumnName("BillDetailFK");
            entity.Property(e => e.CustomerName).HasMaxLength(250);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmpIdfk).HasColumnName("empIdfk");
            entity.Property(e => e.ItemFk).HasColumnName("itemFk");
            entity.Property(e => e.ItemName).HasMaxLength(45);
            entity.Property(e => e.MeasurementFk).HasColumnName("measurementFK");
            entity.Property(e => e.MemoSeries).HasMaxLength(1);
            entity.Property(e => e.OrderedQty).HasColumnName("orderedQty");
            entity.Property(e => e.RecStatus).HasMaxLength(5);

            entity.HasOne(d => d.BillDetailFkNavigation).WithMany(p => p.Empworks)
                .HasForeignKey(d => d.BillDetailFk)
                .HasConstraintName("empwork_ibfk_2");

            entity.HasOne(d => d.EmpIdfkNavigation).WithMany(p => p.Empworks)
                .HasForeignKey(d => d.EmpIdfk)
                .HasConstraintName("empwork_ibfk_4");

            entity.HasOne(d => d.ItemFkNavigation).WithMany(p => p.Empworks)
                .HasForeignKey(d => d.ItemFk)
                .HasConstraintName("empwork_ibfk_3");

            entity.HasOne(d => d.MeasurementFkNavigation).WithMany(p => p.Empworks)
                .HasForeignKey(d => d.MeasurementFk)
                .HasConstraintName("empwork_ibfk_1");

            //entity.HasOne(d => d.Rate).WithMany(p => p.Empworks)
            //    .HasForeignKey(d => d.RateId)
            //    .HasConstraintName("empwork_ibfk_5");
        });

        modelBuilder.Entity<Firm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("firm")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BillTitle).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Line1).HasMaxLength(255);
            entity.Property(e => e.Line2).HasMaxLength(255);
            entity.Property(e => e.Line3).HasMaxLength(255);
            entity.Property(e => e.Line4).HasMaxLength(255);
            entity.Property(e => e.Line5).HasMaxLength(255);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Mobile).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Vatno)
                .HasMaxLength(255)
                .HasColumnName("VATNo");
            entity.Property(e => e.WebSite).HasMaxLength(255);
        });

        modelBuilder.Entity<Firm2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("firm2")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BillTitle).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Line1).HasMaxLength(255);
            entity.Property(e => e.Line2).HasMaxLength(255);
            entity.Property(e => e.Line3).HasMaxLength(255);
            entity.Property(e => e.Line4).HasMaxLength(255);
            entity.Property(e => e.Line5).HasMaxLength(255);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Mobile).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Vatno)
                .HasMaxLength(255)
                .HasColumnName("VATNo");
            entity.Property(e => e.WebSite).HasMaxLength(255);
        });

        modelBuilder.Entity<Fixedmeasurement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fixedmeasurement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Back)
                .HasMaxLength(6)
                .HasColumnName("BACK");
            entity.Property(e => e.CustomerIdfk).HasColumnName("CustomerIDFK");
            entity.Property(e => e.Front)
                .HasMaxLength(6)
                .HasColumnName("FRONT");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.M1).HasMaxLength(6);
            entity.Property(e => e.M10).HasMaxLength(6);
            entity.Property(e => e.M2).HasMaxLength(6);
            entity.Property(e => e.M3).HasMaxLength(6);
            entity.Property(e => e.M4).HasMaxLength(6);
            entity.Property(e => e.M5).HasMaxLength(6);
            entity.Property(e => e.M6).HasMaxLength(6);
            entity.Property(e => e.M7).HasMaxLength(6);
            entity.Property(e => e.M8).HasMaxLength(6);
            entity.Property(e => e.M9).HasMaxLength(6);
            entity.Property(e => e.Mdate)
                .HasColumnType("datetime")
                .HasColumnName("MDATE");
            entity.Property(e => e.Pattern)
                .HasMaxLength(255)
                .HasColumnName("PATTERN");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("REMARK");
        });

        modelBuilder.Entity<Head>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("head")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AllowDelete).HasColumnType("bit(1)");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.HeadGroupId).HasColumnName("HeadGroupID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Headgroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("headgroup")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("item");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Cgst).HasColumnName("CGST");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(50)
                .HasColumnName("HSNCode");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Sgst).HasColumnName("SGST");
            entity.Property(e => e.Unit).HasMaxLength(20);
        });

        modelBuilder.Entity<ItemOld>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("item_old")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cgst).HasColumnName("CGST");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(50)
                .HasColumnName("HSNCode");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Sgst).HasColumnName("SGST");
            entity.Property(e => e.Unit).HasMaxLength(20);
        });

        modelBuilder.Entity<Languagestring>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("languagestring")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Lang).HasMaxLength(5);
            entity.Property(e => e.MsgId)
                .HasMaxLength(12)
                .HasColumnName("MsgID");
            entity.Property(e => e.MsgString).HasMaxLength(255);
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("measurement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Back)
                .HasMaxLength(255)
                .HasColumnName("BACK");
            entity.Property(e => e.BillHeaderIdIdx).HasColumnName("BillHeaderID_idx");
            entity.Property(e => e.Front)
                .HasMaxLength(255)
                .HasColumnName("FRONT");
            entity.Property(e => e.IsDelivered).HasColumnType("bit(1)");
            entity.Property(e => e.IsVoucherGenerated).HasColumnType("bit(1)");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.M1).HasMaxLength(255);
            entity.Property(e => e.M10).HasMaxLength(255);
            entity.Property(e => e.M2).HasMaxLength(255);
            entity.Property(e => e.M3).HasMaxLength(255);
            entity.Property(e => e.M4).HasMaxLength(255);
            entity.Property(e => e.M5).HasMaxLength(255);
            entity.Property(e => e.M6).HasMaxLength(255);
            entity.Property(e => e.M7).HasMaxLength(255);
            entity.Property(e => e.M8).HasMaxLength(255);
            entity.Property(e => e.M9).HasMaxLength(255);
            entity.Property(e => e.MemoNo).HasColumnName("MemoNO");
            entity.Property(e => e.MemoSeries).HasMaxLength(50);
            entity.Property(e => e.Pattern)
                .HasMaxLength(255)
                .HasColumnName("PATTERN");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("REMARK");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TailorId).HasColumnName("TailorID");
            entity.Property(e => e.TailorIrate).HasColumnName("TailorIRate");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VoucherId).HasColumnName("VoucherID");
        });

        modelBuilder.Entity<Pattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pattern");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.ItemIdfk).HasColumnName("ItemIDFK");
        });

        modelBuilder.Entity<Posting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("posting")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountPeriod).HasMaxLength(20);
            entity.Property(e => e.AssetType).HasMaxLength(10);
            entity.Property(e => e.VoucherId).HasColumnName("VoucherID");
        });

        modelBuilder.Entity<Rateemp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rateemp");

            entity.HasIndex(e => e.ItemIdFk, "'ItemIdFk'_idx");

            entity.HasOne(d => d.ItemIdFkNavigation).WithMany(p => p.Rateemps)
                .HasForeignKey(d => d.ItemIdFk)
                .HasConstraintName("'ItemIdFk'");
        });

        modelBuilder.Entity<Remark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("remark");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("role")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Tailorrate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tailorrate")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.EmployeeIdfk, "EmployeeIDFK_idx");

            entity.HasIndex(e => e.ItemIdfks, "ItemID_idx");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeIdfk).HasColumnName("EmployeeIDFK");
            entity.Property(e => e.ItemIdfks).HasColumnName("ItemIDfks");

            entity.HasOne(d => d.EmployeeIdfkNavigation).WithMany(p => p.Tailorrates)
                .HasForeignKey(d => d.EmployeeIdfk)
                .HasConstraintName("EmployeeIDFK");

            entity.HasOne(d => d.ItemIdfksNavigation).WithMany(p => p.Tailorrates)
                .HasForeignKey(d => d.ItemIdfks)
                .HasConstraintName("ItemIDfks");
        });

        modelBuilder.Entity<Tailorvoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tailorvoucher")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Narration).HasMaxLength(100);
            entity.Property(e => e.TailorId).HasColumnName("TailorID");
            entity.Property(e => e.Type).HasMaxLength(10);
        });

        modelBuilder.Entity<Tblreport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tblreport")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.RptNm).HasMaxLength(255);
            entity.Property(e => e.UsedSql)
                .HasMaxLength(255)
                .HasColumnName("UsedSQL");
        });

        modelBuilder.Entity<Userview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("userview")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Adate)
                .HasColumnType("datetime")
                .HasColumnName("ADate");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Bdate)
                .HasColumnType("datetime")
                .HasColumnName("BDate");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Mobile1).HasMaxLength(255);
            entity.Property(e => e.Mobile2).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.Phone1).HasMaxLength(255);
            entity.Property(e => e.Phone2).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("voucher")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ChequeDate).HasColumnType("datetime");
            entity.Property(e => e.ChequeDdnumber)
                .HasMaxLength(20)
                .HasColumnName("ChequeDDNumber");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Narration).HasMaxLength(100);
            entity.Property(e => e.PaymentMode).HasMaxLength(10);
            entity.Property(e => e.Type).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
