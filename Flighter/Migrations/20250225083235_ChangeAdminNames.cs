﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAdminNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Roles]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
