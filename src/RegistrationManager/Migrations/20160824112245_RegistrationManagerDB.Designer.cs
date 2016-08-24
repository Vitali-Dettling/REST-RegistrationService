using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RegistrationManager.Models;

namespace RegistrationManager.Migrations
{
    [DbContext(typeof(DbManagerContext))]
    [Migration("20160824112245_RegistrationManagerDB")]
    partial class RegistrationManagerDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RegistrationManager.Models.Credentials", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 18);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("NewPassword")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 18);

                    b.HasKey("ID");

                    b.ToTable("DbCredentials");
                });
        }
    }
}
