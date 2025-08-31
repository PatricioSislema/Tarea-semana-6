using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TareaSeman6
{
    internal class Persona
    {
        // Atributos
        public string nombre {  get; set; }
        public string apellido{  get; set; }
        public int edad {  get; set; }

        //Constructor
        public Persona(string nombre, string apellido, int edad) 
        { 
            this.nombre = nombre;
            this.apellido = apellido;
            this.edad = edad;
        }

        // Método para mostrar datos
        public virtual void MostrarDatos()
        {
            Console.WriteLine($"Nombre: {nombre}");
            Console.WriteLine($"Apellido: {apellido}");
            Console.WriteLine($"Edad: {edad}");
        }
    }
}
