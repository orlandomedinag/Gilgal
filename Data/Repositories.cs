using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class Repositories
    {
        private readonly string _connectionString;
        public Repositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<long>> GetPersonalmovAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"
                    SELECT IDPersonal 
                    FROM Personals 
                    WHERE Activo = 1 AND ResponsableInventario LIKE '%Almacén%'";
                return (await db.QueryAsync<long>(sql)).AsList();
            }
        }

        public async Task<List<Movimiento>> GetMovimientosAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"
                    SELECT * 
                    FROM Movimientos 
                    WHERE Activo = 1 AND IDTipoMovimiento = 2";
                return (await db.QueryAsync<Movimiento>(sql)).AsList();
            }
        }

    }
}
