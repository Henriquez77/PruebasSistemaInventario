using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebasSistemaInventario.Models;

namespace PruebasSistemaInventario.Controllers
{
    public class ProductoesController : Controller
    {
        //variable de tipo InventarioContext
        //readonly indica que la variable solo puede asignarse en el constructor de la clase y no puede modificarse después de su asignación inicial
        private readonly InventarioContext _context;
        //Este es el constructor de la clase ProductoesController, se utiliza para inicializar una nueva instancia de la clase ProductoesController.
        //El parámetro context es una instancia de la clase InventarioContext que se utiliza para interactuar con la base de datos en la aplicación web
        //En la línea _context = context; la instancia de InventarioContext pasada como parámetro se asigna a la variable readonly _context, que se utilizará en toda la clase DashboardController.
        public ProductoesController(InventarioContext context)
        {
            _context = context;

        }

        //Metodo asincrono Index, peticion GET
        //Se ejecuta cuando se accede al Index del controlador Productoes(Pagina principal de productos)
        public async Task<IActionResult> Index()
        {
              //Primero se valida que el modelo no sea nulo
              //Si el modelo es correcto retorna la vista enviando como argumento el listado de productos en base de datos 
              //Si el modelo es incorrecto se genera problema mostrando un mensaje
              return _context.Productos != null ? 
                          View(await _context.Productos.ToListAsync()) :
                          Problem("Entity set 'InventarioContext.Productos'  is null.");
        }

        //Metodo asincrono Details, peticion GET
        //Se ejecuta cuando se accede al Details del controlador Productoes(Cualquier boton de detalles del un producto)
        //ESta accion recibe un ID como parametro, ese ID es el del producto que estamos solicitando sus detalles 
        public async Task<IActionResult> Details(int? id)
        {
            //Primero se valida si el ID o el modelo Productos no sean nulos
            if (id == null || _context.Productos == null)
            {
                //Si son nulos retorna un NotFound()
                return NotFound();
            }

            //En caso de que ID y modelo sean correctos se procede a realizar la busqueda del producto seleccionado en la base de datos
            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            //Validamos si la DB nos devolvio algun registro
            if (producto == null)
            {
                //Si no existe el registro retorna un NotFound()
                return NotFound();
            }
            //Si se encontro el producto seleccionado, retorna la vista Details pasandole el modelo con el producto encontrado
            return View(producto);
        }

        //Metodo Create, peticion GET
        //Se ejecuta cuando se accede al Create del controlador Productoes(Pagina para insertar productos)
        public IActionResult Create()
        {
            //Mostrando la vista Create
            return View();

        }

        //Metodo asincrono Create, peticion POST
        //Este se ejecuta cuando se envia el formulario Create para insertar un nuevo producto
        //[ValidateAntiForgeryToken] se utiliza para proteger las solicitudes de formulario legítimas en una aplicación web y prevenir ataques CSRF
        //[HttpPost] se utiliza para marcar una acción del controlador como una acción que debe ser ejecutada solo cuando se recibe una solicitud HTTP POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Los parametros que espera esta accion ([Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad")] Producto producto) se utiliza para controlar qué propiedades del modelo pueden ser vinculadas en la solicitud HTTP
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad")] Producto producto)
        {
            //Primero se valida que el estado del modelo este completo o valido 
            if (ModelState.IsValid)
            {
                //Si es valido se realiza la insercion del producto ingresado en el formulario
                _context.Add(producto);
                await _context.SaveChangesAsync();
                //Luego se redirige hacia la vista Index del controlador Productoes(Pagina principal de productos)
                return RedirectToAction(nameof(Index));
            }
            //Si el producto no se pudo insertar se recarga la pagina
            return View(producto);
        }

        //Metodo asincrono Edit, peticion GET
        //Se ejecuta cuando se accede al Edit del controlador Productoes(editar un producto)
        //ESta accion recibe un ID como parametro, ese ID es el del producto que estamos solicitando editar 
        public async Task<IActionResult> Edit(int? id)
        {
            //Primero se valida si el ID o el modelo Productos no sean nulos
            if (id == null || _context.Productos == null)
            {
                //Si son nulos retorna un NotFound()
                return NotFound();
            }
            //En caso de que ID y modelo sean correctos se procede a realizar la busqueda del producto seleccionado en la base de datos
            var producto = await _context.Productos.FindAsync(id);
            //Validamos si la DB nos devolvio algun registro
            if (producto == null)
            {
                //Si son nulos retorna un NotFound()
                return NotFound();
            }
            //Si se encontro el producto seleccionado, retorna la vista Edit pasandole el modelo con el producto encontrado
            return View(producto);
        }

        //Metodo asincrono Edit, peticion POST
        //Este se ejecuta cuando se envia el formulario Edit para editar un producto
        //[ValidateAntiForgeryToken] se utiliza para proteger las solicitudes de formulario legítimas en una aplicación web y prevenir ataques CSRF
        //[HttpPost] se utiliza para marcar una acción del controlador como una acción que debe ser ejecutada solo cuando se recibe una solicitud HTTP POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Los parametros que espera esta accion ([Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad")] Producto producto) se utiliza para controlar qué propiedades del modelo pueden ser vinculadas en la solicitud HTTP
        //Y el int id, ese parametro es para hacer la edicion del producto correcto
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Descripcion,Precio,Cantidad")] Producto producto)
        {
            //Se valida que el id recivido por GET no sea diferente al id que esta cargado en el modelo
            if (id != producto.ProductoId)
            {
                //Si es diferente retorna un NotFound()
                return NotFound();
            }
            //Validando que el modelo cargado sea valido
            if (ModelState.IsValid)
            {
                //Si es valido se realiza una actualizacion de el producto 
                //Dentro de un try para asi evitar que la web se caiga por un intento fallido de actualizacion
                try
                {
                    //realizando actualizacion
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //En caso de no poder actualizar 
                    //Se valida que exista en la BD
                    if (!ProductoExists(producto.ProductoId))
                    {
                        //Si no existe retorna un NotFound()
                        return NotFound();
                    }
                    else
                    {
                        //Si el producto si existia throw nos muestra un error
                        //Ya que el problema pudo aver sido una excepcion en la BD
                        throw;
                    }
                }
                //Si el registro de actualizo se redirige hacia Index del controlador Productoes(Pagina principal de productos)
                return RedirectToAction(nameof(Index));
            }
            //Si el producto no se actualizo, retorna la vista Edit pasandole el modelo con el producto encontrado nuevamente
            return View(producto);
        }

        //Este código se utiliza para mostrar los detalles del registro de producto que se va a eliminar y para manejar errores si el registro no se puede encontrar en la base de datos.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        //este código se utiliza para mostrar los detalles del registro de producto que se va a eliminar y para manejar errores si el registro no se puede encontrar en la base de datos
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'InventarioContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //es un método auxiliar útil para evitar errores al buscar registros que no existen en la base de datos.
        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.ProductoId == id)).GetValueOrDefault();
        }
    }
}
