﻿using Microsoft.EntityFrameworkCore;
using AppLidra.Shared.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}
