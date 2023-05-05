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
    public class UsuariosController : Controller
    {
        //este código es esencial para la funcionalidad del controlador de usuarios, ya que proporciona acceso al contexto de la base de datos que se necesita para realizar operaciones en ella.
        private readonly InventarioContext _context;

        public UsuariosController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        // este método es responsable de recuperar todos los usuarios de la base de datos junto con su información de rol asociada y enviarlos a la vista para mostrarlos al usuario.
        public async Task<IActionResult> Index()
        {
            var inventarioContext = _context.Usuarios.Include(u => u.Rol);
            return View(await inventarioContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        //este método es responsable de recuperar los detalles de un usuario específico y su rol asociado y enviarlos a la vista para mostrarlos al usuario.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        //este método es responsable de mostrar una vista de formulario que permita al usuario ingresar información para crear un nuevo usuario y también de pasar información adicional a la vista, en este caso, una lista de roles disponibles.
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "RolId");
            return View();
        }

        // POST: Usuarios/Create
        // este método es responsable de procesar la solicitud de creación de un nuevo usuario a través del formulario de creación y agregarlo a la base de datos si los datos son válidos. Si el modelo no es válido, se vuelve a mostrar la vista de creación de usuario con los datos ingresados por el usuario y un mensaje de error.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Contrasenia,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "RolId", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        //este método es responsable de mostrar una vista de formulario que permita al usuario modificar los datos de un usuario existente y también de pasar información adicional a la vista, en este caso, una lista de roles disponibles.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // este método se utiliza para procesar la solicitud de edición de un usuario existente y actualizar los datos del usuario en la base de datos. Si hay algún error de validación o de concurrencia de base de datos, el método devuelve la vista de edición correspondiente con un mensaje de error. Si se guardan los cambios correctamente, el método redirige al usuario a la vista de lista de usuarios.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Contrasenia,RolId")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "RolId", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        //este código es una acción del controlador que se utiliza para mostrar los detalles de un usuario específico que se desea eliminar de la base de datos.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        // este fragmento de código es la acción que se ejecuta después de que el usuario ha confirmado que desea eliminar un registro de usuario. Esta acción elimina el registro correspondiente de la base de datos y luego redirige al usuario a la página que muestra la lista actualizada de usuarios.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'InventarioContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //esta función privada se utiliza para verificar si un registro de usuario con un determinado ID existe en la base de datos. Esto se utiliza en el controlador de la acción "Delete" para garantizar que solo se intente eliminar un registro que realmente existe en la base de datos.
        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }
    }
}
