using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebasSistemaInventario.ViewModels;
using PruebasSistemaInventario.Models;

namespace PruebasSistemaInventario.Controllers
{
    public class DashboardController : Controller
    {
        //variable de tipo InventarioContext
        //readonly indica que la variable solo puede asignarse en el constructor de la clase y no puede modificarse después de su asignación inicial
        private readonly InventarioContext _context;
        //Este es el constructor de la clase DashboardController, se utiliza para inicializar una nueva instancia de la clase DashboardController.
        //El parámetro context es una instancia de la clase InventarioContext que se utiliza para interactuar con la base de datos en la aplicación web
        //En la línea _context = context; la instancia de InventarioContext pasada como parámetro se asigna a la variable readonly _context, que se utilizará en toda la clase DashboardController.
        public DashboardController(InventarioContext context)
        {
            _context = context;
            
        }
        //Metodo Index, peticion GET
        //Se ejecuta cuando se accede al Index del controlador Dashboard(Pagina principal administrativa)
        public IActionResult Index()
        {
            //Estas variables las utilizamos para hacer un conteo de los registros que tienen cada una de las tablas que creamos
            var _Productos = _context.Productos.Count(); 
            var _Compras = _context.Compras.Count();
            var _Ventas = _context.Ventas.Count();
            var _Usuarios = _context.Usuarios.Count();
            var _Proveedores = _context.Proveedores.Count();
            var _Roles = _context.Roles.Count();
            //Agregamos esos conteos a un ViewModel para enviarlo hacia la vista Dashboard(Pagina principal administrativa)
            var Modelo = new ViewModelCountAll
            {
                Productos = _Productos,
                Compras = _Compras,
                Ventas = _Ventas,
                Usuarios = _Usuarios,
                Proveedores = _Proveedores,
                Roles = _Roles
                    
            };
            //Mostrando la vista Dashboard(Pagina principal administrativa)
            return View(Modelo);
        }
    }
}
