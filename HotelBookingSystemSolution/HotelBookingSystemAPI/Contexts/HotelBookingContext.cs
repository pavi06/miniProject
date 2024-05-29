using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace HotelBookingSystemAPI.Contexts
{
    public class HotelBookingContext : DbContext
    {
        public HotelBookingContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HotelEmployee> Employees { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookedRooms> BookedRooms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<HotelAvailabilityByDate> HotelAvailabilityByDates { get; set; }      



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Guest>()
                .HasIndex(g => g.Email)
                .IsUnique();

            modelBuilder.Entity<HotelEmployee>()
                .HasIndex(e => e.Email)
                .IsUnique();


            modelBuilder.Entity<BookedRooms>().HasKey(br => new { br.BookingId, br.RoomId });

            modelBuilder.Entity<HotelAvailabilityByDate>().HasKey(ha => new { ha.HotelId, ha.Date });


            modelBuilder.Entity<Room>()
               .HasOne(r => r.Hotel)
               .WithMany(h => h.Rooms)
               .HasForeignKey(r => r.HotelId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Ratings)
                .HasForeignKey(r => r.HotelId)
                .IsRequired();

            modelBuilder.Entity<BookedRooms>()
               .HasOne(br => br.Booking)
               .WithMany(b => b.RoomsBooked)
               .HasForeignKey(br => br.BookingId);

            modelBuilder.Entity<BookedRooms>()
               .HasOne(br => br.Room)
               .WithMany(r => r.roomsBooked)
               .HasForeignKey(br => br.RoomId);

            modelBuilder.Entity<Booking>()
               .HasOne(b => b.Guest)
               .WithMany(p => p.bookings)
               .HasForeignKey(b => b.GuestId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Booking>()
               .HasOne(b => b.Hotel)
               .WithMany(h => h.bookingsForHotel)
               .HasForeignKey(b => b.HotelId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<RoomType>()
              .HasOne(r => r.Hotel)
              .WithMany(h => h.RoomTypes)
              .HasForeignKey(r => r.HotelId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Payment>()
              .HasOne(p => p.Book)
              .WithMany(b => b.Payments)
              .HasForeignKey(p => p.BookId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<HotelAvailabilityByDate>()
              .HasOne(ha => ha.Hotel)
              .WithMany(h => h.hotelAvailabilityByDates)
              .HasForeignKey(ha => ha.HotelId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<HotelEmployee>()
                .HasOne(e => e.Hotel)
                .WithMany(h => h.employees)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
