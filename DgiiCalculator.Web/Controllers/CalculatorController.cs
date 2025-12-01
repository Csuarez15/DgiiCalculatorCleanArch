using DgiiCalculator.Application.DTOs;
using DgiiCalculator.Application.Interfaces;
using DgiiCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DgiiCalculator.Web.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorService _calculatorService;

        // Inyección de Dependencias: El constructor pide el servicio
        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        // GET: Muestra la pantalla inicial vacía
        [HttpGet]
        public IActionResult Index()
        {
            return View(new CalculatorViewModel());
        }

        // POST: Procesa el cálculo
        [HttpPost]
        public IActionResult Index(CalculatorViewModel model)
        {
            //Validación de formato
            if (!ModelState.IsValid)
            {
                // Si hay errores (ej: puso letras en el sueldo), devolvemos la vista con los errores.
                return View(model);
            }

            //Llamada al Servicio (Lógica de Negocio)
            var serviceResponse = _calculatorService.CalculateIsr(model.Input);

            //Manejo de Respuesta del Servicio
            if (serviceResponse.Success)
            {
                // Éxito: Asignamos el resultado al modelo
                model.Result = serviceResponse.Data;
                model.ErrorMessage = null;
            }
            else
            {
                // Error de Negocio (ej: número negativo lógico)
                // Agregamos el error al modelo para mostrarlo en rojo
                ModelState.AddModelError(string.Empty, serviceResponse.ErrorMessage);
                model.ErrorMessage = serviceResponse.ErrorMessage;
                model.Result = null;
            }

            return View(model);
        }
    }
}