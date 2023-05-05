using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebasSistemaInventario.ViewModels;
using PruebasSistemaInventario.Models;

namespace PruebasSistemaInventario.Controllers
{
    public class DashboardController : Controller
    {
        private readonly InventarioContext _context;

        public DashboardController(InventarioContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            TempData["Usuario"] = TempData["Usuario"];
            var _Productos = _context.Productos.Count(); 
            var _Compras = _context.Compras.Count();
            var _Ventas = _context.Ventas.Count();
            var _Usuarios = _context.Usuarios.Count();
            var _Proveedores = _context.Proveedores.Count();
            var _Roles = _context.Roles.Count();
            var Modelo = new ViewModelCountAll
            {
                Productos = _Productos,
                Compras = _Compras,
                Ventas = _Ventas,
                Usuarios = _Usuarios,
                Proveedores = _Proveedores,
                Roles = _Roles
                    
            };
            return View(Modelo);
        }
    }
}
