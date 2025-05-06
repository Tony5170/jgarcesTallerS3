using System.Text.RegularExpressions;

namespace jgarcesT3.Views;

public partial class Vista1 : ContentPage
{
    // Propiedades para almacenar los datos del contacto
    private string tipoIdentificacion;
    private string identificacion;
    private string nombres;
    private string apellidos;
    private DateTime fechaRegistro;
    private string correo;
    private decimal salario;

    public Vista1()
    {
        try
        {
            InitializeComponent();

            // Establecer fecha actual en el DatePicker
            pkfecha.Date = DateTime.Now;

            // Agregar handler para el cambio en el Picker
            pkid.SelectedIndexChanged += Pkid_SelectedIndexChanged;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error de inicializaci�n", $"Error: {ex.Message}", "Aceptar");
        }
    }

    private void Pkid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Limpiar el campo de identificaci�n al cambiar el tipo
            txtidentificacion.Text = string.Empty;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al cambiar tipo de identificaci�n: {ex.Message}", "Aceptar");
        }
    }

    private void txtidentificacion_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                return;

            // Verificar que el tipo de identificaci�n ha sido seleccionado
            if (pkid.SelectedIndex == -1)
            {
                DisplayAlert("Advertencia", "Primero seleccione un tipo de identificaci�n", "Aceptar");
                txtidentificacion.Text = string.Empty;
                return;
            }

            string tipoId = pkid.SelectedItem.ToString();
            string valor = e.NewTextValue;

            // Validar seg�n el tipo de identificaci�n
            switch (tipoId)
            {
                case "CI":
                    // Solo permitir n�meros y m�ximo 10 d�gitos
                    if (!Regex.IsMatch(valor, @"^\d{0,10}$"))
                    {
                        DisplayAlert("Error", "La c�dula debe contener solo n�meros y m�ximo 10 d�gitos", "Aceptar");
                        txtidentificacion.Text = e.OldTextValue;
                    }
                    break;
                case "RUC":
                    // Solo permitir n�meros y m�ximo 13 d�gitos
                    if (!Regex.IsMatch(valor, @"^\d{0,13}$"))
                    {
                        DisplayAlert("Error", "El RUC debe contener solo n�meros y m�ximo 13 d�gitos", "Aceptar");
                        txtidentificacion.Text = e.OldTextValue;
                    }
                    break;
                case "Pasaporte":
                    // No hay restricciones espec�ficas para el pasaporte
                    break;
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al validar identificaci�n: {ex.Message}", "Aceptar");
        }
    }

    // M�todo para validar que en el campo de salario solo se ingresen n�meros
    private void txtsalario_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                return;

            // Verificar que solo se ingresen n�meros y punto decimal
            if (!Regex.IsMatch(e.NewTextValue, @"^\d+(\.\d*)?$"))
            {
                DisplayAlert("Error", "El salario debe contener solo n�meros", "Aceptar");
                txtsalario.Text = e.OldTextValue ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al validar salario: {ex.Message}", "Aceptar");
        }
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar que todos los campos est�n completos
            if (!ValidarCampos())
                return;

            // Almacenar la informaci�n en las propiedades
            tipoIdentificacion = pkid.SelectedItem?.ToString();
            identificacion = txtidentificacion.Text;
            nombres = txtnombre.Text;
            apellidos = txtapellido.Text;
            fechaRegistro = pkfecha.Date;
            correo = txtcorreo.Text;

            // Parsear el salario con manejo de errores
            if (!decimal.TryParse(txtsalario.Text, out decimal salarioValue))
            {
                await DisplayAlert("Error", "El salario ingresado no es v�lido", "Aceptar");
                return;
            }

            salario = salarioValue;

            // Calcular el aporte al IESS (9.45% del salario)
            decimal aporteIESS = salario * 0.0945m;

            // Crear la instancia de Vista2 y pasarle los par�metros
            var vista2 = new Vista2(
                tipoIdentificacion,
                identificacion,
                nombres,
                apellidos,
                fechaRegistro,
                correo,
                salario,
                aporteIESS);

            // Navegar a la Vista2
            await Navigation.PushAsync(vista2);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "Aceptar");
        }
    }

    private bool ValidarCampos()
    {
        try
        {
            // Validar tipo de identificaci�n
            if (pkid.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Debe seleccionar un tipo de identificaci�n", "Aceptar");
                return false;
            }

            // Validar identificaci�n
            if (string.IsNullOrEmpty(txtidentificacion.Text))
            {
                DisplayAlert("Error", "Debe ingresar su identificaci�n", "Aceptar");
                return false;
            }

            string tipoId = pkid.SelectedItem.ToString();
            switch (tipoId)
            {
                case "CI":
                    if (txtidentificacion.Text.Length != 10)
                    {
                        DisplayAlert("Error", "La c�dula debe tener 10 d�gitos", "Aceptar");
                        return false;
                    }
                    break;
                case "RUC":
                    if (txtidentificacion.Text.Length != 13)
                    {
                        DisplayAlert("Error", "El RUC debe tener 13 d�gitos", "Aceptar");
                        return false;
                    }
                    break;
            }

            // Validar nombres
            if (string.IsNullOrEmpty(txtnombre.Text))
            {
                DisplayAlert("Error", "Debe ingresar sus nombres", "Aceptar");
                return false;
            }

            // Validar apellidos
            if (string.IsNullOrEmpty(txtapellido.Text))
            {
                DisplayAlert("Error", "Debe ingresar sus apellidos", "Aceptar");
                return false;
            }

            // Validar correo
            if (string.IsNullOrEmpty(txtcorreo.Text) || !ValidarCorreo(txtcorreo.Text))
            {
                DisplayAlert("Error", "Debe ingresar un correo electr�nico v�lido", "Aceptar");
                return false;
            }

            // Validar salario
            if (string.IsNullOrEmpty(txtsalario.Text) || !decimal.TryParse(txtsalario.Text, out decimal salario) || salario <= 0)
            {
                DisplayAlert("Error", "Debe ingresar un salario v�lido mayor a cero", "Aceptar");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al validar campos: {ex.Message}", "Aceptar");
            return false;
        }
    }

    private bool ValidarCorreo(string correo)
    {
        try
        {
            // Validaci�n de correo electr�nico usando expresi�n regular
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(correo, pattern);
        }
        catch
        {
            return false;
        }
    }
}