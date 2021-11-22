﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PFM.Database;

namespace PFM.Migrations
{
    [DbContext(typeof(PfmDbContext))]
    [Migration("20211121224853_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("PFM.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ParentCode")
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("PFM.Database.Entities.MccCodes", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("MerchanTtype")
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("mcccodes");
                });

            modelBuilder.Entity("PFM.Database.Entities.SplitTransactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("CategoriesId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TransactionsId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoriesId");

                    b.HasIndex("TransactionsId");

                    b.ToTable("splittransactions");
                });

            modelBuilder.Entity("PFM.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("BeneficiaryName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("CategoryCode")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("MccCodeCode")
                        .HasColumnType("integer");

                    b.Property<bool>("isSplited")
                        .HasColumnType("boolean");

                    b.Property<int?>("mcc")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MccCodeCode");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("PFM.Database.Entities.SplitTransactions", b =>
                {
                    b.HasOne("PFM.Database.Entities.CategoryEntity", "Categories")
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PFM.Database.Entities.TransactionEntity", "Transactions")
                        .WithMany()
                        .HasForeignKey("TransactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("PFM.Database.Entities.TransactionEntity", b =>
                {
                    b.HasOne("PFM.Database.Entities.MccCodes", "MccCode")
                        .WithMany()
                        .HasForeignKey("MccCodeCode");

                    b.Navigation("MccCode");
                });
#pragma warning restore 612, 618
        }
    }
}
