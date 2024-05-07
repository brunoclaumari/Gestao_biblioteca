using System;
using System.Collections.Generic;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestaoBiblioteca.Context;

public partial class GestaoBibliotecaContext : DbContext
{
    public GestaoBibliotecaContext()
    {
    }

    public GestaoBibliotecaContext(DbContextOptions<GestaoBibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Emprestimo> Emprestimos { get; set; }

    public virtual DbSet<ItensEmprestimo> ItensEmprestimos { get; set; }

    public virtual DbSet<Livro> Livros { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.\\BRUNO;Database=GESTAO_BIBLIOTECA;User=sa;Password=1bruno*;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.ToTable("tbEmprestimo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataDevolucao)
                .HasColumnType("datetime")
                .HasColumnName("data_devolucao");
            entity.Property(e => e.DataEmprestimo)
                .HasColumnType("datetime")
                .HasColumnName("data_emprestimo");


            entity.Property(e => e.StatusEmprestimo)
            .HasColumnName("status_emprestimo")
            .HasDefaultValue(EnumEmprestimoStatus.EmAberto)            
            .HasAnnotation("Relational:DefaultConstraintName", "DF_status"); ;
            

            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Emprestimos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbEmprestimo_tbUsuario");
            
            
        });

        modelBuilder.Entity<ItensEmprestimo>(entity =>
        {
            entity.ToTable("tbItensEmprestimo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmprestimoId).HasColumnName("emprestimo_id");
            entity.Property(e => e.LivroId).HasColumnName("livro_id");

            entity.HasOne(d => d.Emprestimo).WithMany(p => p.ItensEmprestimos)
                .HasForeignKey(d => d.EmprestimoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbItensEmprestimo_tbEmprestimo");

            entity.HasOne(d => d.Livro).WithMany(p => p.ItensEmprestimos)
                .HasForeignKey(d => d.LivroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbItensEmprestimo_tbLivro");
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.ToTable("tbLivro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Autores)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("autores");
            entity.Property(e => e.Genero).HasColumnName("genero");
            entity.Property(e => e.QuantidadeTotal).HasColumnName("quantidade_total");
            entity.Property(e => e.Titulo)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("tbUsuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataAtualizacao)
                .HasColumnType("datetime")
                .HasColumnName("data_atualizacao");
            entity.Property(e => e.DataRegistro)
                .HasColumnType("datetime")
                .HasColumnName("data_registro");
            entity.Property(e => e.Endereco)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("endereco");
            entity.Property(e => e.Nome)
                .IsUnicode(false)
                .HasColumnName("nome");

            entity.Property(e => e.PossuiPendencias)
            .HasDefaultValue(false)
            //.HasDefaultValueSql("(CONVERT([bit],(0)))")
            .HasAnnotation("Relational:DefaultConstraintName", "DF_possui_pendencias");
        });

        // Definindo ação de exclusão em cascata para tbItensEmprestimo
        modelBuilder.Entity<ItensEmprestimo>()
            .HasOne(ie => ie.Emprestimo)
            .WithMany(e => e.ItensEmprestimos)
            .OnDelete(DeleteBehavior.Cascade);

        // Definindo ação de exclusão em cascata para tbEmprestimo
        modelBuilder.Entity<Emprestimo>()
            .HasMany(e => e.ItensEmprestimos)
            .WithOne(ie => ie.Emprestimo)
            .OnDelete(DeleteBehavior.Cascade);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
