﻿using lochess.Areas.Identity.Data;
using lochess.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace lochess.Data
{
    public class LochessDbContext : LochessIdentityContext
    {
        public string ConnectionString { get; set; }

        public LochessDbContext(DbContextOptions<LochessDbContext> options) : base(options) 
        {
        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            base.OnModelCreating(modelBuilder);
        }

    }
}

