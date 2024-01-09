using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;
using System.Security.Policy;
using Org.BouncyCastle.Asn1.Tsp;

namespace IeIAPI
{
    [ApiController]
    [Route("api/carga")]

    public class ControladorCarga : ControllerBase
    {
        private static string host = "monorail.proxy.rlwy.net";
        private static int port = 25026;
        private static string database = "railway";
        private static string user = "root";
        private static string password = "HA2A2baGAEH2B1f-4A42b1g6c2EbGaB4";
        private static string connectionString = $"Server={host};Port={port};Database={database};User Id={user};Password={password};CharSet=utf8mb4;";


        [HttpGet]
        [Route("CAT")]
        public IActionResult ProcesarCATDatos()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();



                    int[] numeros = new int[4];
                    numeros[0] = 60; // COdigo localidad
                    numeros[1] = 60; // Buenardos
                    numeros[2] = 0; // Corregidos
                    Console.WriteLine("\n-------------------------------");
                    Console.WriteLine("Inicio de extraccion 1");
                    string url = "https://raw.githubusercontent.com/CamarillaGuanxi/IEIback/main/IeIAPI/IeIAPI/CAT.xml";

                    XDocument doc = XDocument.Load(url);

                    string json = Extractor2XML.Extractor2(numeros, doc);

                    return Ok(json);
                }
            }
            catch (Exception ex)
            {
                // Imprimir detalles de la excepción en la consola
                Console.WriteLine($"Error en el método PostDatos: {ex.GetType().FullName}");
                Console.WriteLine($"Mensaje de la excepción: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // Retornar un código de estado 500 con un mensaje de error genérico
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }




    }
}
