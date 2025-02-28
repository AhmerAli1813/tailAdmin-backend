using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenApiReqLogs_AppsLogs_logId",
                table: "OpenApiReqLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenApiReqLogs_logId",
                table: "OpenApiReqLogs");

            migrationBuilder.DropColumn(
                name: "logId",
                table: "OpenApiReqLogs");

            migrationBuilder.DropColumn(
                name: "CINC",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppsLogs");

            migrationBuilder.RenameColumn(
                name: "MotherName",
                table: "AspNetUsers",
                newName: "Designation");

            migrationBuilder.RenameColumn(
                name: "FolioNumber",
                table: "AspNetUsers",
                newName: "CNIC");

            migrationBuilder.AlterColumn<long>(
                name: "TransTraceId",
                table: "OpenApiReqLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "OpenApiReqLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DC_Code",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LineManagerId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionHeadId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenApiReqLogs_TransTraceId",
                table: "OpenApiReqLogs",
                column: "TransTraceId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LineManagerId",
                table: "AspNetUsers",
                column: "LineManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegionHeadId",
                table: "AspNetUsers",
                column: "RegionHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_LineManagerId",
                table: "AspNetUsers",
                column: "LineManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RegionHeadId",
                table: "AspNetUsers",
                column: "RegionHeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Regions_RegionId",
                table: "AspNetUsers",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenApiReqLogs_AppsLogs_TransTraceId",
                table: "OpenApiReqLogs",
                column: "TransTraceId",
                principalTable: "AppsLogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_LineManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RegionHeadId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Regions_RegionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenApiReqLogs_AppsLogs_TransTraceId",
                table: "OpenApiReqLogs");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_OpenApiReqLogs_TransTraceId",
                table: "OpenApiReqLogs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LineManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegionHeadId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "OpenApiReqLogs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DC_Code",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LineManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegionHeadId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Designation",
                table: "AspNetUsers",
                newName: "MotherName");

            migrationBuilder.RenameColumn(
                name: "CNIC",
                table: "AspNetUsers",
                newName: "FolioNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TransTraceId",
                table: "OpenApiReqLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "logId",
                table: "OpenApiReqLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CINC",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppsLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OpenApiReqLogs_logId",
                table: "OpenApiReqLogs",
                column: "logId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenApiReqLogs_AppsLogs_logId",
                table: "OpenApiReqLogs",
                column: "logId",
                principalTable: "AppsLogs",
                principalColumn: "Id");
        }
    }
}
