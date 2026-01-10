using Microsoft.EntityFrameworkCore;
using E_Gostinc.Models;
namespace E_Gostinc.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class ArtikelContext : IdentityDbContext<Uporabnik>
{

    public ArtikelContext(DbContextOptions<ArtikelContext> options) : base(options)
    {

    }

        public DbSet<Artikel> Artikel { get; set; }
        public DbSet<DobavniArtikel> DobavniArtikel { get; set; }
        public DbSet<Vrsta> Vrsta { get; set; }
        public DbSet<Skladisce> Skladisce { get; set; }
        public DbSet<Dobava> Dobava { get; set; }
        public DbSet<DobavaVSkladisce> DobavaVSkladisce { get; set; }
        public DbSet<ArtikelDobavniArtikelPovezava> ArtikelDobavniArtikelPovezava { get; set; }
        public DbSet<PrenosMedSkladisci> PrenosMedSkladisci { get; set; }
        public DbSet<Racun> Racun { get; set; }
        public DbSet<RacunArtikel> RacunArtikel { get; set; }
        public DbSet<IzdelekGreVn> IzdelekGreVn { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Racun>(entity =>
        {
            entity.HasKey(e => e.ID);
            
            entity.HasOne(e => e.Uporabnik)
            .WithMany(u => u.Racuni)
            .HasForeignKey(e => e.Izdal_uporabnik_id)
            .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<RacunArtikel>(entity =>
    {
        entity.HasKey(e => e.ID);
        
        entity.HasOne(e => e.Racun)
            .WithMany(r => r.RacunArtikli)
            .HasForeignKey(e => e.Racun_id)
            .OnDelete(DeleteBehavior.Cascade);
        
        entity.HasOne(e => e.Artikel)
            .WithMany()
            .HasForeignKey(e => e.Artikel_id)
            .OnDelete(DeleteBehavior.Restrict);
    });

        modelBuilder.Entity<ArtikelDobavniArtikelPovezava>(entity =>
        {
            entity.HasKey(e => e.ID);
            
            entity.HasOne(e => e.Artikel)
                .WithMany()
                .HasForeignKey(e => e.Artikel_id)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.DobavniArtikel)
                .WithMany()
                .HasForeignKey(e => e.DobavniArtikel_Id)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Dobava>(entity =>
        {
            entity.HasKey(e => e.ID);
        
            entity.HasOne(e => e.Uporabnik)
                .WithMany(u => u.Dobave)
                .HasForeignKey(e => e.Prevzel_uporabnik_id)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DobavaVSkladisce>()
            .HasKey(d => new { d.Skladisce_id, d.Dobava_id, d.DobavniArtikel_Id });

       
        modelBuilder.Entity<DobavaVSkladisce>()
            .HasOne(d => d.Skladisce)
            .WithMany(s => s.DobaveVSkladiscu)
            .HasForeignKey(d => d.Skladisce_id);


        modelBuilder.Entity<DobavaVSkladisce>()
            .HasOne(d => d.Dobava)
            .WithMany(dob => dob.Skladisca)
            .HasForeignKey(d => d.Dobava_id);

        modelBuilder.Entity<DobavaVSkladisce>()
            .HasOne(d => d.DobavniArtikel)
            .WithMany(a => a.DobaveVSkladiscu)
            .HasForeignKey(d => d.DobavniArtikel_Id);

      
        modelBuilder.Entity<IzdelekGreVn>()
            .HasKey(i => new { i.Skladisce_id, i.Racun_id, i.Artikel_id });

        modelBuilder.Entity<IzdelekGreVn>()
            .HasOne(i => i.Skladisce)
            .WithMany(s => s.IzdelekiGrejoVn)
            .HasForeignKey(i => i.Skladisce_id);

        modelBuilder.Entity<IzdelekGreVn>()
            .HasOne(i => i.Racun)
            .WithMany(r => r.IzdelekiGrejoVn)
            .HasForeignKey(i => i.Racun_id);

        modelBuilder.Entity<IzdelekGreVn>()
            .HasOne(i => i.Artikel)
            .WithMany(a => a.IzdelekiGrejoVn)
            .HasForeignKey(i => i.Artikel_id);

    modelBuilder.Entity<PrenosMedSkladisci>(entity =>
    {
        entity.HasKey(e => e.ID);
        entity.ToTable("PrenosMedSkladisci");
        
        entity.HasOne(e => e.IzSkladisca)
            .WithMany()
            .HasForeignKey(e => e.IzSkladisca_id)
            .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(e => e.VSkladisce)
            .WithMany()
            .HasForeignKey(e => e.VSkladisce_id)
            .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(e => e.DobavniArtikel)
            .WithMany()
            .HasForeignKey(e => e.DobavniArtikel_Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(e => e.Uporabnik)
            .WithMany()
            .HasForeignKey(e => e.Uporabnik_id)
            .OnDelete(DeleteBehavior.Restrict);
    });

    }
}
