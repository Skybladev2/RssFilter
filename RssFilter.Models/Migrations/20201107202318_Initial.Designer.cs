// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RssFilter.Models;

namespace RssFilter.Models.Migrations
{
    [DbContext(typeof(PostgresDB))]
    [Migration("20201107202318_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("RssFilter.Models.Feed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("BaseUrl")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastCheck")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastItemId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PublicUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BaseUrl")
                        .IsUnique();

                    b.HasIndex("PublicUrl")
                        .IsUnique();

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("RssFilter.Models.Keyword", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid?>("FeedId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("RssFilter.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<Guid?>("FeedId")
                        .HasColumnType("uuid");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.HasIndex("Link")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("RssFilter.Models.Keyword", b =>
                {
                    b.HasOne("RssFilter.Models.Feed", "Feed")
                        .WithMany()
                        .HasForeignKey("FeedId");
                });

            modelBuilder.Entity("RssFilter.Models.Post", b =>
                {
                    b.HasOne("RssFilter.Models.Feed", "Feed")
                        .WithMany()
                        .HasForeignKey("FeedId");
                });
#pragma warning restore 612, 618
        }
    }
}
