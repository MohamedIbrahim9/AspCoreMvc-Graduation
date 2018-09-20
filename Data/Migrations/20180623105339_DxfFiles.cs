using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Graduation.Data.Migrations
{
    public partial class DxfFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DxfFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FK_ApplicatioUserId = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    RelativeFilePath = table.Column<string>(nullable: true),
                    StaticFilePath = table.Column<string>(nullable: true),
                    UploadedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DxfFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DxfFiles_AspNetUsers_FK_ApplicatioUserId",
                        column: x => x.FK_ApplicatioUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DxfFiles_FK_ApplicatioUserId",
                table: "DxfFiles",
                column: "FK_ApplicatioUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DxfFiles");
        }
    }
}
