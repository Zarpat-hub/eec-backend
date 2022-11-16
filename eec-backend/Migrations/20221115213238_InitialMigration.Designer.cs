﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eec_backend.Data;

#nullable disable

namespace eecbackend.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20221115213238_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("eec_backend.Models.Product", b =>
                {
                    b.Property<string>("ModelIdentifier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DesignType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("DimensionDeptht")
                        .HasColumnType("float");

                    b.Property<double?>("DimensionHeight")
                        .HasColumnType("float");

                    b.Property<double?>("DimensionWidth")
                        .HasColumnType("float");

                    b.Property<double>("EnergyConsumption")
                        .HasColumnType("float");

                    b.Property<string>("EnergyEfficiencyClass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("EnergyEfficiencyIndex")
                        .HasColumnType("float");

                    b.Property<string>("EnergySource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("RatedCapacity")
                        .HasColumnType("float");

                    b.Property<string>("SupplierOrTrademark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModelIdentifier");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
