using System.Text;

namespace jgarcesT3.Views;

public partial class Vista2 : ContentPage
{
    // Propiedades para almacenar los datos del contacto
    private string tipoIdentificacion;
    private string identificacion;
    private string nombres;
    private string apellidos;
    private DateTime fechaRegistro;
    private string correo;
    private decimal salario;
    private decimal aporteIESS;

    public Vista2(string tipoId, string id, string nombres, string apellidos,
                 DateTime fecha, string correo, decimal salario, decimal aporteIESS)
    {
        InitializeComponent();

        // Almacenar la información recibida
        this.tipoIdentificacion = tipoId;
        this.identificacion = id;
        this.nombres = nombres;
        this.apellidos = apellidos;
        this.fechaRegistro = fecha;
        this.correo = correo;
        this.salario = salario;
        this.aporteIESS = aporteIESS;

        // Cargar los datos en la vista
        CargarDatos();
    }

    private void CargarDatos()
    {
        // Mostrar información del contacto en los Labels
        lblTipoId.Text = tipoIdentificacion;
        lblIdentificacion.Text = identificacion;
        lblNombres.Text = nombres;
        lblApellidos.Text = apellidos;
        lblFecha.Text = fechaRegistro.ToString("dd/MM/yyyy");
        lblCorreo.Text = correo;
        lblSalario.Text = salario.ToString("C");

        // Mostrar el aporte al IESS
        lblAporteIESS.Text = aporteIESS.ToString("C");
    }

    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Crear el contenido del archivo
            string contenido = GenerarContenidoArchivo();

            // Obtener la ruta de almacenamiento
            string descargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string rutaArchivo = Path.Combine(descargas, $"Contacto_{identificacion}.txt");

            // Escribir el archivo
            File.WriteAllText(rutaArchivo, contenido);


            await DisplayAlert("Éxito", $"Archivo guardado exitosamente en:\n{rutaArchivo}", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al exportar: {ex.Message}", "Aceptar");
        }
    }

    private string GenerarContenidoArchivo()
    {
        // Crear un StringBuilder para construir el contenido del archivo
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("===== INFORMACIÓN DEL CONTACTO =====");
        sb.AppendLine();

        sb.AppendLine("--- DATOS PERSONALES ---");
        sb.AppendLine($"Tipo de Identificación: {tipoIdentificacion}");
        sb.AppendLine($"Número de Identificación: {identificacion}");
        sb.AppendLine($"Nombres: {nombres}");
        sb.AppendLine($"Apellidos: {apellidos}");
        sb.AppendLine();

        sb.AppendLine("--- INFORMACIÓN DE CONTACTO ---");
        sb.AppendLine($"Fecha de Registro: {fechaRegistro:dd/MM/yyyy}");
        sb.AppendLine($"Correo Electrónico: {correo}");
        sb.AppendLine();

        sb.AppendLine("--- INFORMACIÓN SALARIAL ---");
        sb.AppendLine($"Salario Mensual: {salario:C}");
        sb.AppendLine($"Aporte al IESS (9.45%): {aporteIESS:C}");
        sb.AppendLine();

        sb.AppendLine($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

        return sb.ToString();
    }
}
