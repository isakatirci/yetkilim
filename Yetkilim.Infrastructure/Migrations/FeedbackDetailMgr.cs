// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Migrations.FeedbackDetailMgr
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using Yetkilim.Infrastructure.Data.Context;

namespace Yetkilim.Infrastructure.Migrations
{
  [DbContext(typeof (YetkilimDbContext))]
  [Migration("20190114192420_FeedbackDetailMgr")]
  public class FeedbackDetailMgr : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>("DetailId", "Feedbacks", (string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?());
      migrationBuilder.CreateTable("FeedbackDetail", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        EmployeeRate = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        FlavorRate = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        PriceRate = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CleaningRate = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        AdviseRate = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table => table.PrimaryKey("PK_FeedbackDetail", x => x.Id));
      migrationBuilder.UpdateData("Companies", "Id", (object) 1, "CreatedDate", (object) new DateTime(2019, 1, 14, 22, 24, 19, 594, DateTimeKind.Local).AddTicks(7490L), (string) null);
      migrationBuilder.UpdateData("PanelUser", "Id", (object) 1, "CreatedDate", (object) new DateTime(2019, 1, 14, 22, 24, 19, 597, DateTimeKind.Local).AddTicks(192L), (string) null);
      migrationBuilder.CreateIndex("IX_Feedbacks_DetailId", "Feedbacks", "DetailId", (string) null, false, (string) null);
      migrationBuilder.AddForeignKey("FK_Feedbacks_FeedbackDetail_DetailId", "Feedbacks", "DetailId", "FeedbackDetail", (string) null, (string) null, "Id", ReferentialAction.NoAction, ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey("FK_Feedbacks_FeedbackDetail_DetailId", "Feedbacks", (string) null);
      migrationBuilder.DropTable("FeedbackDetail", (string) null);
      migrationBuilder.DropIndex("IX_Feedbacks_DetailId", "Feedbacks", (string) null);
      migrationBuilder.DropColumn("DetailId", "Feedbacks", (string) null);
      migrationBuilder.UpdateData("Companies", "Id", (object) 1, "CreatedDate", (object) new DateTime(2019, 1, 11, 0, 35, 5, 726, DateTimeKind.Local).AddTicks(6245L), (string) null);
      migrationBuilder.UpdateData("PanelUser", "Id", (object) 1, "CreatedDate", (object) new DateTime(2019, 1, 11, 0, 35, 5, 728, DateTimeKind.Local).AddTicks(7116L), (string) null);
    }

    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
      modelBuilder.HasAnnotation("ProductVersion", (object) "2.2.0-rtm-35687").HasAnnotation("Relational:MaxIdentifierLength", (object) 128).HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
      modelBuilder.Entity("Yetkilim.Domain.Entity.Company", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("Address");
        b.Property<int>("CompanyTypeId");
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<string>("Image");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("Name");
        b.HasKey("Id");
        b.HasIndex("CompanyTypeId");
        b.ToTable("Companies");
        b.HasData((object) new
        {
          Id = 1,
          CompanyTypeId = 1,
          CreatedBy = "Seed",
          CreatedDate = new DateTime(2019, 1, 14, 22, 24, 19, 594, DateTimeKind.Local).AddTicks(7490L),
          Name = "Yetkilim A.Ş"
        });
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.CompanyType", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("Code").HasMaxLength(20);
        b.Property<string>("Name").IsRequired(true).HasMaxLength(200);
        b.HasKey("Id");
        b.ToTable("CompanyTypes");
        b.HasData((object) new
        {
          Id = 1,
          Code = "CAFE",
          Name = "Cafe"
        }, (object) new
        {
          Id = 2,
          Code = "RESTAURANT",
          Name = "Restoran"
        });
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.ExternalUser", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("NameIdentifier");
        b.Property<string>("Provider");
        b.HasKey("Id");
        b.ToTable("ExternalUser");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Feedback", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<string>("Description");
        b.Property<int?>("DetailId");
        b.Property<int>("FormId");
        b.Property<string>("FormValue");
        b.Property<bool>("IsUserShare");
        b.Property<int>("LikeRate");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<int>("PlaceId");
        b.Property<int?>("UserId");
        b.HasKey("Id");
        b.HasIndex("DetailId");
        b.HasIndex("FormId");
        b.HasIndex("PlaceId");
        b.HasIndex("UserId");
        b.ToTable("Feedbacks");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.FeedbackDetail", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<int>("AdviseRate");
        b.Property<int>("CleaningRate");
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<int>("EmployeeRate");
        b.Property<int>("FlavorRate");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<int>("PriceRate");
        b.HasKey("Id");
        b.ToTable("FeedbackDetail");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.FeedbackForm", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<int>("CompanyId");
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<string>("FormItems");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.HasKey("Id");
        b.HasIndex("CompanyId");
        b.ToTable("FeedbackForms");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.PanelUser", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<int>("CompanyId");
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<string>("Email").HasMaxLength((int) byte.MaxValue);
        b.Property<bool>("IsDeleted");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("Name").HasMaxLength(100);
        b.Property<string>("Password");
        b.Property<int>("Role");
        b.Property<string>("Surname").HasMaxLength(100);
        b.HasKey("Id");
        b.HasIndex("CompanyId");
        b.ToTable("PanelUser");
        b.HasData((object) new
        {
          Id = 1,
          CompanyId = 1,
          CreatedDate = new DateTime(2019, 1, 14, 22, 24, 19, 597, DateTimeKind.Local).AddTicks(192L),
          Email = "super@yetkilim.com",
          IsDeleted = false,
          Name = "Super Admin",
          Password = "4BD37BEC2EAD95835D4B10BBF86A5649",
          Role = 1
        });
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Place", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("Address");
        b.Property<int>("CompanyId");
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<double?>("Latitude");
        b.Property<IPoint>("Location");
        b.Property<double?>("Longitude");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("Name");
        b.HasKey("Id");
        b.HasIndex("CompanyId");
        b.ToTable("Places");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Promotion", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<bool>("IsActive");
        b.Property<string>("Message");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("Title");
        b.Property<string>("UsageCode");
        b.Property<int>("UserId");
        b.HasKey("Id");
        b.HasIndex("UserId");
        b.ToTable("Promotions");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.User", (Action<EntityTypeBuilder>) (b =>
      {
        b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn);
        b.Property<string>("CreatedBy");
        b.Property<DateTime>("CreatedDate");
        b.Property<string>("Email").HasMaxLength((int) byte.MaxValue);
        b.Property<int?>("ExternalUserId");
        b.Property<bool>("IsDeleted");
        b.Property<bool>("IsExternal");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("Name").HasMaxLength(100);
        b.Property<string>("Password");
        b.Property<string>("Phone");
        b.HasKey("Id");
        b.HasIndex("ExternalUserId");
        b.ToTable("Users");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Company", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.CompanyType", "CompanyType").WithMany((string) null).HasForeignKey("CompanyTypeId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Feedback", (Action<EntityTypeBuilder>) (b =>
      {
        b.HasOne("Yetkilim.Domain.Entity.FeedbackDetail", "Detail").WithMany((string) null).HasForeignKey("DetailId").OnDelete(DeleteBehavior.Restrict);
        b.HasOne("Yetkilim.Domain.Entity.FeedbackForm", "Form").WithMany((string) null).HasForeignKey("FormId").OnDelete(DeleteBehavior.Restrict);
        b.HasOne("Yetkilim.Domain.Entity.Place", "Place").WithMany("Feedbacks").HasForeignKey("PlaceId").OnDelete(DeleteBehavior.Restrict);
        b.HasOne("Yetkilim.Domain.Entity.User", "User").WithMany("Feedbacks").HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.FeedbackForm", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.Company", "Company").WithMany((string) null).HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.PanelUser", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.Company", "Company").WithMany((string) null).HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Place", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.Company", "Company").WithMany("Places").HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Promotion", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.User", "User").WithMany("Promotions").HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.User", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.ExternalUser", "ExternalUser").WithMany((string) null).HasForeignKey("ExternalUserId").OnDelete(DeleteBehavior.Restrict)));
    }
  }
}
