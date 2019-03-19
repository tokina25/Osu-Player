﻿using Milky.OsuPlayer.Common.Data.EF.Model;
using Milky.OsuPlayer.Common.Migrations;
using osu_database_reader.Components.Beatmaps;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;
using OSharp.Beatmap.MetaData;

namespace Milky.OsuPlayer.Common.Data.EF
{
    public class BeatmapDbContext : DbContext
    {
        static BeatmapDbContext()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<BeatmapDbContext, BeatmapMigrationConfiguration>(true));
        }

        public DbSet<Beatmap> Beatmaps { get; set; }

        public BeatmapDbContext() : base("name=beatmapDb")
        {
            //Database.Initialize(false);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions
            //    .Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new BeatmapMap());
            base.OnModelCreating(modelBuilder);
        }
    }

    internal class MapIdentifiable : IEqualityComparer<IMapIdentifiable>
    {
        public bool Equals(IMapIdentifiable x, IMapIdentifiable y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.GetIdentity().Equals(y.GetIdentity());
        }

        public int GetHashCode(IMapIdentifiable obj)
        {
            return obj.FolderName.GetHashCode() + obj.FolderName.GetHashCode();
        }
    }

    public class BeatmapMap : EntityTypeConfiguration<Beatmap>
    {
        public BeatmapMap()
        {
            this.ToTable("beatmap");
            this.HasKey(m => m.Id);
        }
    }
}
