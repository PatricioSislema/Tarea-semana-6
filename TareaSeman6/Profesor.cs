using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TareaSeman6
{
    internal class Profesor:Persona
    {
        // Atributo propio
        public string materia {  get; set; }
        public Profesor(string nombre, string apellido, int edad, string materia) 
            // Atributos heredados de la clase Persona
            : base (nombre, apellido,edad)
        {
            this.materia = materia;
        }
        public override void MostrarDatos()
        {
            //Método MostraDatos sobreescrita
            base.MostrarDatos();
            Console.WriteLine($"Materia: {materia}");
        }
    }
}
