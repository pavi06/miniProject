using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace HotelBookingSystemAPI.Contexts
{
    public class HotelBookingContext : DbContext
    {
        public HotelBookingContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Person> Person { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookedRooms> BookedRooms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Discount> Discounts { get; set; }      



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
            .Entity<RoomType>()
            .Property(rt => rt.Type)
            .HasConversion(
                t => t.ToString(), 
                t => (RoomTypes)Enum.ToObject(typeof(RoomTypes), t));

            modelBuilder.Entity<BookedRooms>().HasKey(br => new { br.BookingId, br.RoomId });

            modelBuilder.Entity<Discount>().HasKey(d => new { d.HotelId, d.RoomTypeId });

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
               .WithMany(r => r.roomBookings)
               .HasForeignKey(br => br.RoomId);

            modelBuilder.Entity<Booking>()
               .HasOne(b => b.Guest)
               .WithMany(p => p.bookings)
               .HasForeignKey(b => b.GuestId);

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

        }
    }
}
