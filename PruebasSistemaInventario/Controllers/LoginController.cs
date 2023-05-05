using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PruebasSistemaInventario.Models;
using PruebasSistemaInventario.ViewModels;

namespace PruebasSistemaInventario.Controllers
{
    public class LoginController : Controller
    {
        //variable de tipo InventarioContext
        //readonly indica que la variable solo puede asignarse en el constructor de la clase y no puede modificarse después de su asignación inicial
        private readonly InventarioContext _context;

        //Este es el constructor de la clase LoginController, se utiliza para inicializar una nueva instancia de la clase LoginController.
        //El parámetro context es una instancia de la clase InventarioContext que se utiliza para interactuar con la base de datos en la aplicación web
        //En la línea _context = context; la instancia de InventarioContext pasada como parámetro se asigna a la variable readonly _context, que se utilizará en toda la clase LoginController.
        public LoginController(InventarioContext context)
        {
            _context = context;
        }

        //Metodo Index, peticion GET
        //Se ejecuta cuando se accede al Index del controlador Login(Pagina principal login)
        public IActionResult Index()
        {
            return View();
        }


        //Metodo asincrono Index, peticion POST
        //Este se ejecuta cuando se envia el formulario login para validar el usuario ingresado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Nombre,Contrasenia")] Usuario user)
        {
            //Listando todos los registros de la tabla Usuarios de la base de datos
            List<Usuario> bucar = _context.Usuarios.ToList();

            //Validando que si existan registros
            if (bucar != null)
            {
                //Recorriendo los registros encontrados uno por uno
                foreach (var item in bucar)
                {
                    //Validando cada registro, si usuario y contraseña coincide con la enviada en el login 
                    //El usuario es redirigido hacia la pagina principal  
                    if(item.Nombre == user.Nombre && item.Contrasenia == user.Contrasenia)
                    {
                        //Redireccionamiento 
                        //La funccion RedirectToAction toma como argumentos el nombre del método de acción al que se debe redirigir al usuario y el nombre del controlador que contiene ese método de acción                         
                        return RedirectToAction(nameof(Index), "Dashboard");
                    }
                }
            }

            //Si usuario y contraseña no fueron encotrados se carga un mensaje de error en un ViewData con clave UserIncorret
            ViewData["UserIncorret"] = "Usuario o Contraseña incorrectos";
            //Y se regresa al login mostrando el mensaje anterior
            return View();
        }
        //Metodo Registrar, peticion GET
        //Se ejecuta cuando se accede a Registrar del controlador Login(Pagina de registro de usuarios)
        public IActionResult Registrar()
        {
            return View();
        }

        //Metodo asincrono Registrar, peticion POST
        //Este se ejecuta cuando se envia el formulario de registro de usuario para guardar un usuario nuevo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Registrar([Bind("Nombre","Contrasenia")] Usuario user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Login");
            }
            return View("Registrar");
        }
    }
}
