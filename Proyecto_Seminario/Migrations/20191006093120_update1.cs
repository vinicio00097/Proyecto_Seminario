using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Proyecto_Seminario.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75348",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75351",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75354",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75357",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75360",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75363",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75366",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75368",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75371",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75373",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75375",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75378",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_75379",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_76748",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_77306",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateSequence(
                name: "ISEQ$$_77317",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.CreateTable(
                name: "ACCIONES",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_ACCION = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ACCIONES_PK", x => x.ID_ACCION);
                });

            migrationBuilder.CreateTable(
                name: "PASOS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PASO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR2(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PASOS_PK", x => x.ID_PASO);
                });

            migrationBuilder.CreateTable(
                name: "PASOSINSTANCIAS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PASOINSTANCIA = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR2(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PASOSINSTANCIAS_PK", x => x.ID_PASOINSTANCIA);
                });

            migrationBuilder.CreateTable(
                name: "PLANTILLAS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PLANTILLA = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR2(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PLANTILLAS_PK", x => x.ID_PLANTILLA);
                });

            migrationBuilder.CreateTable(
                name: "RANGOS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_RANGO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NIVEL = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RANGOS_PK", x => x.ID_RANGO);
                });

            migrationBuilder.CreateTable(
                name: "TIPOS_DATOS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_TIPO_DATO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TABLE1_PK", x => x.ID_TIPO_DATO);
                });

            migrationBuilder.CreateTable(
                name: "PLANTILLAS_PASOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PLANTILLA_PASO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    PLANTILLA = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    PASO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PLANTILLAS_PASOS_DETALLE_PK", x => x.ID_PLANTILLA_PASO);
                    table.ForeignKey(
                        name: "PLANTILLAS_PASOS_DETALLE_FK2",
                        column: x => x.PASO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PASOS",
                        principalColumn: "ID_PASO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PLANTILLAS_PASOS_DETALLE_FK1",
                        column: x => x.PLANTILLA,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PLANTILLAS",
                        principalColumn: "ID_PLANTILLA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRES = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    APELLIDOS = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    USUARIO_EMAIL = table.Column<string>(type: "VARCHAR2(30)", nullable: false),
                    RANGO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("USUARIOS_PK", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "USUARIOS_FK1",
                        column: x => x.RANGO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "RANGOS",
                        principalColumn: "ID_RANGO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PLANTILLAS_CAMPOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PLANTILLA_CAMPO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    PLANTILLA = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    NOMBRE_CAMPO = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    TIPO_DATO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PLANTILLAS_CAMPOS_DETALLE_PK", x => x.ID_PLANTILLA_CAMPO);
                    table.ForeignKey(
                        name: "PLANTILLAS_CAMPOS_DETALLE_FK1",
                        column: x => x.PLANTILLA,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PLANTILLAS",
                        principalColumn: "ID_PLANTILLA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PLANTILLAS_CAMPOS_DETALLE_FK2",
                        column: x => x.TIPO_DATO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "TIPOS_DATOS",
                        principalColumn: "ID_TIPO_DATO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "INSTANCIASPLANTILLAS",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_INSTANCIA_PLANTILLA = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    USUARIO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    ESTADO = table.Column<string>(type: "CHAR(1)", nullable: false),
                    INICIADA = table.Column<string>(type: "CHAR(1)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    FechaCreado = table.Column<DateTime>(nullable: true),
                    FechaIniciado = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("INSTANCIASPLANTILLAS_PK", x => x.ID_INSTANCIA_PLANTILLA);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_FK1",
                        column: x => x.USUARIO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "USUARIOS",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PASOS_USUARIOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PASO_USUARIO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    PLANTILLA_PASO_DETALLE = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    USUARIO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PLANTILLAS_PASOS_USUARIOS_DETALLE_PK2", x => x.ID_PASO_USUARIO);
                    table.ForeignKey(
                        name: "PLANTILLAS_PASOS_USUARIOS_DETALLE_FK1",
                        column: x => x.PLANTILLA_PASO_DETALLE,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PLANTILLAS_PASOS_DETALLE",
                        principalColumn: "ID_PLANTILLA_PASO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PLANTILLAS_PASOS_USUARIOS_DETALLE_FK2",
                        column: x => x.USUARIO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "USUARIOS",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PASOS_DATOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PASO_DATO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    PLANTILLA_CAMPO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    PASO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    SOLO_LECTURA = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PASOS_DATOS_DETALLE_PK", x => x.ID_PASO_DATO);
                    table.ForeignKey(
                        name: "PASOS_DATOS_DETALLE_FK1",
                        column: x => x.PASO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PASOS",
                        principalColumn: "ID_PASO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PASOS_DATOS_DETALLE_FK2",
                        column: x => x.PLANTILLA_CAMPO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PLANTILLAS_CAMPOS_DETALLE",
                        principalColumn: "ID_PLANTILLA_CAMPO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INSTANCIASPLANTILLAS_DATOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_INSTANCIA_PLANTILLA_DATO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    INSTANCIAPLANTILLA = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    NOMBRE_CAMPO = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    DATO_STRING = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    TIPO_DATO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    DATO_INTEGER = table.Column<decimal>(type: "NUMBER(38)", nullable: true),
                    DATO_DATE = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("INSTANCIASPLANTILLAS_DATOS_DETALLE_PK", x => x.ID_INSTANCIA_PLANTILLA_DATO);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_DATOS_DETALLE_FK1",
                        column: x => x.INSTANCIAPLANTILLA,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "INSTANCIASPLANTILLAS",
                        principalColumn: "ID_INSTANCIA_PLANTILLA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_DATOS_DETALLE_FK2",
                        column: x => x.TIPO_DATO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "TIPOS_DATOS",
                        principalColumn: "ID_TIPO_DATO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PLANTILLA_PASO_DETALLE = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    INSTANCIA_PLANTILLA = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    PASO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    ESTADO = table.Column<decimal>(type: "NUMBER(38)", nullable: true),
                    USUARIO_ACCION = table.Column<decimal>(type: "NUMBER(38)", nullable: true),
                    FECHA_INICIO = table.Column<DateTime>(type: "DATE", nullable: true),
                    FECHA_FIN = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("INSTANCIASPLANTILLAS_PASOS_DETALLE_PK", x => x.ID_PLANTILLA_PASO_DETALLE);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_PASOS_DETALLE_FK2",
                        column: x => x.ESTADO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "ACCIONES",
                        principalColumn: "ID_ACCION",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_PASOS_DETALLE_FK3",
                        column: x => x.INSTANCIA_PLANTILLA,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "INSTANCIASPLANTILLAS",
                        principalColumn: "ID_INSTANCIA_PLANTILLA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_PASOS_DETALLE_FK1",
                        column: x => x.PASO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PASOSINSTANCIAS",
                        principalColumn: "ID_PASOINSTANCIA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "INSTANCIASPLANTILLAS_PASOS_DETALLE_FK4",
                        column: x => x.USUARIO_ACCION,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "USUARIOS",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PASOSINSTANCIAS_DATOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PASOSINSTANCIAS_DATOS = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    INSTANCIA_PLANTILLA_DATO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    PASO = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    SOLO_LECTURA = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PASOSINSTANCIAS_DATOS_DETALLE_PK", x => x.ID_PASOSINSTANCIAS_DATOS);
                    table.ForeignKey(
                        name: "PASOSINSTANCIAS_DATOS_DETALLE_FK1",
                        column: x => x.INSTANCIA_PLANTILLA_DATO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "INSTANCIASPLANTILLAS_DATOS_DETALLE",
                        principalColumn: "ID_INSTANCIA_PLANTILLA_DATO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PASOSINSTANCIAS_DATOS_DETALLE_FK2",
                        column: x => x.PASO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "PASOSINSTANCIAS",
                        principalColumn: "ID_PASOINSTANCIA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PASOSINSTANCIAS_USUARIOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                columns: table => new
                {
                    ID_PASOS_USUARIOS = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    PLANTILLA_PASO_DETALLE = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    USUARIO = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PASOS_USUARIOS_DETALLE_PK", x => x.ID_PASOS_USUARIOS);
                    table.ForeignKey(
                        name: "PASOS_USUARIOS_DETALLE_FK1",
                        column: x => x.PLANTILLA_PASO_DETALLE,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                        principalColumn: "ID_PLANTILLA_PASO_DETALLE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PASOS_USUARIOS_DETALLE_FK2",
                        column: x => x.USUARIO,
                        principalSchema: "C##PROYECTO_SEMINARIO",
                        principalTable: "USUARIOS",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ACCIONES_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "ACCIONES",
                column: "ID_ACCION",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "INSTANCIASPLANTILLAS_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS",
                column: "ID_INSTANCIA_PLANTILLA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_USUARIO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS",
                column: "USUARIO");

            migrationBuilder.CreateIndex(
                name: "INSTANCIASPLANTILLAS_DATOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_DATOS_DETALLE",
                column: "ID_INSTANCIA_PLANTILLA_DATO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_DATOS_DETALLE_INSTANCIAPLANTILLA",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_DATOS_DETALLE",
                column: "INSTANCIAPLANTILLA");

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_DATOS_DETALLE_TIPO_DATO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_DATOS_DETALLE",
                column: "TIPO_DATO");

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_PASOS_DETALLE_ESTADO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                column: "ESTADO");

            migrationBuilder.CreateIndex(
                name: "INSTANCIASPLANTILLAS_PASOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                column: "ID_PLANTILLA_PASO_DETALLE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_PASOS_DETALLE_INSTANCIA_PLANTILLA",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                column: "INSTANCIA_PLANTILLA");

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_PASOS_DETALLE_PASO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                column: "PASO");

            migrationBuilder.CreateIndex(
                name: "IX_INSTANCIASPLANTILLAS_PASOS_DETALLE_USUARIO_ACCION",
                schema: "C##PROYECTO_SEMINARIO",
                table: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                column: "USUARIO_ACCION");

            migrationBuilder.CreateIndex(
                name: "PASOS_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS",
                column: "ID_PASO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PASOS_DATOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS_DATOS_DETALLE",
                column: "ID_PASO_DATO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PASOS_DATOS_DETALLE_PASO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS_DATOS_DETALLE",
                column: "PASO");

            migrationBuilder.CreateIndex(
                name: "IX_PASOS_DATOS_DETALLE_PLANTILLA_CAMPO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS_DATOS_DETALLE",
                column: "PLANTILLA_CAMPO");

            migrationBuilder.CreateIndex(
                name: "PASOS_USUARIOS_DETALLE_PK2",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS_USUARIOS_DETALLE",
                column: "ID_PASO_USUARIO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PASOS_USUARIOS_DETALLE_PLANTILLA_PASO_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS_USUARIOS_DETALLE",
                column: "PLANTILLA_PASO_DETALLE");

            migrationBuilder.CreateIndex(
                name: "IX_PASOS_USUARIOS_DETALLE_USUARIO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOS_USUARIOS_DETALLE",
                column: "USUARIO");

            migrationBuilder.CreateIndex(
                name: "PASOSINSTANCIAS_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS",
                column: "ID_PASOINSTANCIA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PASOSINSTANCIAS_DATOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS_DATOS_DETALLE",
                column: "ID_PASOSINSTANCIAS_DATOS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PASOSINSTANCIAS_DATOS_DETALLE_INSTANCIA_PLANTILLA_DATO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS_DATOS_DETALLE",
                column: "INSTANCIA_PLANTILLA_DATO");

            migrationBuilder.CreateIndex(
                name: "IX_PASOSINSTANCIAS_DATOS_DETALLE_PASO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS_DATOS_DETALLE",
                column: "PASO");

            migrationBuilder.CreateIndex(
                name: "PASOS_USUARIOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS_USUARIOS_DETALLE",
                column: "ID_PASOS_USUARIOS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PASOSINSTANCIAS_USUARIOS_DETALLE_PLANTILLA_PASO_DETALLE",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS_USUARIOS_DETALLE",
                column: "PLANTILLA_PASO_DETALLE");

            migrationBuilder.CreateIndex(
                name: "IX_PASOSINSTANCIAS_USUARIOS_DETALLE_USUARIO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PASOSINSTANCIAS_USUARIOS_DETALLE",
                column: "USUARIO");

            migrationBuilder.CreateIndex(
                name: "PLANTILLAS_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS",
                column: "ID_PLANTILLA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PLANTILLAS_CAMPOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS_CAMPOS_DETALLE",
                column: "ID_PLANTILLA_CAMPO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PLANTILLAS_CAMPOS_DETALLE_PLANTILLA",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS_CAMPOS_DETALLE",
                column: "PLANTILLA");

            migrationBuilder.CreateIndex(
                name: "IX_PLANTILLAS_CAMPOS_DETALLE_TIPO_DATO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS_CAMPOS_DETALLE",
                column: "TIPO_DATO");

            migrationBuilder.CreateIndex(
                name: "PLANTILLAS_PASOS_DETALLE_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS_PASOS_DETALLE",
                column: "ID_PLANTILLA_PASO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PLANTILLAS_PASOS_DETALLE_PASO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS_PASOS_DETALLE",
                column: "PASO");

            migrationBuilder.CreateIndex(
                name: "IX_PLANTILLAS_PASOS_DETALLE_PLANTILLA",
                schema: "C##PROYECTO_SEMINARIO",
                table: "PLANTILLAS_PASOS_DETALLE",
                column: "PLANTILLA");

            migrationBuilder.CreateIndex(
                name: "RANGOS_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "RANGOS",
                column: "ID_RANGO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "TABLE1_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "TIPOS_DATOS",
                column: "ID_TIPO_DATO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "USUARIOS_PK",
                schema: "C##PROYECTO_SEMINARIO",
                table: "USUARIOS",
                column: "ID_USUARIO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_RANGO",
                schema: "C##PROYECTO_SEMINARIO",
                table: "USUARIOS",
                column: "RANGO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PASOS_DATOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PASOS_USUARIOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PASOSINSTANCIAS_DATOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PASOSINSTANCIAS_USUARIOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PLANTILLAS_CAMPOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PLANTILLAS_PASOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "INSTANCIASPLANTILLAS_DATOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "INSTANCIASPLANTILLAS_PASOS_DETALLE",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PASOS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PLANTILLAS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "TIPOS_DATOS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "ACCIONES",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "INSTANCIASPLANTILLAS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "PASOSINSTANCIAS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "USUARIOS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropTable(
                name: "RANGOS",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75348",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75351",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75354",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75357",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75360",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75363",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75366",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75368",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75371",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75373",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75375",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75378",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_75379",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_76748",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_77306",
                schema: "C##PROYECTO_SEMINARIO");

            migrationBuilder.DropSequence(
                name: "ISEQ$$_77317",
                schema: "C##PROYECTO_SEMINARIO");
        }
    }
}
