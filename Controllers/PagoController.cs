using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;




namespace inmobiliaria.Controllers
{

    public class PagoController : Controller
    {

        private readonly RepositorioPago repo;

        public PagoController(RepositorioPago repo)
        {
            this.repo = repo;
        }

    }



}