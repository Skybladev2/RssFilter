﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RssFilter.Models;

namespace RssFilter.Models.Migrations
{
    [DbContext(typeof(SQLiteDb))]
    [Migration("20210215214958_RemoveIsPublishedToVk")]
    partial class RemoveIsPublishedToVk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("RssFilter.Models.Feed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("BaseUrl")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastCheck")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("RssFilter.Models.Keyword", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("FeedId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("RssFilter.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("FeedId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Link")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("RssFilter.Models.Keyword", b =>
                {
                    b.HasOne("RssFilter.Models.Feed", "Feed")
                        .WithMany()
                        .HasForeignKey("FeedId");

                    b.Navigation("Feed");
                });

            modelBuilder.Entity("RssFilter.Models.Post", b =>
                {
                    b.HasOne("RssFilter.Models.Feed", "Feed")
                        .WithMany()
                        .HasForeignKey("FeedId");

                    b.Navigation("Feed");
                });
#pragma warning restore 612, 618
        }
    }
}
