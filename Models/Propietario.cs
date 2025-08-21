
using System.ComponentModel.DataAnnotations;

public class Persona
{
    public int id_propietario { get; set; }
    public int dni_propietario { get; set; }
    public string contrasena_propietario { get; set; }
    public string nombre_propietario { get; set; }
    public string apellido_propietario { get; set; }
    public string email_propietario { get; set; }
    public string telefono_propietario { get; set; }

    public Persona(int id_propietario, int dni_propietario, string contrasena_propietario, string nombre_propietario, string apellido_propietario, string email_propietario, string telefono_propietario)
    {
        this.id_propietario = id_propietario;
        this.dni_propietario = dni_propietario;
        this.contrasena_propietario = contrasena_propietario;
        this.nombre_propietario = nombre_propietario;
        this.apellido_propietario = apellido_propietario;
        this.email_propietario = email_propietario;
        this.telefono_propietario = telefono_propietario;
    }





}