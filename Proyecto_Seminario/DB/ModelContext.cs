using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Proyecto_Seminario
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Acciones> Acciones { get; set; }
        public virtual DbSet<Instanciasplantillas> Instanciasplantillas { get; set; }
        public virtual DbSet<InstanciasplantillasDatosDetalle> InstanciasplantillasDatosDetalle { get; set; }
        public virtual DbSet<InstanciasplantillasPasosDetalle> InstanciasplantillasPasosDetalle { get; set; }
        public virtual DbSet<Pasos> Pasos { get; set; }
        public virtual DbSet<PasosDatosDetalle> PasosDatosDetalle { get; set; }
        public virtual DbSet<PasosUsuariosDetalle> PasosUsuariosDetalle { get; set; }
        public virtual DbSet<Pasosinstancias> Pasosinstancias { get; set; }
        public virtual DbSet<PasosinstanciasDatosDetalle> PasosinstanciasDatosDetalle { get; set; }
        public virtual DbSet<PasosinstanciasUsuariosDetalle> PasosinstanciasUsuariosDetalle { get; set; }
        public virtual DbSet<Plantillas> Plantillas { get; set; }
        public virtual DbSet<PlantillasCamposDetalle> PlantillasCamposDetalle { get; set; }
        public virtual DbSet<PlantillasPasosDetalle> PlantillasPasosDetalle { get; set; }
        public virtual DbSet<Rangos> Rangos { get; set; }
        public virtual DbSet<TiposDatos> TiposDatos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("User Id=c##proyecto_seminario;Password=hola;Data Source=localhost:1521/xe;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:DefaultSchema", "C##PROYECTO_SEMINARIO");

            modelBuilder.Entity<Acciones>(entity =>
            {
                entity.HasKey(e => e.IdAccion)
                    .HasName("ACCIONES_PK");

                entity.ToTable("ACCIONES");

                entity.HasIndex(e => e.IdAccion)
                    .HasName("ACCIONES_PK")
                    .IsUnique();

                entity.Property(e => e.IdAccion)
                    .HasColumnName("ID_ACCION")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(30)");
            });

            modelBuilder.Entity<Instanciasplantillas>(entity =>
            {
                entity.HasKey(e => e.IdInstanciaPlantilla)
                    .HasName("INSTANCIASPLANTILLAS_PK");

                entity.ToTable("INSTANCIASPLANTILLAS");

                entity.HasIndex(e => e.IdInstanciaPlantilla)
                    .HasName("INSTANCIASPLANTILLAS_PK")
                    .IsUnique();

                entity.Property(e => e.IdInstanciaPlantilla)
                    .HasColumnName("ID_INSTANCIA_PLANTILLA")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("ESTADO")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.Iniciada)
                    .IsRequired()
                    .HasColumnName("INICIADA")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Usuario)
                    .HasColumnName("USUARIO")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Instanciasplantillas)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INSTANCIASPLANTILLAS_FK1");
            });

            modelBuilder.Entity<InstanciasplantillasDatosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdInstanciaPlantillaDato)
                    .HasName("INSTANCIASPLANTILLAS_DATOS_DETALLE_PK");

                entity.ToTable("INSTANCIASPLANTILLAS_DATOS_DETALLE");

                entity.HasIndex(e => e.IdInstanciaPlantillaDato)
                    .HasName("INSTANCIASPLANTILLAS_DATOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdInstanciaPlantillaDato)
                    .HasColumnName("ID_INSTANCIA_PLANTILLA_DATO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DatoDate)
                    .HasColumnName("DATO_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatoInteger)
                    .HasColumnName("DATO_INTEGER")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.DatoString)
                    .HasColumnName("DATO_STRING")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Instanciaplantilla)
                    .HasColumnName("INSTANCIAPLANTILLA")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.NombreCampo)
                    .IsRequired()
                    .HasColumnName("NOMBRE_CAMPO")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.TipoDato)
                    .HasColumnName("TIPO_DATO")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.InstanciaplantillaNavigation)
                    .WithMany(p => p.InstanciasplantillasDatosDetalle)
                    .HasForeignKey(d => d.Instanciaplantilla)
                    .HasConstraintName("INSTANCIASPLANTILLAS_DATOS_DETALLE_FK1");

                entity.HasOne(d => d.TipoDatoNavigation)
                    .WithMany(p => p.InstanciasplantillasDatosDetalle)
                    .HasForeignKey(d => d.TipoDato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INSTANCIASPLANTILLAS_DATOS_DETALLE_FK2");
            });

            modelBuilder.Entity<InstanciasplantillasPasosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPlantillaPasoDetalle)
                    .HasName("INSTANCIASPLANTILLAS_PASOS_DETALLE_PK");

                entity.ToTable("INSTANCIASPLANTILLAS_PASOS_DETALLE");

                entity.HasIndex(e => e.IdPlantillaPasoDetalle)
                    .HasName("INSTANCIASPLANTILLAS_PASOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdPlantillaPasoDetalle)
                    .HasColumnName("ID_PLANTILLA_PASO_DETALLE")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Estado)
                    .HasColumnName("ESTADO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.FechaFin)
                    .HasColumnName("FECHA_FIN")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaInicio)
                    .HasColumnName("FECHA_INICIO")
                    .HasColumnType("DATE");

                entity.Property(e => e.InstanciaPlantilla)
                    .HasColumnName("INSTANCIA_PLANTILLA")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Paso)
                    .HasColumnName("PASO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.UsuarioAccion)
                    .HasColumnName("USUARIO_ACCION")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.InstanciasplantillasPasosDetalle)
                    .HasForeignKey(d => d.Estado)
                    .HasConstraintName("INSTANCIASPLANTILLAS_PASOS_DETALLE_FK2");

                entity.HasOne(d => d.InstanciaPlantillaNavigation)
                    .WithMany(p => p.InstanciasplantillasPasosDetalle)
                    .HasForeignKey(d => d.InstanciaPlantilla)
                    .HasConstraintName("INSTANCIASPLANTILLAS_PASOS_DETALLE_FK3");

                entity.HasOne(d => d.PasoNavigation)
                    .WithMany(p => p.InstanciasplantillasPasosDetalle)
                    .HasForeignKey(d => d.Paso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INSTANCIASPLANTILLAS_PASOS_DETALLE_FK1");

                entity.HasOne(d => d.UsuarioAccionNavigation)
                    .WithMany(p => p.InstanciasplantillasPasosDetalle)
                    .HasForeignKey(d => d.UsuarioAccion)
                    .HasConstraintName("INSTANCIASPLANTILLAS_PASOS_DETALLE_FK4");
            });

            modelBuilder.Entity<Pasos>(entity =>
            {
                entity.HasKey(e => e.IdPaso)
                    .HasName("PASOS_PK");

                entity.ToTable("PASOS");

                entity.HasIndex(e => e.IdPaso)
                    .HasName("PASOS_PK")
                    .IsUnique();

                entity.Property(e => e.IdPaso)
                    .HasColumnName("ID_PASO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");
            });

            modelBuilder.Entity<PasosDatosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPasoDato)
                    .HasName("PASOS_DATOS_DETALLE_PK");

                entity.ToTable("PASOS_DATOS_DETALLE");

                entity.HasIndex(e => e.IdPasoDato)
                    .HasName("PASOS_DATOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdPasoDato)
                    .HasColumnName("ID_PASO_DATO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Paso)
                    .HasColumnName("PASO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.PlantillaCampo)
                    .HasColumnName("PLANTILLA_CAMPO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.SoloLectura)
                    .IsRequired()
                    .HasColumnName("SOLO_LECTURA")
                    .HasColumnType("CHAR(1)");

                entity.HasOne(d => d.PasoNavigation)
                    .WithMany(p => p.PasosDatosDetalle)
                    .HasForeignKey(d => d.Paso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PASOS_DATOS_DETALLE_FK1");

                entity.HasOne(d => d.PlantillaCampoNavigation)
                    .WithMany(p => p.PasosDatosDetalle)
                    .HasForeignKey(d => d.PlantillaCampo)
                    .HasConstraintName("PASOS_DATOS_DETALLE_FK2");
            });

            modelBuilder.Entity<PasosUsuariosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPasoUsuario)
                    .HasName("PLANTILLAS_PASOS_USUARIOS_DETALLE_PK2");

                entity.ToTable("PASOS_USUARIOS_DETALLE");

                entity.HasIndex(e => e.IdPasoUsuario)
                    .HasName("PASOS_USUARIOS_DETALLE_PK2")
                    .IsUnique();

                entity.Property(e => e.IdPasoUsuario)
                    .HasColumnName("ID_PASO_USUARIO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PlantillaPasoDetalle)
                    .HasColumnName("PLANTILLA_PASO_DETALLE")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Usuario)
                    .HasColumnName("USUARIO")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.PlantillaPasoDetalleNavigation)
                    .WithMany(p => p.PasosUsuariosDetalle)
                    .HasForeignKey(d => d.PlantillaPasoDetalle)
                    .HasConstraintName("PLANTILLAS_PASOS_USUARIOS_DETALLE_FK1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.PasosUsuariosDetalle)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PLANTILLAS_PASOS_USUARIOS_DETALLE_FK2");
            });

            modelBuilder.Entity<Pasosinstancias>(entity =>
            {
                entity.HasKey(e => e.IdPasoinstancia)
                    .HasName("PASOSINSTANCIAS_PK");

                entity.ToTable("PASOSINSTANCIAS");

                entity.HasIndex(e => e.IdPasoinstancia)
                    .HasName("PASOSINSTANCIAS_PK")
                    .IsUnique();

                entity.Property(e => e.IdPasoinstancia)
                    .HasColumnName("ID_PASOINSTANCIA")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");
            });

            modelBuilder.Entity<PasosinstanciasDatosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPasosinstanciasDatos)
                    .HasName("PASOSINSTANCIAS_DATOS_DETALLE_PK");

                entity.ToTable("PASOSINSTANCIAS_DATOS_DETALLE");

                entity.HasIndex(e => e.IdPasosinstanciasDatos)
                    .HasName("PASOSINSTANCIAS_DATOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdPasosinstanciasDatos)
                    .HasColumnName("ID_PASOSINSTANCIAS_DATOS")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.InstanciaPlantillaDato)
                    .HasColumnName("INSTANCIA_PLANTILLA_DATO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Paso)
                    .HasColumnName("PASO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.SoloLectura)
                    .IsRequired()
                    .HasColumnName("SOLO_LECTURA")
                    .HasColumnType("CHAR(1)");

                entity.HasOne(d => d.InstanciaPlantillaDatoNavigation)
                    .WithMany(p => p.PasosinstanciasDatosDetalle)
                    .HasForeignKey(d => d.InstanciaPlantillaDato)
                    .HasConstraintName("PASOSINSTANCIAS_DATOS_DETALLE_FK1");

                entity.HasOne(d => d.PasoNavigation)
                    .WithMany(p => p.PasosinstanciasDatosDetalle)
                    .HasForeignKey(d => d.Paso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PASOSINSTANCIAS_DATOS_DETALLE_FK2");
            });

            modelBuilder.Entity<PasosinstanciasUsuariosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPasosUsuarios)
                    .HasName("PASOS_USUARIOS_DETALLE_PK");

                entity.ToTable("PASOSINSTANCIAS_USUARIOS_DETALLE");

                entity.HasIndex(e => e.IdPasosUsuarios)
                    .HasName("PASOS_USUARIOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdPasosUsuarios)
                    .HasColumnName("ID_PASOS_USUARIOS")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PlantillaPasoDetalle)
                    .HasColumnName("PLANTILLA_PASO_DETALLE")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Usuario)
                    .HasColumnName("USUARIO")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.PlantillaPasoDetalleNavigation)
                    .WithMany(p => p.PasosinstanciasUsuariosDetalle)
                    .HasForeignKey(d => d.PlantillaPasoDetalle)
                    .HasConstraintName("PASOS_USUARIOS_DETALLE_FK1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.PasosinstanciasUsuariosDetalle)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PASOS_USUARIOS_DETALLE_FK2");
            });

            modelBuilder.Entity<Plantillas>(entity =>
            {
                entity.HasKey(e => e.IdPlantilla)
                    .HasName("PLANTILLAS_PK");

                entity.ToTable("PLANTILLAS");

                entity.HasIndex(e => e.IdPlantilla)
                    .HasName("PLANTILLAS_PK")
                    .IsUnique();

                entity.Property(e => e.IdPlantilla)
                    .HasColumnName("ID_PLANTILLA")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");
            });

            modelBuilder.Entity<PlantillasCamposDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPlantillaCampo)
                    .HasName("PLANTILLAS_CAMPOS_DETALLE_PK");

                entity.ToTable("PLANTILLAS_CAMPOS_DETALLE");

                entity.HasIndex(e => e.IdPlantillaCampo)
                    .HasName("PLANTILLAS_CAMPOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdPlantillaCampo)
                    .HasColumnName("ID_PLANTILLA_CAMPO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreCampo)
                    .IsRequired()
                    .HasColumnName("NOMBRE_CAMPO")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Plantilla)
                    .HasColumnName("PLANTILLA")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.TipoDato)
                    .HasColumnName("TIPO_DATO")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.PlantillaNavigation)
                    .WithMany(p => p.PlantillasCamposDetalle)
                    .HasForeignKey(d => d.Plantilla)
                    .HasConstraintName("PLANTILLAS_CAMPOS_DETALLE_FK1");

                entity.HasOne(d => d.TipoDatoNavigation)
                    .WithMany(p => p.PlantillasCamposDetalle)
                    .HasForeignKey(d => d.TipoDato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PLANTILLAS_CAMPOS_DETALLE_FK2");
            });

            modelBuilder.Entity<PlantillasPasosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPlantillaPaso)
                    .HasName("PLANTILLAS_PASOS_DETALLE_PK");

                entity.ToTable("PLANTILLAS_PASOS_DETALLE");

                entity.HasIndex(e => e.IdPlantillaPaso)
                    .HasName("PLANTILLAS_PASOS_DETALLE_PK")
                    .IsUnique();

                entity.Property(e => e.IdPlantillaPaso)
                    .HasColumnName("ID_PLANTILLA_PASO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Paso)
                    .HasColumnName("PASO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Plantilla)
                    .HasColumnName("PLANTILLA")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.PasoNavigation)
                    .WithMany(p => p.PlantillasPasosDetalle)
                    .HasForeignKey(d => d.Paso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PLANTILLAS_PASOS_DETALLE_FK2");

                entity.HasOne(d => d.PlantillaNavigation)
                    .WithMany(p => p.PlantillasPasosDetalle)
                    .HasForeignKey(d => d.Plantilla)
                    .HasConstraintName("PLANTILLAS_PASOS_DETALLE_FK1");
            });

            modelBuilder.Entity<Rangos>(entity =>
            {
                entity.HasKey(e => e.IdRango)
                    .HasName("RANGOS_PK");

                entity.ToTable("RANGOS");

                entity.HasIndex(e => e.IdRango)
                    .HasName("RANGOS_PK")
                    .IsUnique();

                entity.Property(e => e.IdRango)
                    .HasColumnName("ID_RANGO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nivel)
                    .HasColumnName("NIVEL")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");
            });

            modelBuilder.Entity<TiposDatos>(entity =>
            {
                entity.HasKey(e => e.IdTipoDato)
                    .HasName("TABLE1_PK");

                entity.ToTable("TIPOS_DATOS");

                entity.HasIndex(e => e.IdTipoDato)
                    .HasName("TABLE1_PK")
                    .IsUnique();

                entity.Property(e => e.IdTipoDato)
                    .HasColumnName("ID_TIPO_DATO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("USUARIOS_PK");

                entity.ToTable("USUARIOS");

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("USUARIOS_PK")
                    .IsUnique();

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("ID_USUARIO")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("APELLIDOS")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("NOMBRES")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Rango)
                    .HasColumnName("RANGO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.UsuarioEmail)
                    .IsRequired()
                    .HasColumnName("USUARIO_EMAIL")
                    .HasColumnType("VARCHAR2(30)");

                entity.HasOne(d => d.RangoNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Rango)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USUARIOS_FK1");
            });

            modelBuilder.HasSequence("ISEQ$$_75348");

            modelBuilder.HasSequence("ISEQ$$_75351");

            modelBuilder.HasSequence("ISEQ$$_75354");

            modelBuilder.HasSequence("ISEQ$$_75357");

            modelBuilder.HasSequence("ISEQ$$_75360");

            modelBuilder.HasSequence("ISEQ$$_75363");

            modelBuilder.HasSequence("ISEQ$$_75366");

            modelBuilder.HasSequence("ISEQ$$_75368");

            modelBuilder.HasSequence("ISEQ$$_75371");

            modelBuilder.HasSequence("ISEQ$$_75373");

            modelBuilder.HasSequence("ISEQ$$_75375");

            modelBuilder.HasSequence("ISEQ$$_75378");

            modelBuilder.HasSequence("ISEQ$$_75379");

            modelBuilder.HasSequence("ISEQ$$_76748");

            modelBuilder.HasSequence("ISEQ$$_77306");

            modelBuilder.HasSequence("ISEQ$$_77317");
        }
    }
}
