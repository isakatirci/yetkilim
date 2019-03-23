// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Migrations.ExternalUserMgr
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
  [Migration("20190102195012_ExternalUserMgr")]
  public class ExternalUserMgr : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>("Surname", "Users", (string) null, new bool?(), new int?(100), false, (string) null, true, (object) null, (string) null, (string) null, typeof (string), (string) null, new bool?(), new int?(100), false, false, (object) null, (string) null, (string) null, new bool?(), new bool?());
      migrationBuilder.AlterColumn<string>("Name", "Users", (string) null, new bool?(), new int?(100), false, (string) null, true, (object) null, (string) null, (string) null, typeof (string), (string) null, new bool?(), new int?(100), false, false, (object) null, (string) null, (string) null, new bool?(), new bool?());
      migrationBuilder.AlterColumn<string>("Email", "Users", (string) null, new bool?(), new int?((int) byte.MaxValue), false, (string) null, true, (object) null, (string) null, (string) null, typeof (string), (string) null, new bool?(), new int?((int) byte.MaxValue), false, false, (object) null, (string) null, (string) null, new bool?(), new bool?());
      migrationBuilder.AddColumn<int>("ExternalUserId", "Users", (string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?());
      migrationBuilder.AddColumn<bool>("IsExternal", "Users", (string) null, new bool?(), new int?(), false, (string) null, false, (object) false, (string) null, (string) null, new bool?());
      migrationBuilder.CreateTable("ExternalUser", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Provider = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        NameIdentifier = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table => table.PrimaryKey("PK_ExternalUser", x => x.Id));
      migrationBuilder.CreateIndex("IX_Users_ExternalUserId", "Users", "ExternalUserId", (string) null, false, (string) null);
      migrationBuilder.AddForeignKey("FK_Users_ExternalUser_ExternalUserId", "Users", "ExternalUserId", "ExternalUser", (string) null, (string) null, "Id", ReferentialAction.NoAction, ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey("FK_Users_ExternalUser_ExternalUserId", "Users", (string) null);
      migrationBuilder.DropTable("ExternalUser", (string) null);
      migrationBuilder.DropIndex("IX_Users_ExternalUserId", "Users", (string) null);
      migrationBuilder.DropColumn("ExternalUserId", "Users", (string) null);
      migrationBuilder.DropColumn("IsExternal", "Users", (string) null);
      migrationBuilder.AlterColumn<string>("Surname", "Users", (string) null, new bool?(), new int?(100), false, (string) null, false, (object) null, (string) null, (string) null, typeof (string), (string) null, new bool?(), new int?(100), false, true, (object) null, (string) null, (string) null, new bool?(), new bool?());
      migrationBuilder.AlterColumn<string>("Name", "Users", (string) null, new bool?(), new int?(100), false, (string) null, false, (object) null, (string) null, (string) null, typeof (string), (string) null, new bool?(), new int?(100), false, true, (object) null, (string) null, (string) null, new bool?(), new bool?());
      migrationBuilder.AlterColumn<string>("Email", "Users", (string) null, new bool?(), new int?((int) byte.MaxValue), false, (string) null, false, (object) null, (string) null, (string) null, typeof (string), (string) null, new bool?(), new int?((int) byte.MaxValue), false, true, (object) null, (string) null, (string) null, new bool?(), new bool?());
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
        b.Property<int>("FormId");
        b.Property<string>("FormValue");
        b.Property<int>("LikeRate");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<int>("PlaceId");
        b.Property<int?>("UserId");
        b.HasKey("Id");
        b.HasIndex("FormId");
        b.HasIndex("PlaceId");
        b.HasIndex("UserId");
        b.ToTable("Feedbacks");
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
        b.Property<string>("Surname").HasMaxLength(100);
        b.HasKey("Id");
        b.HasIndex("ExternalUserId");
        b.ToTable("Users");
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Company", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.CompanyType", "CompanyType").WithMany((string) null).HasForeignKey("CompanyTypeId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Feedback", (Action<EntityTypeBuilder>) (b =>
      {
        b.HasOne("Yetkilim.Domain.Entity.FeedbackForm", "Form").WithMany((string) null).HasForeignKey("FormId").OnDelete(DeleteBehavior.Restrict);
        b.HasOne("Yetkilim.Domain.Entity.Place", "Place").WithMany("Feedbacks").HasForeignKey("PlaceId").OnDelete(DeleteBehavior.Restrict);
        b.HasOne("Yetkilim.Domain.Entity.User", "User").WithMany("Feedbacks").HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
      }));
      modelBuilder.Entity("Yetkilim.Domain.Entity.FeedbackForm", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.Company", "Company").WithMany((string) null).HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Place", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.Company", "Company").WithMany("Places").HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.Promotion", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.User", "User").WithMany("Promotions").HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict)));
      modelBuilder.Entity("Yetkilim.Domain.Entity.User", (Action<EntityTypeBuilder>) (b => b.HasOne("Yetkilim.Domain.Entity.ExternalUser", "ExternalUser").WithMany((string) null).HasForeignKey("ExternalUserId").OnDelete(DeleteBehavior.Restrict)));
    }
  }
}
