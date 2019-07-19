using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Hawhaw_Nomor_IsUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Hawhaw_Nomor",
                table: "Hawhaw",
                column: "Nomor",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hawhaw_Nomor",
                table: "Hawhaw");
        }
    }
}
