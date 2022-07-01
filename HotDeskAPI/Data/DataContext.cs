using HotDeskAPI.Entities;
using HotDeskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDeskAPI.Data;

public class DataContext : DbContext
{
    public DataContext() {}

    public DataContext(DbContextOptions<DataContext> options) : base(options) {}
    
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Desk> Desks { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
}