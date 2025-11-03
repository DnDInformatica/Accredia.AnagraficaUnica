using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Accredia.GestioneAnagrafica.API.Models;
using System;

namespace Accredia.GestioneAnagrafica.API.Data
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
                entity.ToTable("Persona", schema: "Persone");
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
                entity.ToTable("EntitaAziendale", schema: "Persone");
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
                entity.ToTable("Email", schema: "Persone");
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
                entity.ToTable("Telefono", schema: "Persone");
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
                entity.ToTable("PersonaIndirizzo", schema: "Persone");
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
                entity.ToTable("AmbitoApplicazione", schema: "Accreditamento");
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

            // EnteAccreditamento Configuration
            modelBuilder.Entity<EnteAccreditamento>(entity =>
            {
                entity.ToTable("EnteAccreditamento", schema: "Organismi");
                
                entity.HasKey(e => e.EntitaAziendaleId);

                entity.Property(e => e.EntitaAziendaleId)
                    .IsRequired();

                entity.Property(e => e.Denominazione)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Sigla)
                    .HasMaxLength(50);

                entity.Property(e => e.Note)
                    .HasMaxLength(500);

                entity.Property(e => e.DataFondazione);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UniqueRowId)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(e => e.DataCancellazione == null);

                // Indexes
                entity.HasIndex(e => e.EntitaAziendaleId);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // ===== SCHEMA TIPOLOGICA =====

            // TipoEmail Configuration
            modelBuilder.Entity<TipoEmail>(entity =>
            {
                entity.ToTable("TipoEmail", schema: "Tipologica");
            });

            // TipoTelefono Configuration
            modelBuilder.Entity<TipoTelefono>(entity =>
            {
                entity.ToTable("TipoTelefono", schema: "Tipologica");
            });

            // TipoIndirizzo Configuration
            modelBuilder.Entity<TipoIndirizzo>(entity =>
            {
                entity.ToTable("TipoIndirizzo", schema: "Tipologica");
            });

            // TitoloOnorifico Configuration
            modelBuilder.Entity<TitoloOnorifico>(entity =>
            {
                entity.ToTable("TitoloOnorifico", schema: "Tipologica");
            });

            // TipoEnteAccreditamento Configuration
            modelBuilder.Entity<TipoEnteAccreditamento>(entity =>
            {
                entity.ToTable("TipoEnteAccreditamento", schema: "Tipologica");
            });

            // ===== SCHEMA ACCREDITAMENTO =====

            // RilascioAccreditamento Configuration
            modelBuilder.Entity<RilascioAccreditamento>(entity =>
            {
                entity.ToTable("RilascioAccreditamento", schema: "Accreditamento");
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

            // ===== SCHEMA ORGANISMI =====

            // OrganismoAccreditato Configuration
            modelBuilder.Entity<OrganismoAccreditato>(entity =>
            {
                entity.ToTable("OrganismoAccreditato", schema: "Organismi");
            });

            // EntitaAnagraficaContatto Configuration
            modelBuilder.Entity<EntitaAnagraficaContatto>(entity =>
            {
                entity.ToTable("EntitaAnagraficaContatto", schema: "Persone");
            });

            // Documento Configuration
            modelBuilder.Entity<Documento>(entity =>
            {
                entity.ToTable("Documento", schema: "Accreditamento");
            });

            // ===== SCHEMA RISORSE UMANE =====

            // Dipartimento Configuration
            modelBuilder.Entity<Dipartimento>(entity =>
            {
                entity.ToTable("Dipartimento", schema: "RisorseUmane");
                entity.HasKey(e => e.DipartimentoId);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(d => d.DataCancellazione == null);

                // Relationships
                entity.HasOne(d => d.DipartimentoPadre)
                    .WithMany(d => d.DipartimentiFiliali)
                    .HasForeignKey(d => d.DipartimentoPadreId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Dipartimento_Padre");

                // Indexes
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.DipartimentoPadreId);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // Reparto Configuration
            modelBuilder.Entity<Reparto>(entity =>
            {
                entity.ToTable("Reparto", schema: "RisorseUmane");
                entity.HasKey(e => e.RepartoId);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(r => r.DataCancellazione == null);

                // Relationships
                // Relazione con Dipartimento
                entity.HasOne(r => r.Dipartimento)
                    .WithMany(d => d.Reparti)
                    .HasForeignKey(r => r.DipartimentoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relazione con Manager (Dipendente) - Importante: configurare esplicitamente
                entity.HasOne(r => r.Manager)
                    .WithMany()
                    .HasForeignKey(r => r.ManagerDipendenteId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Reparto_Manager");

                // Relazione con Dipendenti
                entity.HasMany(r => r.Dipendenti)
                    .WithOne(d => d.Reparto)
                    .HasForeignKey(d => d.RepartoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.DipartimentoId);
                entity.HasIndex(e => e.ManagerDipendenteId);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // Turno Configuration
            modelBuilder.Entity<Turno>(entity =>
            {
                entity.ToTable("Turno", schema: "RisorseUmane");
                entity.HasKey(e => e.TurnoId);

                entity.Property(e => e.Descrizione)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OraInizio)
                    .IsRequired();

                entity.Property(e => e.OraFine)
                    .IsRequired();

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(t => t.DataCancellazione == null);

                // Relationships
                entity.HasMany(t => t.Dipendenti)
                    .WithOne(d => d.Turno)
                    .HasForeignKey(d => d.TurnoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.Descrizione);
                entity.HasIndex(e => e.DataCancellazione);
            });

            // Dipendente Configuration
            modelBuilder.Entity<Dipendente>(entity =>
            {
                entity.ToTable("Dipendente", schema: "RisorseUmane");
                entity.HasKey(e => e.DipendenteId);

                entity.Property(e => e.CodiceFiscale)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Matricola)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.LoginID)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Mansione)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DataCreazione)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Query Filter per Soft Delete
                entity.HasQueryFilter(d => d.DataCancellazione == null);

                // Relationships
                // Relazione con Reparto
                entity.HasOne(d => d.Reparto)
                    .WithMany(r => r.Dipendenti)
                    .HasForeignKey(d => d.RepartoId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Dipendente_Reparto");

                // Relazione con Turno
                entity.HasOne(d => d.Turno)
                    .WithMany(t => t.Dipendenti)
                    .HasForeignKey(d => d.TurnoId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Dipendente_Turno");

                // Indexes
                entity.HasIndex(e => e.CodiceFiscale).IsUnique();
                entity.HasIndex(e => e.Matricola).IsUnique();
                entity.HasIndex(e => e.RepartoId);
                entity.HasIndex(e => e.TurnoId);
                entity.HasIndex(e => e.DataCancellazione);
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
                    e.Entity is AmbitoApplicazione ||
                    e.Entity is Dipendente ||
                    e.Entity is Dipartimento ||
                    e.Entity is Reparto ||
                    e.Entity is Turno);

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
                    else if (entry.Entity is Dipendente dipendente)
                    {
                        dipendente.DataCreazione = DateTime.UtcNow;
                        dipendente.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is Dipartimento dipartimento)
                    {
                        dipartimento.DataCreazione = DateTime.UtcNow;
                        dipartimento.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is Reparto reparto)
                    {
                        reparto.DataCreazione = DateTime.UtcNow;
                        reparto.DataInizioValidita = DateTime.UtcNow;
                    }
                    else if (entry.Entity is Turno turno)
                    {
                        turno.DataCreazione = DateTime.UtcNow;
                        turno.DataInizioValidita = DateTime.UtcNow;
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
                        case Dipendente d:
                            d.DataModifica = DateTime.UtcNow;
                            break;
                        case Dipartimento dip:
                            dip.DataModifica = DateTime.UtcNow;
                            break;
                        case Reparto r:
                            r.DataModifica = DateTime.UtcNow;
                            break;
                        case Turno tur:
                            tur.DataModifica = DateTime.UtcNow;
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
