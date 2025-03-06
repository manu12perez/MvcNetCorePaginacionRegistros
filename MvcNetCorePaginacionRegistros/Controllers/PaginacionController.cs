using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MvcNetCorePaginacionRegistros.Models;
using MvcNetCorePaginacionRegistros.Repositories;

namespace MvcNetCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        public RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> PaginarRegistroVistaDepartamento(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();

            int siguiente = posicion.Value + 1;
            if(siguiente > numRegistros)
            {
                siguiente = numRegistros;
            }

            int anterior = posicion.Value - 1;
            if(anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ULTIMO"] = numRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;

            VistaDepartamento departamentos =
                await this.repo.GetVistaDepartamentoAsync(posicion.Value);

            return View(departamentos);
        }

        public async Task<IActionResult> PaginarGrupoVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            
            int numRegistros = 
                await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();

            ViewData["REGISTROS"] = numRegistros;

            List<VistaDepartamento> departamentos =
                await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);

            return View(departamentos);
        }

        public async Task<IActionResult> PaginarGrupoDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            int numRegistros =
                await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();

            ViewData["REGISTROS"] = numRegistros;

            List<Departamento> departamentos =
                await this.repo.GetGrupoDepartamentosAsync(posicion.Value);

            return View(departamentos);
        }

        public async Task<IActionResult> PaginarGrupoEmpleados(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            int numRegistros =
                await this.repo.GetEmpleadosCountAsync();

            ViewData["REGISTROS"] = numRegistros;

            List<Empleado> empleados =
                await this.repo.GetGrupoEmpleadosAsync(posicion.Value);

            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosOficio(int? posicion, string oficio)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosOficioAsync(posicion.Value, oficio);
                int registros = await this.repo.GetEmpleadosOficioCountAsync(oficio);

                ViewData["REGISTROS"] = registros;
                ViewData["OFICIO"] = oficio;

                return View(empleados);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmpleadosOficio(string oficio)
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosOficioAsync(1, oficio);
            int registros = await this.repo.GetEmpleadosOficioCountAsync(oficio);

            ViewData["REGISTROS"] = registros;
            ViewData["OFICIO"] = oficio;

            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosOficioOut(int? posicion, string oficio)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                ModelEmpleadosOficio model = await this.repo.GetEmpleadosOficioOutAsync(posicion.Value, oficio);

                ViewData["REGISTROS"] = model.NumeroRegistros;
                ViewData["OFICIO"] = oficio;

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmpleadosOficioOut(string oficio)
        {
            ModelEmpleadosOficio model = await this.repo.GetEmpleadosOficioOutAsync(1, oficio);

            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["OFICIO"] = oficio;

            return View(model);
        }

        public async Task<IActionResult> Details(int id, int posicion)
        {
            Departamento departamento = await this.repo.FindDepartamento(id);
            Empleado empleado = await this.repo.GetEmpleadoDepartamento(posicion, id);
            ModelDepartamentoEmpleado departamentoEmpleado = new ModelDepartamentoEmpleado()
            {
                Empleado = empleado,
                Departamento = departamento
            };

            int numRegistros = await this.repo.GetNumeroRegistros(id);
            ViewData["REGISTROS"] = numRegistros;            

            int siguiente = posicion + 1;
            if (siguiente > numRegistros)
            {
                siguiente = numRegistros;
            }

            int anterior = posicion - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ULTIMO"] = numRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;

            return View(departamentoEmpleado);
        }
    }
}
