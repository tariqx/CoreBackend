using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreBackend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblProduct",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(80)", nullable: true),
                    Brand = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProduct", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "tblProduct",
                columns: new[] { "ID", "Brand", "Name" },
                values: new object[] { 1, "Tesla", "Model Y" });

            migrationBuilder.InsertData(
                table: "tblProduct",
                columns: new[] { "ID", "Brand", "Name" },
                values: new object[] { 2, "Honda", "Accord" });

            migrationBuilder.InsertData(
                table: "tblProduct",
                columns: new[] { "ID", "Brand", "Name" },
                values: new object[] { 3, "Toyota", "Corolla" });

            migrationBuilder.CreateIndex(
                name: "IX_tblProduct_ID_Brand_Name",
                table: "tblProduct",
                columns: new[] { "ID", "Brand", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblProduct");
        }
    }
}
