using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TareaSeman6
{
    public partial class Registro : Form
    {
        // Lista de objetos alumnos y profesores que se mostrarán en el DataGridView
        private BindingList<Alumno> listaAlumnos = new BindingList<Alumno>();
        private BindingList<Profesor> listaProfesores = new BindingList<Profesor>();
        private Alumno alumnoSeleccionado = null;
        private Profesor profesorSeleccionado = null;

        // Posición original del label y textbox de materia
        private int lblMateriaOriginalTop;
        private int txtMateriaOriginalTop;

        public Registro()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Configurar ComboBox con opciones Alumno y Profesor
            cmbTipoRegistro.Items.AddRange(new[] { "ALUMNO", "PROFESOR" });
            cmbTipoRegistro.SelectedIndex = 0; // seleccionar Alumno por defecto

            // Configurar DataGridView
            dataListado.AutoGenerateColumns = true;
            dataListado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataListado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Mostrar alumnos por defecto
            dataListado.DataSource = listaAlumnos;
        }

        
        private void cmbTipoRegistro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipo = cmbTipoRegistro.SelectedItem.ToString().ToUpper();

            // Campos comunes visibles para ambas clases.
            lblNombre.Visible = txtNombre.Visible = true;
            lblApellido.Visible = txtApellido.Visible = true;
            lblEdad.Visible = txtEdad.Visible = true;

            // Mostrar/ocultar campos según tipo de registro seleccionado

            if (tipo == "ALUMNO") // Si es Alumno, mostrar Carrera y ocultar Materia
            {
                lblCarrera.Visible = txtCarrera.Visible = true;
                lblMateria.Visible = txtMateria.Visible = false;

                // Asignar DataSource a lista de alumnos
                dataListado.DataSource = listaAlumnos;
            }
            else if (tipo == "PROFESOR") // Si es Profesor, mostrar Materia y ocultar Carrera
            {
                lblCarrera.Visible = txtCarrera.Visible = false;
                lblMateria.Visible = txtMateria.Visible = true;

                // Subir label y textbox de Materia a la posición de Carrera
                lblMateria.Top = lblCarrera.Top;
                txtMateria.Top = txtCarrera.Top;

                // Asignar DataSource a lista de profesores
                dataListado.DataSource = listaProfesores;
            }
        }

        // Validación de campos obligatorios y edad
        private bool Validar()
        {
            string tipo = cmbTipoRegistro.SelectedItem.ToString().ToUpper();

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Nombres son requeridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                MessageBox.Show("Apellidos son requeridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtApellido.Focus();
                return false;
            }

            if (!int.TryParse(txtEdad.Text, out int edad) || edad <= 0 || edad > 120)
            {
                MessageBox.Show("Ingrese una edad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEdad.Clear();
                txtEdad.Focus();
                return false;
            }

            // Validación de Carrera y Materia según el formulario que elija el usuario.
            if (tipo == "ALUMNO")
            {
                if (string.IsNullOrEmpty(txtCarrera.Text))
                {
                    MessageBox.Show("Carrera requerida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCarrera.Focus();
                    return false;
                }
            }
            else if (tipo == "PROFESOR")
            {
                if (string.IsNullOrEmpty(txtMateria.Text))
                {
                    MessageBox.Show("Materia requerida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMateria.Focus();
                    return false;
                }
            }

            return true;
        }

        // Botón Agregar un Alumno o Profesor según la selección.
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                string tipo = cmbTipoRegistro.SelectedItem.ToString().ToUpper();
                string nombre = txtNombre.Text.Trim().ToUpper();
                string apellido = txtApellido.Text.Trim().ToUpper();
                int edad = int.Parse(txtEdad.Text);

                if (tipo == "ALUMNO")
                {
                    // Agregar alumno a la lista
                    listaAlumnos.Add(new Alumno(nombre, apellido, edad, txtCarrera.Text.Trim().ToUpper()));
                }
                else if (tipo == "PROFESOR")
                {
                    // Agregar profesor a la lista
                    listaProfesores.Add(new Profesor(nombre, apellido, edad, txtMateria.Text.Trim().ToUpper()));
                }

                // Llamado al método LimpiarDatos
                LimpiarDatos();
            }
        }

        // Método para limpiar los campos
        public void LimpiarDatos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtEdad.Clear();
            txtCarrera.Clear();
            txtMateria.Clear();
            txtNombre.Focus();
        }

        // Botón para limpiar todos los campos.
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarDatos(); // Llamado al método LimpiarDatos
        }
        // Ese bloque de código detecta la fila seleccionada en el DataGridView y según el tipo de registro
        // carga la información de un Alumno o de un Profesor en el formulario.
        private void dataListado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string tipo = cmbTipoRegistro.SelectedItem.ToString().ToUpper();

            if (e.RowIndex >= 0 && e.RowIndex < dataListado.Rows.Count)
            {
                if (tipo == "ALUMNO")
                {
                    var alum = dataListado.Rows[e.RowIndex].DataBoundItem as Alumno;
                    CargarAlumno(alum);
                }
                else if (tipo == "PROFESOR")
                {
                    var prof = dataListado.Rows[e.RowIndex].DataBoundItem as Profesor;
                    CargarProfesor(prof);
                }
            }
        }
        // Funcion para cargar la información de alumno.
        private void CargarAlumno(Alumno alum)
        {
            alumnoSeleccionado = alum;

            if (alum != null)
            {
                txtNombre.Text = alum.nombre;
                txtApellido.Text = alum.apellido;
                txtCarrera.Text = alum.carrera;
                txtEdad.Text = alum.edad.ToString();
            }
        }

        // Funcion para cargar la información de profesor
        private void CargarProfesor(Profesor prof)
        {
            profesorSeleccionado = prof;

            if (prof != null)
            {
                txtNombre.Text = prof.nombre;
                txtApellido.Text = prof.apellido;
                txtMateria.Text = prof.materia;
                txtEdad.Text = prof.edad.ToString();
            }
        }

        //Método para actualizar automáticamente los campos del formulario cada vez que se selecciona una fila diferente en la tabla.
        private void dataListado_SelectionChanged(object sender, EventArgs e)
        {
            string tipo = cmbTipoRegistro.SelectedItem.ToString().ToUpper();

            if (dataListado.CurrentRow != null)
            {
                if (tipo == "ALUMNO")
                {
                    var alum = dataListado.CurrentRow.DataBoundItem as Alumno;
                    CargarAlumno(alum);
                }
                else if (tipo == "PROFESOR")
                {
                    var prof = dataListado.CurrentRow.DataBoundItem as Profesor;
                    CargarProfesor(prof);
                }
            }
        }

       // Botón para actualizar y modificar los datos de alumno o profesor.
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string tipo = cmbTipoRegistro.SelectedItem.ToString().ToUpper();

            if (dataListado.CurrentRow != null)
            {
                if (tipo == "ALUMNO")
                {
                    var alum = dataListado.CurrentRow.DataBoundItem as Alumno;
                    if (alum != null)
                    {
                        alum.nombre = txtNombre.Text.Trim().ToUpper();
                        alum.apellido = txtApellido.Text.Trim().ToUpper();
                        alum.edad = int.Parse(txtEdad.Text.Trim());
                        alum.carrera = txtCarrera.Text.Trim().ToUpper();
                        dataListado.Refresh();
                        MessageBox.Show("Alumno actualizado correctamente", "Actualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (tipo == "PROFESOR")
                {
                    var prof = dataListado.CurrentRow.DataBoundItem as Profesor;
                    if (prof != null)
                    {
                        prof.nombre = txtNombre.Text.Trim().ToUpper();
                        prof.apellido = txtApellido.Text.Trim().ToUpper();
                        prof.edad = int.Parse(txtEdad.Text.Trim());
                        prof.materia = txtMateria.Text.Trim().ToUpper();
                        dataListado.Refresh();
                        MessageBox.Show("Profesor actualizado correctamente", "Actualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        // Botón para eliminar los registros de alumno o profesor.
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataListado.CurrentRow != null) // Comprueba si hay fila seleccionada
            {
                var seleccionado = dataListado.CurrentRow.DataBoundItem;

                if (seleccionado != null) // Si hay una fila seleccionada continua.
                {
                    if (MessageBox.Show("¿Seguro que deseas eliminar este registro?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (seleccionado is Alumno alumno) //Si es de tipo Alumno, elimina al alumno.
                        {
                            listaAlumnos.Remove(alumno);
                            MessageBox.Show("Alumno eliminado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (seleccionado is Profesor profesor)  //caso contrario si es de tipo Profesor, elimina al profesor.
                        {
                            listaProfesores.Remove(profesor);
                            MessageBox.Show("Profesor eliminado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un registro para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

