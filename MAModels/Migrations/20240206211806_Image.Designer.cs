﻿// <auto-generated />
using System;
using MAModels.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MAModels.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240206211806_Image")]
    partial class Image
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"));

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ImageExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.HasKey("ImageId");

                    b.HasIndex("MovieId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MovieId"));

                    b.Property<bool>("IsForAdult")
                        .HasColumnType("bit");

                    b.Property<string>("MovieDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovieMaker")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovieTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("MovieYearProduction")
                        .HasColumnType("smallint");

                    b.HasKey("MovieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Preference", b =>
                {
                    b.Property<int>("ModelTrainId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ModelTrainId"));

                    b.Property<DateTime>("DateTimeCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("DescriptionVote")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovieDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovieGenres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<string>("MovieMaker")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovieTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("MovieYear")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Vote")
                        .HasColumnType("real");

                    b.HasKey("ModelTrainId");

                    b.ToTable("Preferencies");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<DateTime>("DateTimeVote")
                        .HasColumnType("datetime2");

                    b.Property<string>("DescriptionVote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<float>("Vote")
                        .HasColumnType("real");

                    b.HasKey("ReviewId");

                    b.HasIndex("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MovieTag", b =>
                {
                    b.Property<int>("MoviesListMovieId")
                        .HasColumnType("int");

                    b.Property<int>("TagsListTagId")
                        .HasColumnType("int");

                    b.HasKey("MoviesListMovieId", "TagsListTagId");

                    b.HasIndex("TagsListTagId");

                    b.ToTable("MovieTag");
                });

            modelBuilder.Entity("MovieUser", b =>
                {
                    b.Property<int>("MoviesListMovieId")
                        .HasColumnType("int");

                    b.Property<int>("UsersListUserId")
                        .HasColumnType("int");

                    b.HasKey("MoviesListMovieId", "UsersListUserId");

                    b.HasIndex("UsersListUserId");

                    b.ToTable("MovieUser");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Image", b =>
                {
                    b.HasOne("MAModels.EntityFrameworkModels.Movie", "Movie")
                        .WithMany("ImagesList")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Review", b =>
                {
                    b.HasOne("MAModels.EntityFrameworkModels.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MAModels.EntityFrameworkModels.User", "User")
                        .WithMany("ReviewsList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MovieTag", b =>
                {
                    b.HasOne("MAModels.EntityFrameworkModels.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesListMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MAModels.EntityFrameworkModels.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsListTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieUser", b =>
                {
                    b.HasOne("MAModels.EntityFrameworkModels.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesListMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MAModels.EntityFrameworkModels.User", null)
                        .WithMany()
                        .HasForeignKey("UsersListUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.Movie", b =>
                {
                    b.Navigation("ImagesList");
                });

            modelBuilder.Entity("MAModels.EntityFrameworkModels.User", b =>
                {
                    b.Navigation("ReviewsList");
                });
#pragma warning restore 612, 618
        }
    }
}