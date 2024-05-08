﻿// <auto-generated />
using System;
using GestaoBiblioteca.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestaoBiblioteca.Migrations
{
    [DbContext(typeof(GestaoBibliotecaContext))]
    [Migration("20240508045136_Alterando_tbLivro")]
    partial class Alterando_tbLivro
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GestaoBiblioteca.Entities.Emprestimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataDevolucao")
                        .HasColumnType("datetime")
                        .HasColumnName("data_devolucao");

                    b.Property<DateTime>("DataEmprestimo")
                        .HasColumnType("datetime")
                        .HasColumnName("data_emprestimo");

                    b.Property<int>("StatusEmprestimo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("status_emprestimo")
                        .HasAnnotation("Relational:DefaultConstraintName", "DF_status");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("tbEmprestimo", (string)null);
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.ItensEmprestimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmprestimoId")
                        .HasColumnType("int")
                        .HasColumnName("emprestimo_id");

                    b.Property<int>("LivroId")
                        .HasColumnType("int")
                        .HasColumnName("livro_id");

                    b.HasKey("Id");

                    b.HasIndex("EmprestimoId");

                    b.HasIndex("LivroId");

                    b.ToTable("tbItensEmprestimo", (string)null);
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.Livro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Autores")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("autores");

                    b.Property<int>("Genero")
                        .HasColumnType("int")
                        .HasColumnName("genero");

                    b.Property<int>("QuantidadeEmprestada")
                        .HasColumnType("int")
                        .HasColumnName("quantidade_emprestada");

                    b.Property<int>("QuantidadeTotal")
                        .HasColumnType("int")
                        .HasColumnName("quantidade_total");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("titulo");

                    b.HasKey("Id");

                    b.ToTable("tbLivro", (string)null);
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("datetime")
                        .HasColumnName("data_atualizacao");

                    b.Property<DateTime>("DataRegistro")
                        .HasColumnType("datetime")
                        .HasColumnName("data_registro");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("endereco");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nome");

                    b.Property<bool>("PossuiPendencias")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("possui_pendencias")
                        .HasAnnotation("Relational:DefaultConstraintName", "DF_possui_pendencias");

                    b.HasKey("Id");

                    b.ToTable("tbUsuario", (string)null);
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.Emprestimo", b =>
                {
                    b.HasOne("GestaoBiblioteca.Entities.Usuario", "Usuario")
                        .WithMany("Emprestimos")
                        .HasForeignKey("UsuarioId")
                        .IsRequired()
                        .HasConstraintName("FK_tbEmprestimo_tbUsuario");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.ItensEmprestimo", b =>
                {
                    b.HasOne("GestaoBiblioteca.Entities.Emprestimo", "Emprestimo")
                        .WithMany("ItensEmprestimos")
                        .HasForeignKey("EmprestimoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_tbItensEmprestimo_tbEmprestimo");

                    b.HasOne("GestaoBiblioteca.Entities.Livro", "Livro")
                        .WithMany("ItensEmprestimos")
                        .HasForeignKey("LivroId")
                        .IsRequired()
                        .HasConstraintName("FK_tbItensEmprestimo_tbLivro");

                    b.Navigation("Emprestimo");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.Emprestimo", b =>
                {
                    b.Navigation("ItensEmprestimos");
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.Livro", b =>
                {
                    b.Navigation("ItensEmprestimos");
                });

            modelBuilder.Entity("GestaoBiblioteca.Entities.Usuario", b =>
                {
                    b.Navigation("Emprestimos");
                });
#pragma warning restore 612, 618
        }
    }
}
