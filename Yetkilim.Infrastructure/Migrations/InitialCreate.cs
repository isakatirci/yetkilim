// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Migrations.InitialCreate
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

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
  [Migration("20181227161017_InitialCreate")]
  public class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable("CompanyTypes", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        Name = table.Column<string>((string) null, new bool?(), new int?(200), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        Code = table.Column<string>((string) null, new bool?(), new int?(20), false, (string) null, true, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table => table.PrimaryKey("PK_CompanyTypes", x => x.Id));
      migrationBuilder.CreateTable("Users", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Name = table.Column<string>((string) null, new bool?(), new int?(100), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        Surname = table.Column<string>((string) null, new bool?(), new int?(100), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        Email = table.Column<string>((string) null, new bool?(), new int?((int) byte.MaxValue), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        Password = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        IsDeleted = table.Column<bool>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table => table.PrimaryKey("PK_Users", x => x.Id));
      migrationBuilder.CreateTable("Companies", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Name = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Image = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Address = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CompanyTypeId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table =>
      {
        table.PrimaryKey("PK_Companies", x => x.Id);
        table.ForeignKey("FK_Companies_CompanyTypes_CompanyTypeId", x => x.CompanyTypeId, "CompanyTypes", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
      });
      migrationBuilder.CreateTable("Promotions", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Title = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Message = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        UsageCode = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        IsActive = table.Column<bool>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        UserId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table =>
      {
        table.PrimaryKey("PK_Promotions", x => x.Id);
        table.ForeignKey("FK_Promotions_Users_UserId", x => x.UserId, "Users", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
      });
      migrationBuilder.CreateTable("FeedbackForms", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CompanyId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        FormItems = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table =>
      {
        table.PrimaryKey("PK_FeedbackForms", x => x.Id);
        table.ForeignKey("FK_FeedbackForms_Companies_CompanyId", x => x.CompanyId, "Companies", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
      });
      migrationBuilder.CreateTable("Places", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Name = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Address = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Latitude = table.Column<double>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Longitude = table.Column<double>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CompanyId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table =>
      {
        table.PrimaryKey("PK_Places", x => x.Id);
        table.ForeignKey("FK_Places_Companies_CompanyId", x => x.CompanyId, "Companies", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
      });
      migrationBuilder.CreateTable("Feedbacks", table => new
      {
        Id = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()).Annotation("SqlServer:ValueGenerationStrategy", (object) SqlServerValueGenerationStrategy.IdentityColumn),
        ModifiedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        CreatedDate = table.Column<DateTime>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        CreatedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        ModifiedBy = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        Description = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        LikeRate = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        FormValue = table.Column<string>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        FormId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?()),
        UserId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, true, (object) null, (string) null, (string) null, new bool?()),
        PlaceId = table.Column<int>((string) null, new bool?(), new int?(), false, (string) null, false, (object) null, (string) null, (string) null, new bool?())
      }, (string) null, table =>
      {
        table.PrimaryKey("PK_Feedbacks", x => x.Id);
        table.ForeignKey("FK_Feedbacks_FeedbackForms_FormId", x => x.FormId, "FeedbackForms", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
        table.ForeignKey("FK_Feedbacks_Places_PlaceId", x => x.PlaceId, "Places", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
        table.ForeignKey("FK_Feedbacks_Users_UserId", x => x.UserId, "Users", "Id", (string) null, ReferentialAction.NoAction, ReferentialAction.Restrict);
      });
      migrationBuilder.InsertData("CompanyTypes", new string[3]
      {
        "Id",
        "Code",
        "Name"
      }, new object[3]
      {
        (object) 1,
        (object) "CAFE",
        (object) "Cafe"
      }, (string) null);
      migrationBuilder.InsertData("CompanyTypes", new string[3]
      {
        "Id",
        "Code",
        "Name"
      }, new object[3]
      {
        (object) 2,
        (object) "RESTAURANT",
        (object) "Restoran"
      }, (string) null);
      migrationBuilder.CreateIndex("IX_Companies_CompanyTypeId", "Companies", "CompanyTypeId", (string) null, false, (string) null);
      migrationBuilder.CreateIndex("IX_FeedbackForms_CompanyId", "FeedbackForms", "CompanyId", (string) null, false, (string) null);
      migrationBuilder.CreateIndex("IX_Feedbacks_FormId", "Feedbacks", "FormId", (string) null, false, (string) null);
      migrationBuilder.CreateIndex("IX_Feedbacks_PlaceId", "Feedbacks", "PlaceId", (string) null, false, (string) null);
      migrationBuilder.CreateIndex("IX_Feedbacks_UserId", "Feedbacks", "UserId", (string) null, false, (string) null);
      migrationBuilder.CreateIndex("IX_Places_CompanyId", "Places", "CompanyId", (string) null, false, (string) null);
      migrationBuilder.CreateIndex("IX_Promotions_UserId", "Promotions", "UserId", (string) null, false, (string) null);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable("Feedbacks", (string) null);
      migrationBuilder.DropTable("Promotions", (string) null);
      migrationBuilder.DropTable("FeedbackForms", (string) null);
      migrationBuilder.DropTable("Places", (string) null);
      migrationBuilder.DropTable("Users", (string) null);
      migrationBuilder.DropTable("Companies", (string) null);
      migrationBuilder.DropTable("CompanyTypes", (string) null);
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
        b.Property<string>("Email").IsRequired(true).HasMaxLength((int) byte.MaxValue);
        b.Property<bool>("IsDeleted");
        b.Property<string>("ModifiedBy");
        b.Property<DateTime?>("ModifiedDate");
        b.Property<string>("Name").IsRequired(true).HasMaxLength(100);
        b.Property<string>("Password");
        b.Property<string>("Surname").IsRequired(true).HasMaxLength(100);
        b.HasKey("Id");
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
    }
  }
}
