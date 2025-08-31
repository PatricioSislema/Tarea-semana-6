using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TareaSeman6
{
    internal class Alumno:Persona
    {
        // Atributo propio
     public string carrera {  get; set; }
        public Alumno(string nombre, string apellido, int edad, string carrera)
            // Atributos heredados de la clase Persona
            : base(nombre, apellido, edad)
        {
            this.carrera = carrera;
        }
        public override void MostrarDatos()
        {
            //Método MostraDatos sobreescrita
            base.MostrarDatos();
            Console.WriteLine($"Carrera: {carrera}");
        }
    }
}
