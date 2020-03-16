using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calculadora.Models;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// apresenta a view com a calculadora no primeiro pedido - HTTP GET
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //levar o resultado do 'visor' para a view (valor inicial)
            ViewBag.Visor = "0";
            ViewBag.LimparVisor = "false";
            return View();
        }

        /// <summary>
        /// View, em modo 'POST'?
        /// </summary>
        /// <param name="visor"> mostra o valor no visor</param>
        /// <param name="bt">indica o valor da tecla clicada</param>
        /// <param name="operando">guarda o valor do primeiro operando para utilização na operação</param>
        /// <param name="operador">operador a utilizar na operação</param>
        /// <param name="limparVisor">Identifica se o visor deve ser, ou nao, limpo</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string visor, string bt, string operando, string operador, bool limparVisor) // ele reconhece os "name" que demos no Index
        {
            switch (bt)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    if (visor == "0" || limparVisor) visor = bt;
                    else visor += bt; // visor = visor + bt; 
                    // impedir o visor de ser limpo
                    limparVisor = false;
                    break;
                case "+/-":
                    // inverter o valor do Visor
                    //Pode ser feito de 2 formas: 
                    // - multiplicar por -1 => implica converter o valor do visor de string para numero É A QUE VAMOS APLICAR
                    // - ou entao processar a string .StartsWith().ToString().Substring().Length
                    visor = Convert.ToDouble(visor) * -1 + ""; //acrescentamos "" no fim pq 'visor' é string
                    break;
                case ",":
                    if (!visor.Contains(",")) visor += bt;
                    break;
                case "+":
                case "-":
                case "x":
                case ":":
                case "=":
                    //primeira vez que carrego num operador
                    if (operador != null)
                    {
                        // vars auxiliares
                        double operando1 = Convert.ToDouble(operando);
                        double operando2 = Convert.ToDouble(visor);

                        switch (operador)
                        {
                            case "+":
                                visor = operando1 + operando2 + "";
                                break;
                            case "-":
                                visor = operando1 - operando2 + "";
                                break;
                            case "x":
                                visor = operando1 * operando2 + "";
                                break;
                            case ":":
                                visor = operando1 / operando2 + "";
                                break;
                        }

                    }
                    //guardar os valores para memória futura
                    if (bt != "=") operador = bt;
                    else operador = "";
                    operando = visor;
                    limparVisor = true;
                    break;
                case "C":
                    visor = "0";
                    operador = "";
                    operando = "";
                    limparVisor = true;
                    break;
            
        }
            //levar o resultado do 'visor' para a view
            ViewBag.Visor = visor;
            ViewBag.Operador = operador;
            ViewBag.Operando = operando;
            ViewBag.LimparVisor = limparVisor+"";

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
