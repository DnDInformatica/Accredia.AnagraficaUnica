using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GestioneOrganismi.Backend.Models;
using System;

namespace GestioneOrganismi.Backend.Data
{
    /// <summary>
    /// DbContext segmentato per il Bounded Context "Persone"
    /// Supporta SQL Server e PostgreSQL
    /// </summary>
    public class PersoneDbContext : DbContext
    {
        public PersoneDbContext(DbContextOptions<PersoneDbContext> options) : base(options) { }

        public DbSet<Persona> Persone { get; set; } = null!;
        public DbSet<EntitaAziendale> EntiAziendali { get; set; } = null!;
        public DbSet<Email> Email { get; set; } = null!;
        public DbSet<Telefono> Telefoni { get; set; } = null!;
        public DbSet<PersonaIndirizzo> PersonaIndirizzi { get; set; } = null!;
        public DbSet<EnteAccreditamento> EntiAccreditamento { get; set; } = null!;
        public DbSet<EntitaAnagraficaContatto> EntiAnagraficiContatti { get; set; } = null!;
        
        // Schema Organismi
        public DbSet<OrganismoAccreditato> OrganismiAccreditati { get; set; } = null!;
        
        // Schema Accreditamento
        public DbSet<AmbitoApplicazione> AmbitoApplicazione { get; set; } = null!;
        public DbSet<RilascioAccreditamento> RilascioAccreditamento { get; set; } = null!;
        public DbSet<Documento> Documenti { get; set; } = null!;
        
        // Schema Tipologica
        public DbSet<TipoEmail> TipiEmail { get; set; } = null!;
        public DbSet<TipoTelefono> TipiTelefono { get; set; } = null!;
        public DbSet<TipoIndirizzo> TipiIndirizzo { get; set; } = null!;
        public DbSet<TipoEnteAccreditamento> TipiEnteAccreditamento { get; set; } = null!;
        public DbSet<TitoloOnorifico> TitoliOnorifici { get; set; } = null!;
        
        // Schema RisorseUmane
        public DbSet<Dipendente> Dipendenti { get; set; } = null!;
        public DbSet<Dipartimento> Dipartimenti { get; set; } = null!;
        public DbSet<Reparto> Reparti { get; set; } = null!;
        public DbSet<Turno> Turni { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Persona Configuration
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.PersonaId);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Cognome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CodiceFiscale)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Genere)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RowGuid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(p => p.DataCancellazione == null);

                // Relationship
                entity.HasOne(e => e.EntitaAziendale)
                    .WithMany(ea => ea.Persone)
                    .HasForeignKey(e => e.EntitaAziendaleId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.CodiceFiscale).IsUnique();
                entity.HasIndex(e => new { e.Nome, e.Cognome });
                entity.HasIndex(e => e.EntitaAziendaleId);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // EntitaAziendale Configuration
            modelBuilder.Entity<EntitaAziendale>(entity =>
            {
                entity.HasKey(e => e.EntitaAziendaleId);

                entity.Property(e => e.Denominazione)
                    .HasMaxLength(500);

                entity.Property(e => e.CodiceFiscale)
                    .HasMaxLength(20);

                entity.Property(e => e.PartitaIVA)
                    .HasMaxLength(20);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RowGuid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(ea => ea.DataCancellazione == null);

                // Indexes
                entity.HasIndex(e => e.CodiceFiscale).IsUnique();
                entity.HasIndex(e => e.DataCancellazione);
            });

            // Email Configuration
            modelBuilder.Entity<Email>(entity =>
            {
                entity.HasKey(e => e.EmailId);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(510)
                    .HasColumnName("Email");

                entity.Property(e => e.EntitaAziendaleId)
                    .IsRequired();

                entity.Property(e => e.TipoEmailId)
                    .IsRequired();

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RowGuid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(e => e.DataCancellazione == null);

                // Relationships
                entity.HasOne(e => e.EntitaAziendale)
                    .WithMany()
                    .HasForeignKey(e => e.EntitaAziendaleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TipoEmail)
                    .WithMany()
                    .HasForeignKey(e => e.TipoEmailId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.EmailAddress);
                entity.HasIndex(e => e.EntitaAziendaleId);
                entity.HasIndex(e => e.TipoEmailId);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // Telefono Configuration
            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.HasKey(e => e.TelefonoId);

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PrefissoInternazionale)
                    .HasMaxLength(20);

                entity.Property(e => e.Estensione)
                    .HasMaxLength(20);

                entity.Property(e => e.EntitaAziendaleId)
                    .IsRequired();

                entity.Property(e => e.TipoTelefonoId)
                    .IsRequired();

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RowGuid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(t => t.DataCancellazione == null);

                // Relationships
                entity.HasOne(t => t.EntitaAziendale)
                    .WithMany()
                    .HasForeignKey(t => t.EntitaAziendaleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.TipoTelefono)
                    .WithMany()
                    .HasForeignKey(t => t.TipoTelefonoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.Numero);
                entity.HasIndex(e => e.EntitaAziendaleId);
                entity.HasIndex(e => e.TipoTelefonoId);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // PersonaIndirizzo Configuration
            modelBuilder.Entity<PersonaIndirizzo>(entity =>
            {
                entity.HasKey(e => e.PersonaIndirizzoId);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(pi => pi.DataCancellazione == null);

                // Indexes
                entity.HasIndex(e => e.PersonaId);
                entity.HasIndex(e => e.Attivo);
            });

            // AmbitoApplicazione Configuration
            modelBuilder.Entity<AmbitoApplicazione>(entity =>
            {
                entity.HasKey(e => e.AmbitoApplicazioneId);

                entity.Property(e => e.Codice)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Denominazione)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Descrizione)
                    .HasMaxLength(1000);

                entity.Property(e => e.Ordine)
                    .IsRequired();

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RowGuid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(a => a.DataCancellazione == null);

                // Indexes
                entity.HasIndex(e => e.Codice).IsUnique();
                entity.HasIndex(e => e.Ordine);
                entity.HasIndex(e => e.Attivo);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // RilascioAccreditamento Configuration
            modelBuilder.Entity<RilascioAccreditamento>(entity =>
            {
                entity.HasKey(e => e.RilascioId);

                entity.Property(e => e.NumeroAtto)
                    .HasMaxLength(200);

                entity.Property(e => e.Stato)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DataRilascio)
                    .IsRequired();

                // Relationships
                entity.HasOne(e => e.AmbitoApplicazione)
                    .WithMany()
                    .HasForeignKey(e => e.AmbitoApplicazioneId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EnteAccreditamento)
                    .WithMany()
                    .HasForeignKey(e => e.EnteAccreditamentoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EnteAccreditato)
                    .WithMany()
                    .HasForeignKey(e => e.EnteAccreditatoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.NumeroAtto);
                entity.HasIndex(e => e.DataRilascio);
                entity.HasIndex(e => e.Stato);
                entity.HasIndex(e => e.AmbitoApplicazioneId);
            });
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => 
                    e.Entity is Persona ||
                    e.Entity is EntitaAziendale ||
                    e.Entity is Email ||
                    e.Entity is Telefono ||
                    e.Entity is PersonaIndirizzo ||
                    e.Entity is AmbitoApplicazione);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is Persona persona)
                    {
                        persona.DataCreazione = DateTime.UtcNow;
                        persona.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is EntitaAziendale entita)
                    {
                        entita.DataCreazione = DateTime.UtcNow;
                        entita.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is Email email)
                    {
                        email.DataCreazione = DateTime.UtcNow;
                        email.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is Telefono telefono)
                    {
                        telefono.DataCreazione = DateTime.UtcNow;
                        telefono.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is PersonaIndirizzo indirizzo)
                    {
                        indirizzo.DataCreazione = DateTime.UtcNow;
                        indirizzo.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is AmbitoApplicazione ambito)
                    {
                        ambito.DataCreazione = DateTime.UtcNow;
                        ambito.DataInizioValidita = DateTime.UtcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    switch (entry.Entity)
                    {
                        case Persona p:
                            p.DataModifica = DateTime.UtcNow;
                            break;
                        case EntitaAziendale ea:
                            ea.DataModifica = DateTime.UtcNow;
                            break;
                        case Email e:
                            e.DataModifica = DateTime.UtcNow;
                            break;
                        case Telefono t:
                            t.DataModifica = DateTime.UtcNow;
                            break;
                        case PersonaIndirizzo pi:
                            pi.DataModifica = DateTime.UtcNow;
                            break;
                        case AmbitoApplicazione a:
                            a.DataModifica = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Design-time factory per le migration (da usare in Package Manager Console)
    /// </summary>
    public class PersoneDbContextFactory : IDesignTimeDbContextFactory<PersoneDbContext>
    {
        public PersoneDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersoneDbContext>();

            // Di default usa SQL Server. Per PostgreSQL, commentare la riga sotto e scommentare PostgreSQL
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=GestioneOrganismi.Persone;Integrated Security=true;TrustServerCertificate=true;",
                sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")
            );

            // Per PostgreSQL (decommenta se usi PostgreSQL)
            // optionsBuilder.UseNpgsql(
            //     "Host=localhost;Database=GestioneOrganismi.Persone;Username=postgres;Password=password;",
            //     npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", "public")
            // );

            return new PersoneDbContext(optionsBuilder.Options);
        }
    }
}
