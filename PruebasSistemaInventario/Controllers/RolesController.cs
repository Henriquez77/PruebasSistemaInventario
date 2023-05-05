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
    public class RolesController : Controller
    {
        //este código sirve para proporcionar acceso a la base de datos de la aplicación en la clase "RolesController", lo que permite al controlador leer y escribir datos relacionados con los roles de usuario en la aplicación.
        private readonly InventarioContext _context;

        public RolesController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Roles
        //este código maneja las solicitudes HTTP GET para la página principal de la aplicación, accede a la entidad "Roles" de la base de datos a través del objeto de contexto "_context" y devuelve una vista que muestra los datos de esa entidad si existe, de lo contrario, devuelve un resultado de problema.
        public async Task<IActionResult> Index()
        {
              return _context.Roles != null ? 
                          View(await _context.Roles.ToListAsync()) :
                          Problem("Entity set 'InventarioContext.Roles'  is null.");
        }

        // GET: Roles/Details/5
        //este código maneja las solicitudes HTTP GET para mostrar los detalles de un rol específico en la aplicación, busca el rol en la base de datos utilizando el identificador único pasado en el parámetro "id" y devuelve una vista que muestra los detalles del rol si se encuentra, de lo contrario, devuelve un resultado de "NotFound".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RolId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        //este código maneja las solicitudes HTTP GET para mostrar el formulario de creación de un nuevo rol en la aplicación y devuelve una vista que muestra el formulario correspondiente.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        //  este código maneja las solicitudes HTTP POST para crear un nuevo rol en la aplicación, agrega el nuevo rol a la base de datos si el modelo es válido, muestra mensajes de error si el modelo no es válido y redirecciona al usuario a la página principal de la aplicación después de agregar el nuevo rol a la base de datos.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RolId,Nombre")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        // este código maneja las solicitudes HTTP GET para mostrar el formulario de edición de un rol existente en la aplicación, busca el rol correspondiente utilizando el identificador único recibido como parámetro y devuelve una vista que muestra el formulario de edición correspondiente si el rol se encuentra, o una respuesta "NotFound()" si el rol no se encuentra.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // este código maneja las solicitudes HTTP POST para actualizar un rol existente en la aplicación, actualiza el rol en la base de datos si el modelo es válido y redirecciona al usuario a la página principal de la aplicación después de actualizar el rol, o muestra mensajes de error si el modelo no es válido o el recurso solicitado no se pudo encontrar.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RolId,Nombre")] Role role)
        {
            if (id != role.RolId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RolId))
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
            return View(role);
        }

        // GET: Roles/Delete/5
        //este código maneja las solicitudes HTTP GET para mostrar la vista de confirmación de eliminación de un rol existente en la aplicación, buscando el rol correspondiente en la base de datos y devolviendo la vista "Delete" con los detalles del rol como parámetro.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RolId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        // este código maneja las solicitudes HTTP POST para eliminar un rol existente en la aplicación, busca el rol correspondiente en la base de datos, elimina el rol si se encuentra y guarda los cambios en la base de datos. Luego, redirige al usuario a la acción "Index" del controlador actual.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'InventarioContext.Roles'  is null.");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //este método comprueba si existe un rol con el identificador "id" en la base de datos utilizando el objeto de contexto "_context". El método devuelve "true" si existe un rol con el identificador especificado y "false" en caso contrario.
        private bool RoleExists(int id)
        {
          return (_context.Roles?.Any(e => e.RolId == id)).GetValueOrDefault();
        }
    }
}
