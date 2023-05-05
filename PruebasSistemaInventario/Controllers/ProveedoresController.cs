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
    public class ProveedoresController : Controller
    {
        //este código se utiliza para crear una instancia del controlador ProveedoresController y proporcionarle acceso a la base de datos InventarioContext para que pueda interactuar con ella.
        private readonly InventarioContext _context;

        public ProveedoresController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        //este código se utiliza para mostrar una lista de proveedores almacenados en la base de datos y para manejar errores si la lista de proveedores es nula.
        public async Task<IActionResult> Index()
        {
              return _context.Proveedores != null ? 
                          View(await _context.Proveedores.ToListAsync()) :
                          Problem("Entity set 'InventarioContext.Proveedores'  is null.");
        }

        // GET: Proveedores/Details/5
        // este código se utiliza para buscar y mostrar los detalles de un proveedor específico en la base de datos y para manejar errores si el proveedor no se puede encontrar.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // GET: Proveedores/Create
        //este código se utiliza para mostrar la vista "Create" que contiene el formulario para crear un nuevo proveedor.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        //este código se utiliza para agregar un nuevo proveedor a la base de datos después de que se haya enviado el formulario de creación de proveedor. Si el modelo es válido, se agrega el nuevo proveedor y se redirige al usuario a la lista de proveedores. Si el modelo no es válido, se muestra la vista "Create" con los errores de validación.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorId,Nombre,Telefono,Pais")] Proveedore proveedore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedore);
        }

        // GET: Proveedores/Edit/5
        //este código se utiliza para mostrar la vista de edición para un proveedor existente. Si el proveedor no se encuentra, el método devuelve una respuesta HTTP 404 Not Found. Si el proveedor se encuentra, el método devuelve la vista de edición para el proveedor.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }
            return View(proveedore);
        }

        // POST: Proveedores/Edit/5
        // Este código corresponde a un método POST para actualizar un proveedor existente. Toma como parámetros el id del proveedor que se desea actualizar y un objeto proveedor con los campos actualizados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProveedorId,Nombre,Telefono,Pais")] Proveedore proveedore)
        {
            if (id != proveedore.ProveedorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedoreExists(proveedore.ProveedorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proveedore);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // POST: Proveedores/Delete/5
        //Este código corresponde al controlador de la vista "Delete" para eliminar un proveedor. La función recibe como parámetro el id del proveedor a eliminar y comprueba que no sea nulo y que exista en la base de datos. Si el id es nulo o la lista de proveedores en el contexto es nula, se devuelve una respuesta NotFound. Si se encuentra el proveedor en la base de datos, se muestra la vista con los detalles del proveedor y se espera a que el usuario confirme la eliminación.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedores == null)
            {
                return Problem("Entity set 'InventarioContext.Proveedores'  is null.");
            }
            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore != null)
            {
                _context.Proveedores.Remove(proveedore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Este método sirve para verificar si existe un proveedor en la base de datos con un cierto ID. Toma el ID como argumento y busca en la tabla de proveedores del contexto de la base de datos para ver si existe un proveedor con ese ID. Si existe, devuelve true; de lo contrario, devuelve false.
        private bool ProveedoreExists(int id)
        {
          return (_context.Proveedores?.Any(e => e.ProveedorId == id)).GetValueOrDefault();
        }
    }
}
