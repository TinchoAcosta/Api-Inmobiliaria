
using Microsoft.AspNetCore.Mvc;

/* 
El Controlador de las clases lo que hace es recibir las peticiones del usuario y redireccionarlas a las vistas correspondientes.
En este caso, el controlador PropietarioController maneja las solicitudes relacionadas con los propietarios.
El repositorio PropietarioRepository se encarga de las operaciones de acceso a datos relacionadas con los propietarios.
 */
public class PropietarioController : Controller
{
    private readonly RepositorioPropietario repo;

    public PropietarioController(RepositorioPropietario repo)
    {
        this.repo = new RepositorioPropietario();
    }

    public IActionResult Index()
    {
        // Aquí se llamaría al repositorio para obtener los propietarios
        // var propietarios = repo.ObtenerTodos();
        // return View(propietarios);
        //esto redireccionaria al index de la vista Propietario

        return View(); // Placeholder, replace with actual data retrieval
    }


}
