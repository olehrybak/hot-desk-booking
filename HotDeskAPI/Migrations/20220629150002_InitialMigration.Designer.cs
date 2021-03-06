// <auto-generated />
using HotDeskAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotDeskAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220629150002_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HotDeskAPI.Models.Desk", b =>
                {
                    b.Property<int>("DeskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeskId"), 1L, 1);

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.HasKey("DeskId");

                    b.HasIndex("LocationId");

                    b.ToTable("Desks");
                });

            modelBuilder.Entity("HotDeskAPI.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("HotDeskAPI.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"), 1L, 1);

                    b.Property<int>("DeskId")
                        .HasColumnType("int");

                    b.Property<int>("EndDate")
                        .HasColumnType("int");

                    b.Property<int>("StartDate")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.HasIndex("DeskId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("HotDeskAPI.Models.Desk", b =>
                {
                    b.HasOne("HotDeskAPI.Models.Location", "Location")
                        .WithMany("Desk")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("HotDeskAPI.Models.Reservation", b =>
                {
                    b.HasOne("HotDeskAPI.Models.Desk", "Desk")
                        .WithMany("Reservation")
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Desk");
                });

            modelBuilder.Entity("HotDeskAPI.Models.Desk", b =>
                {
                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("HotDeskAPI.Models.Location", b =>
                {
                    b.Navigation("Desk");
                });
#pragma warning restore 612, 618
        }
    }
}
